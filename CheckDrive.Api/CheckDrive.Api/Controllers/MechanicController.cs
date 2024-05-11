using CheckDrive.Domain.DTOs.Mechanic;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.ResourceParameters;
using Microsoft.AspNetCore.Mvc;


namespace CheckDrive.Api.Controllers;
[ApiController]
[Route("[controller]")]

public class MechanicController : Controller
{
    private readonly IMechanicService _mechanicService;

    public MechanicController(IMechanicService mechanicService)
    {
        _mechanicService = mechanicService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MechanicDto>>> GetMechanicsAsync(
    [FromQuery] MechanicResourceParameters mechanicResource)
    {
        var mechanics = await _mechanicService.GetMechanicesAsync(mechanicResource);

        return Ok(mechanics);
    }

    [HttpGet("{id}", Name = "GetMechanicByIdAsync")]
    public async Task<ActionResult<MechanicDto>> GetMechanicByIdAsync(int id)
    {
        var mechanic = await _mechanicService.GetMechanicByIdAsync(id);

        if (mechanic is null)
            return NotFound($"Mechanic with id: {id} does not exist.");

        return Ok(mechanic);
    }

    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] MechanicForCreateDto mechanicForCreate)
    {
        var createdMechanic = await _mechanicService.CreateMechanicAsync(mechanicForCreate);

        return CreatedAtAction(nameof(GetMechanicByIdAsync), new { createdMechanic.Id }, createdMechanic);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] MechanicForUpdateDto mechanicForUpdate)
    {
        if (id != mechanicForUpdate.Id)
        {
            return BadRequest(
                $"Route id: {id} does not match with parameter id: {mechanicForUpdate.Id}.");
        }

        var updatedMechanic = await _mechanicService.UpdateMechanicAsync(mechanicForUpdate);

        return Ok(updatedMechanic);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _mechanicService.DeleteMechanicAsync(id);

        return NoContent();
    }
}

