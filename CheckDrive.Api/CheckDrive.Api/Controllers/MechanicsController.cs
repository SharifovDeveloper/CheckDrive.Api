using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.ResourceParameters;
using Microsoft.AspNetCore.Mvc;
using CheckDrive.ApiContracts.Account;
using CheckDrive.ApiContracts.Mechanic;


namespace CheckDrive.Api.Controllers;
[ApiController]
[Route("[controller]")]

public class MechanicsController : Controller
{
    private readonly IMechanicService _mechanicService;
    private readonly IAccountService _accountService;

    public MechanicsController(IMechanicService mechanicService, IAccountService accountService)
    {
        _mechanicService = mechanicService;
        _accountService = accountService;
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
    public async Task<ActionResult> PutAsync(int id, [FromBody] AccountForUpdateDto mechanicForUpdate)
    {
        if (id != mechanicForUpdate.Id)
        {
            return BadRequest(
                $"Route id: {id} does not match with parameter id: {mechanicForUpdate.Id}.");
        }

        var updatedMechanic = await _accountService.UpdateAccountAsync(mechanicForUpdate);

        return Ok(updatedMechanic);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _mechanicService.DeleteMechanicAsync(id);

        return NoContent();
    }
}

