using AsapSystems.BLL.Dtos.Auth;
using AsapSystems.BLL.Services.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AsapSystems.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly TokenValidationParameters _tokenValidationParameters;

    public AuthController(IAuthService authService, TokenValidationParameters tokenValidationParameters)
    {
        _authService = authService;
        _tokenValidationParameters = tokenValidationParameters;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> RegisterAsync(RegisterDto registerDto)
    {
        var result = await _authService.RegisterAsync(registerDto);

        return Ok(result);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync(LoginDto loginDto)
    {
        var result = await _authService.LoginAsync(loginDto);

        return Ok(result);
    }

    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
    {
        var result = await _authService.RefreshTokenAsync(refreshTokenDto, _tokenValidationParameters);

        return Ok(result);
    }

    [HttpPost("Logout")]
    public async Task<IActionResult> LogoutAsync(LogoutDto logoutDto)
    {
        var result = await _authService.LogoutAsync(logoutDto);

        return Ok(result);
    }
}
