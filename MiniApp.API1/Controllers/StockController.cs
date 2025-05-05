using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MiniApp.API1.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class StockController : ControllerBase
{
    [HttpGet]
    public IActionResult GetStock()
    {
        var userName = HttpContext.User.Identity.Name;
        var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
        return Ok(new
        {
            UserName = userName,
            UserId = userIdClaim?.Value
        });
    }
}
