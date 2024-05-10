using CheckDrive.Domain.DTOs.Dispatcher;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.ResourceParameters;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DispatchersController : Controller
{
    private readonly IDispatcherService _dispatcherService;

    public DispatchersController(IDispatcherService dispatcherService)
    {
        _dispatcherService = dispatcherService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DispatcherDto>>> GetDispatchersAsync(
        [FromQuery] DispatcherResourceParameters dispatcherResource)
    {
        var dispatchers = await _dispatcherService.GetDispatchersAsync(dispatcherResource);

        return Ok(dispatchers);
    }
    [HttpGet("{id}", Name = "GetDispatcherByIdAsync")]
    public async Task<ActionResult<DispatcherDto>> GetDispatcherByIdAsync(int id)
    {
        var dispatcher = await _dispatcherService.GetDispatcherByIdAsync(id);

        if (dispatcher is null)
            return NotFound($"Dispatcher with id: {id} does not exist.");

        return Ok(dispatcher);
    }
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] DispatcherForCreateDto dispatcherforCreateDto)
    {
        var createdDispatcher = await _dispatcherService.CreateDispatcherAsync(dispatcherforCreateDto);

        return CreatedAtAction(nameof(GetDispatcherByIdAsync), new { createdDispatcher.Id }, createdDispatcher);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] DispatcherForUpdateDto dispatcherforUpdateDto)
    {
        if (id != dispatcherforUpdateDto.Id)
        {
            return BadRequest(
                $"Route id: {id} does not match with parameter id: {dispatcherforUpdateDto.Id}.");
        }

        var updateDispatcher = await _dispatcherService.UpdateDispatcherAsync(dispatcherforUpdateDto);

        return Ok(updateDispatcher);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _dispatcherService.DeleteDispatcherAsync(id);

        return NoContent();
    }
}

