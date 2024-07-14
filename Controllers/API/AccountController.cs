using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Aulas.Areas.Identity.Pages.Account;
using CPTWorkouts.Models;
using CPTWorkouts.Data;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace CPTWorkouts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
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

                    return Ok(new { email = model.Email, role = roles.FirstOrDefault() });
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

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel.InputModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new { message = "Failed to register user." });
            }

            var role = model.IsTreinador ? "Treinador" : "Cliente";
            await _userManager.AddToRoleAsync(user, role);

            if (model.IsTreinador)
            {
                var treinador = new Treinadores
                {
                    UserID = user.Id,
                    Nome = model.Treinador.Nome,
                    DataNascimento = model.Treinador.DataNascimento,
                    Telemovel = model.Treinador.Telemovel,
                    TreinadorID = model.TreinadorID
                };
                _context.Treinadores.Add(treinador);
            }
            else
            {
                var cliente = new Clientes
                {
                    UserID = user.Id,
                    Nome = model.Cliente.Nome,
                    DataNascimento = model.Cliente.DataNascimento,
                    Telemovel = model.Cliente.Telemovel
                };
                _context.Clientes.Add(cliente);
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered successfully." });
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound($"User with email '{model.Email}' not found.");
            }

            var codeBytes = WebEncoders.Base64UrlDecode(model.Code);
            var code = Encoding.UTF8.GetString(codeBytes);

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                // Optionally, you can log the user in after email confirmation
                // await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok(new { message = "Email confirmed successfully." });
            }
            else
            {
                return BadRequest("Email confirmation failed.");
            }
        }
    }

}

