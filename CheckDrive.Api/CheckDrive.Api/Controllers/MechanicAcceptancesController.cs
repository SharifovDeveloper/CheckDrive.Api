using CheckDrive.Domain.DTOs.Driver;
using CheckDrive.Domain.DTOs.MechanicAcceptance;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.ResourceParameters;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MechanicAcceptancesController : Controller
{
    private readonly IMechanicAcceptanceService _mechanicAcceptanceService;

    public MechanicAcceptancesController(IMechanicAcceptanceService mechanicAcceptanceService)
    {
        _mechanicAcceptanceService = mechanicAcceptanceService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MechanicAcceptanceDto>>> GetMechanicAcceptancesAsync(
    [FromQuery] MechanicAcceptanceResourceParameters mechanicAcceptanceResource)
    {
        var mechanicAcceptances = await _mechanicAcceptanceService.GetMechanicAcceptencesAsync(mechanicAcceptanceResource);

        return Ok(mechanicAcceptances);
    }

    [HttpGet("{id}", Name = "GetMechanicAcceptanceByIdAsync")]
    public async Task<ActionResult<DriverDto>> GetMechanicAcceptanceByIdAsync(int id)
    {
        var mechanicAcceptance = await _mechanicAcceptanceService.GetMechanicAcceptenceByIdAsync(id);

        if (mechanicAcceptance is null)
            return NotFound($"MechanicAcceptance with id: {id} does not exist.");

        return Ok(mechanicAcceptance);
    }

    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] MechanicAcceptanceForCreateDto mechanicAcceptanceforCreateDto)
    {
        var createdMechanicAcceptance = await _mechanicAcceptanceService.CreateMechanicAcceptenceAsync(mechanicAcceptanceforCreateDto);

        return CreatedAtAction(nameof(GetMechanicAcceptanceByIdAsync), new { createdMechanicAcceptance.Id }, createdMechanicAcceptance);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] MechanicAcceptanceForUpdateDto mechanicAcceptanceforUpdateDto)
    {
        if (id != mechanicAcceptanceforUpdateDto.Id)
        {
            return BadRequest(
                $"Route id: {id} does not match with parameter id: {mechanicAcceptanceforUpdateDto.Id}.");
        }

        var updateMechanicAcceptance = await _mechanicAcceptanceService.UpdateMechanicAcceptenceAsync(mechanicAcceptanceforUpdateDto);

        return Ok(updateMechanicAcceptance);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _mechanicAcceptanceService.DeleteMechanicAcceptenceAsync(id);

        return NoContent();
    }
}

