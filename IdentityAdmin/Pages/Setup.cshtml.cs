using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityAdmin.Pages;

public class SetupModel : PageModel {
	private readonly IServiceProvider _serviceProvider;
	public SetupModel(IServiceProvider serviceProvider) {
		_serviceProvider = serviceProvider;
	}
	public async Task OnGet() {
		var defaultUserEmails = new List<string>() {
			"admin@admin.admin"
		};


		await Initialize(_serviceProvider, defaultUserEmails);
	}

	public static async Task Initialize(
		IServiceProvider serviceProvider,
		List<string> userEmails) {

		var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();
		var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

		foreach (var email in userEmails) {
			var userPassword = GenerateSecurePassword();
			var userId = await EnsureUser(userManager, email, userPassword);

			// NotifyUser(userName, userPassword);
		}

		// Roles
		// roleManager.CreateAsync()
	}

	private static string GenerateSecurePassword() {
		return "@dminPassw0rd";
	}

	private static async Task<string> EnsureUser(UserManager<IdentityUser> userManager,
												 string email, string userPassword) {
		var user = await userManager.FindByNameAsync(email);

		if (user == null) {
			user = new IdentityUser(email) {
				Email = email,
				EmailConfirmed = true
			};
			await userManager.CreateAsync(user, userPassword);
		}

		return user.Id;
	}
}


