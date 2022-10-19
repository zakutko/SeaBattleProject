using BLL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WEB.ViewModels;

namespace WEB.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null) return Unauthorized();

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded)
            {
                return BadRequest("Failed login");
            }
            return Ok(CreateUserObject(user));
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserViewModel>> Register(RegisterViewModel model)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == model.Email))
            {
                return BadRequest("Email taken");
            }
            if (await _userManager.Users.AnyAsync(x => x.UserName == model.UserName))
            {
                return BadRequest("UserName taken");
            }

            var user = new AppUser
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.UserName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)   
            {
                return CreateUserObject(user);
            }
            return BadRequest("Problem registering user");
        }

        [HttpGet]
        public async Task<ActionResult<UserViewModel>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

            return CreateUserObject(user);
        }

        private UserViewModel CreateUserObject(AppUser user)
        {
            return new UserViewModel
            {
                DisplayName = user.DisplayName,
                Token = _tokenService.CreateToken(user),
                UserName = user.UserName
            };
        }
    }
}
