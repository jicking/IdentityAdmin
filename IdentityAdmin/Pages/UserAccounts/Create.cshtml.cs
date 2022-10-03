using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace IdentityAdmin.Pages.UserAccounts;

public class CreateModel : PageModel {
	private readonly UserManager<IdentityUser> userManager;


	public CreateModel(
		UserManager<IdentityUser> userManager
		) {
		this.userManager = userManager;
	}

	[BindProperty]
	public CreateInputModel Input { get; set; }

	public void OnGet() {

	}

	public async Task<IActionResult> OnPost() {
		if (!ModelState.IsValid) {
			return Page();
		}
		var existingAccount = userManager.Users.AsNoTracking().FirstOrDefault(u => u.NormalizedEmail == Input.Email.ToUpper());
		if (existingAccount != null) {
			ModelState.AddModelError("Email", "Email already exist on db");
			return Page();
		}

		// save
		var user = new IdentityUser(Input.Email) {
			UserName = Input.Email,
			Email = Input.Email,
			EmailConfirmed = true
		};
		await userManager.CreateAsync(user, Input.Password);

		return RedirectToPage("./Index");
	}
}


public class CreateInputModel {

	public Guid Id { get; set; }

	[EmailAddress]
	[Required]
	public string Email { get; set; }

	[Required]
	[RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Invalid password")]
	[DataType(DataType.Password)]
	public string Password { get; set; }

	[Compare("Password")]
	[DataType(DataType.Password)]
	public string VerifyPassword { get; set; }
}
