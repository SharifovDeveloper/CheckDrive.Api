using CheckDrive.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;

[ApiController]
[Route("api/login")]
public class AuthorizationController(IAuthorizationService authorizationService) : Controller
{
    private readonly IAuthorizationService _authorizationService = authorizationService;

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(string login, string password)
    {
        var token = await _authorizationService.Login(login, password);

        HttpContext.Response.Cookies.Append("tasty-cookies", token);

        return Ok(token);
    }
}
