using CheckDrive.ApiContracts.Account;
using CheckDrive.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;

[ApiController]
[Route("api/login")]
public class AuthorizationController(IAuthorizationService authorizationService) : ControllerBase
{
    private readonly IAuthorizationService _authorizationService = authorizationService;

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(AccountForLoginDto accountForLogin)
    {
        var token = await _authorizationService.Login(accountForLogin.Login, accountForLogin.Password);

        if (token == null)
        {
            return Unauthorized("Invalid email or password");
        }

        HttpContext.Response.Cookies.Append("tasty-cookies", token);

        return Ok(token);
    }
}
