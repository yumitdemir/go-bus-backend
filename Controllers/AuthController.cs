using go_bus_backend.Dto;
using go_bus_backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace go_bus_backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ITokenRepository _tokenRepository;

    public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
    {
        _userManager = userManager;
        _tokenRepository = tokenRepository;
    }


    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register(RegisterRequestDto registerRequestDto)
    {
        var identityUser = new IdentityUser
        {
            UserName = registerRequestDto.UserName,
            Email = registerRequestDto.UserName,
        };
        var identityResault = await _userManager.CreateAsync(identityUser, registerRequestDto.Password);
        if (identityResault.Succeeded)
        {
            if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
            {
                identityResault = await _userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
                if (identityResault.Succeeded)
                {
                    return Ok("User registered");
                }
            }
        }

        return BadRequest("Something went wrong");
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
    {
        var user = await _userManager.FindByEmailAsync(loginRequestDto.UserName);
        if (user != null)
        {
            var checkPasswordResult = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            if (checkPasswordResult)
            {
                var roles = await _userManager.GetRolesAsync(user);

                if (roles != null)
                {
                    var jwtToken = _tokenRepository.CreateJwtToken(user, roles.ToList());
                    var loginResponseDto = new LoginResponseDto
                    {
                        JwtToken = jwtToken
                    };
                    return Ok(loginResponseDto);
                }

            }
        }

        return BadRequest("Username or password is incorrect");
    }
}