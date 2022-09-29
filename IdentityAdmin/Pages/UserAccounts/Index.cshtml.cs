using IdentityAdmin.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IdentityAdmin.Pages.Accounts;

public class IndexModel : PageModel {
	private readonly UserManager<IdentityUser> userManager;

	public IndexModel(
		UserManager<IdentityUser> userManager
		) {
		this.userManager = userManager;
	}

	public List<Microsoft.AspNetCore.Identity.IdentityUser> UserAccounts { get; set; }

	public void OnGet() {
		UserAccounts = userManager.Users.AsNoTracking().ToList();
	}
}
