using AuthTokenServer.BusinessLayer.Abstract;
using AuthTokenServer.EntityLayer.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthTokenServer.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
    {
        var result = await _userService.CreateUserAsync(createUserDto);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        return BadRequest(result.Message);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUser()
    {
        var result = await _userService.GetUserByUserNameAsync(HttpContext.User.Identity.Name);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        return BadRequest(result.Message);
    }

    [HttpPost("CreateUserRoles/{id}")]
    public async Task<IActionResult> CreateUserRoles(string id)
    {
        var result = await _userService.CreateUserRoles(id);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result.Message);
    }
}
