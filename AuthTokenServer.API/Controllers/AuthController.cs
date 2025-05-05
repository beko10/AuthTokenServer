using AuthTokenServer.BusinessLayer.Abstract;
using AuthTokenServer.EntityLayer.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthTokenServer.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authService;

    public AuthController(IAuthenticationService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateToken(LoginDto loginDto)
    {
        var result = await _authService.CreateTokenAsync(loginDto);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        return BadRequest(result.Message);
    }

    [HttpPost]
    public IActionResult CreateTokenByClient(ClientLoginDto clientLoginDto)
    {
        var result = _authService.CreateTokenByClient(clientLoginDto);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        return BadRequest(result.Message);
    }

    [HttpPost]
    public async Task<IActionResult> RevokeRefreshToken(RefreshTokenDto refreshTokenDto)
    {
        var result = await _authService.RevokeRefreshToken(refreshTokenDto.RefreshToken);
        if (result.IsSuccess)
        {
            return Ok(result.Message);
        }
        return BadRequest(result.Message);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTokenByRefreshToken(RefreshTokenDto refreshTokenDto)
    {
        var result = await _authService.CreateTokenByRefreshToken(refreshTokenDto.RefreshToken);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        return BadRequest(result.Message);
    }
}
