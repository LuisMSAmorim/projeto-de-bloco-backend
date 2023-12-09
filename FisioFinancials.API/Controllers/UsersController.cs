using FisioFinancials.API.Config;
using FisioFinancials.API.Services;
using FisioFinancials.API.Services.Interfaces;
using FisioFinancials.Domain.Model.DTOs;
using FisioFinancials.Domain.Model.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace FisioFinancials.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private UserManager<User> _userManager;
    private ITokenService _tokenService;
    private readonly JwtBearerTokenSettings _jwtBearerTokenSettings;

    public UsersController
    (
       UserManager<User> userManager,
       ITokenService tokenService,
       IOptions<JwtBearerTokenSettings> jwtTokenOptions
    )
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _jwtBearerTokenSettings = jwtTokenOptions.Value;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] CreateUserDTO userDTO)
    {
        User user = new()
        {
            Email = userDTO.Email,
            UserName = userDTO.Username,
            FirstName = userDTO.FirstName,
            LastName = userDTO.LastName
        };

        IdentityResult result = await _userManager.CreateAsync(user, userDTO.Password);

        if (!result.Succeeded)
        {
            var dictionary = new ModelStateDictionary();
            foreach (IdentityError error in result.Errors)
            {
                dictionary.AddModelError(error.Code, error.Description);
            }

            return new BadRequestObjectResult(new { Message = "User Registration Failed", Errors = dictionary });
        }

        return Ok(new { Message = "User Registration Successful" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(LoginUserDTO loginUserDTO)
    {
        User user;

        if (!ModelState.IsValid
            || loginUserDTO == null
            || (user = await ValidateUser(loginUserDTO)) == null)
        {
            return new BadRequestObjectResult(new { Message = "Login failed" });
        }

        var token = _tokenService.GenerateToken(user, _jwtBearerTokenSettings);

        return Ok(new { Token = token, Message = "Success" });
    }

    private async Task<User> ValidateUser(LoginUserDTO loginUserDTO)
    {
        var user = await _userManager.FindByNameAsync(loginUserDTO.UserName);
        if (user != null)
        {
            var result = _userManager.PasswordHasher
                                     .VerifyHashedPassword(user, user.PasswordHash, loginUserDTO.Password);

            return result == PasswordVerificationResult.Failed ? null : user;
        }

        return null;
    }

}
