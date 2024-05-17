using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.ResourceParameters;
using Microsoft.AspNetCore.Mvc;
using CheckDrive.ApiContracts.Driver;
using CheckDrive.ApiContracts.MechanicHandover;

namespace CheckDrive.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MechanicHandoversController : Controller
{
    private readonly IMechanicHandoverService _mechanicHandoverService;

    public MechanicHandoversController(IMechanicHandoverService mechanicHandoverService)
    {
        _mechanicHandoverService = mechanicHandoverService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MechanicHandoverDto>>> GetMechanichandoversAsync(
    [FromQuery] MechanicHandoverResourceParameters mechanicHandoverResource)
    {
        var mechanicHandovers = await _mechanicHandoverService.GetMechanicHandoversAsync(mechanicHandoverResource);

        return Ok(mechanicHandovers);
    }

    [HttpGet("{id}", Name = "GetMechanicHandoverByIdAsync")]
    public async Task<ActionResult<DriverDto>> GetMechanicHandoverByIdAsync(int id)
    {
        var mechanicHandover = await _mechanicHandoverService.GetMechanicHandoverByIdAsync(id);

        if (mechanicHandover is null)
            return NotFound($"MechanicHandover with id: {id} does not exist.");

        return Ok(mechanicHandover);
    }

    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] MechanicHandoverForCreateDto mechanicHandoverforCreateDto)
    {
        var createdMechanicHandover = await _mechanicHandoverService.CreateMechanicHandoverAsync(mechanicHandoverforCreateDto);

        return CreatedAtAction(nameof(GetMechanicHandoverByIdAsync), new { createdMechanicHandover.Id }, createdMechanicHandover);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] MechanicHandoverForUpdateDto mechanicHandoverforUpdateDto)
    {
        if (id != mechanicHandoverforUpdateDto.Id)
        {
            return BadRequest(
                $"Route id: {id} does not match with parameter id: {mechanicHandoverforUpdateDto.Id}.");
        }

        var updateMechanicHandover = await _mechanicHandoverService.UpdateMechanicHandoverAsync(mechanicHandoverforUpdateDto);

        return Ok(updateMechanicHandover);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _mechanicHandoverService.DeleteMechanicHandoverAsync(id);

        return NoContent();
    }
}

