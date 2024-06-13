using CheckDrive.Domain.Interfaces.Hubs;
using CheckDrive.Services.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CheckDrive.Api.Controllers
{
    public class TestingChatHubContoller : Controller
    {
        private readonly IChatHub _hub;
        public TestingChatHubContoller(IChatHub hub)
        {
            _hub = hub;
        }

        [HttpPost("sendPrivateRequest")]
        public async Task<IActionResult> SendPrivateRequest(string userId, string message)
        {
            try
            {
                await _hub.SendPrivateRequest(userId, message);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
