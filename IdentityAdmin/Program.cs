using IdentityAdmin.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;

namespace IdentityAdmin;
public class Program {
	public static void Main(string[] args) {
		var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
		logger.Debug("init IdentityAdmin =======");

		try {
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddDatabaseDeveloperPageExceptionFilter();
			builder.Services.AddRazorPages();

			// IDENTITY
			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(connectionString));
			//options.UseInMemoryDatabase("IdentityAdminDb"));
			builder.Services.AddDefaultIdentity<IdentityUser>()
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>();


			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment()) {
				app.UseMigrationsEndPoint();
			}
			else {
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapRazorPages();

			app.Run();
		}
		catch (Exception exception) {
			// NLog: catch setup errors
			logger.Error(exception, "Stopped IdentityAdmin because of exception =======");
			throw;
		}
		finally {
			// Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
			NLog.LogManager.Shutdown();
		}
	}
}