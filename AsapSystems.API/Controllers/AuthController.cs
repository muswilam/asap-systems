using AsapSystems.BLL.Dtos.Auth;
using AsapSystems.BLL.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace AsapSystems.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> RegisterAsync(RegisterDto registerDto)
    {
        var result = await _authService.RegisterAsync(registerDto);

        return Ok(result);
    }
}
