using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityAdmin.Pages;

public class SetupModel : PageModel {
	private readonly IServiceProvider _serviceProvider;
	public SetupModel(IServiceProvider serviceProvider) {
		_serviceProvider = serviceProvider;
	}
	public async Task OnGet() {
		var defaultUsers = new List<string>() {
			"admin@admin.admin"
		};

		await Initialize(_serviceProvider, defaultUsers);
	}

	public static async Task Initialize(IServiceProvider serviceProvider,
									List<string> userList) {
		var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

		foreach (var userName in userList) {
			var userPassword = GenerateSecurePassword();
			var userId = await EnsureUser(userManager, userName, userPassword);

			// NotifyUser(userName, userPassword);
		}
	}

	private static string GenerateSecurePassword() {
		return "@dminPassw0rd";
	}

	private static async Task<string> EnsureUser(UserManager<IdentityUser> userManager,
												 string userName, string userPassword) {
		var user = await userManager.FindByNameAsync(userName);

		if (user == null) {
			user = new IdentityUser(userName) {
				EmailConfirmed = true
			};
			await userManager.CreateAsync(user, userPassword);
		}

		return user.Id;
	}
}


