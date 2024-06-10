using CheckDrive.ApiContracts.Dashboard;
using CheckDrive.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;

//[Authorize(Policy = "Admin")]
[Route("api/dashboard")]
[ApiController]
public class DashboardController : Controller
{
    private readonly IDashboardService _dashboardService;
    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }
    [HttpGet]
    public async Task<ActionResult<DashboardDto>> GetDashboard() => Ok(await _dashboardService.GetDashboard());
}

