using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Aulas.Areas.Identity.Pages.Account;
using System.Linq;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel.InputModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var roles = await _userManager.GetRolesAsync(user);

                return Ok(new { email = model.Email, role = roles.FirstOrDefault() }); // Return user email and role
            }
            if (result.RequiresTwoFactor)
            {
                return BadRequest(new { message = "Requires two factor authentication" });
            }
            if (result.IsLockedOut)
            {
                return BadRequest(new { message = "User account locked out." });
            }
            else
            {
                return Unauthorized(new { message = "Invalid login attempt." });
            }
        }

        return BadRequest(ModelState);
    }
}
