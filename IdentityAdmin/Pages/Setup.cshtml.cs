using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityAdmin.Pages;

public class SetupModel : PageModel {
	private readonly IServiceProvider _serviceProvider;
	public SetupModel(IServiceProvider serviceProvider) {
		_serviceProvider = serviceProvider;
	}
	public async Task OnGet() {
		await Initialize(_serviceProvider);
	}

	public static async Task Initialize(
		IServiceProvider serviceProvider
		) {

		var defaultUserRoles = new List<string>() {
			"admin",
			"manager",
			"user"
		};

		var defaultUsers = new Dictionary<string, string>() {
			{"admin", "admin@identityadmin.net" },
			{"manager", "manager@identityadmin.net" },
			{"user", "user@identityadmin.net" },
		};

		var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
		var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

		// Create default roles
		foreach (var roleName in defaultUserRoles) {
			var role = await roleManager.FindByNameAsync(roleName);
			if (role == null) {
				role = new IdentityRole(roleName);
				await roleManager.CreateAsync(role);
			}
		}

		// Create default users
		foreach (var user in defaultUsers) {
			var userPassword = GenerateSecurePassword();
			var userId = await CreateUser(userManager, roleManager, user.Key, user.Value, userPassword);

			// NotifyUser(userName, userPassword);
		}

		// Roles
		// roleManager.CreateAsync()
	}

	private static string GenerateSecurePassword() {
		return "s@fePassw0rd";
	}

	private static async Task<string> CreateUser(
		UserManager<IdentityUser> userManager,
		RoleManager<IdentityRole> roleManager,
		string username,
		string email,
		string userPassword) {

		var user = await userManager.FindByNameAsync(email);

		if (user == null) {
			user = new IdentityUser(email) {
				UserName = email,
				Email = email,
				EmailConfirmed = true
			};
			await userManager.CreateAsync(user, userPassword);

			if (username == "admin") {
				await userManager.AddToRoleAsync(user, "admin");
			}
		}

		return user.Id;
	}
}


