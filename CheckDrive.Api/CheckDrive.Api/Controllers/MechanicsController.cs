using CheckDrive.ApiContracts.Account;
using CheckDrive.ApiContracts.Mechanic;
using CheckDrive.ApiContracts.MechanicAcceptance;
using CheckDrive.ApiContracts.MechanicHandover;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.ResourceParameters;
using Microsoft.AspNetCore.Mvc;


namespace CheckDrive.Api.Controllers;
[ApiController]
[Route("api/mechanics")]

public class MechanicsController : Controller
{
    private readonly IMechanicService _mechanicService;
    private readonly IMechanicAcceptanceService _mechanicAcceptanceService;
    private readonly IMechanicHandoverService _mechanicHandoverService;
    private readonly IAccountService _accountService;

    public MechanicsController(IMechanicService mechanicService, IAccountService accountService, IMechanicAcceptanceService mechanicAcceptanceService, IMechanicHandoverService mechanicHandoverService)
    {
        _mechanicService = mechanicService;
        _accountService = accountService;
        _mechanicAcceptanceService = mechanicAcceptanceService;
        _mechanicHandoverService = mechanicHandoverService;
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

    [HttpGet("acceptances")]
    public async Task<ActionResult<IEnumerable<MechanicAcceptanceDto>>> GetMechanicAcceptancesAsync(
    [FromQuery] MechanicAcceptanceResourceParameters mechanicAcceptanceResource)
    {
        var mechanicAcceptances = await _mechanicAcceptanceService.GetMechanicAcceptencesAsync(mechanicAcceptanceResource);

        return Ok(mechanicAcceptances);
    }

    [HttpGet("acceptance/{id}")]
    public async Task<ActionResult<MechanicAcceptanceDto>> GetMechanicAcceptanceByIdAsync(int id)
    {
        var mechanicAcceptance = await _mechanicAcceptanceService.GetMechanicAcceptenceByIdAsync(id);

        if (mechanicAcceptance is null)
            return NotFound($"MechanicAcceptance with id: {id} does not exist.");

        return Ok(mechanicAcceptance);
    }

    [HttpPost("acceptance")]
    public async Task<ActionResult> PostAsync([FromBody] MechanicAcceptanceForCreateDto mechanicAcceptanceforCreateDto)
    {
        var createdMechanicAcceptance = await _mechanicAcceptanceService.CreateMechanicAcceptenceAsync(mechanicAcceptanceforCreateDto);

        return CreatedAtAction(nameof(GetMechanicAcceptanceByIdAsync), new { createdMechanicAcceptance.Id }, createdMechanicAcceptance);
    }

    [HttpPut("acceptance/{id}")]
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

    [HttpDelete("acceptance/{id}")]
    public async Task<ActionResult> DeleteAcceptance(int id)
    {
        await _mechanicAcceptanceService.DeleteMechanicAcceptenceAsync(id);

        return NoContent();
    }

    [HttpGet("handovers")]
    public async Task<ActionResult<IEnumerable<MechanicHandoverDto>>> GetMechanichandoversAsync(
    [FromQuery] MechanicHandoverResourceParameters mechanicHandoverResource)
    {
        var mechanicHandovers = await _mechanicHandoverService.GetMechanicHandoversAsync(mechanicHandoverResource);

        return Ok(mechanicHandovers);
    }

    [HttpGet("handover/{id}")]
    public async Task<ActionResult<MechanicHandoverDto>> GetMechanicHandoverByIdAsync(int id)
    {
        var mechanicHandover = await _mechanicHandoverService.GetMechanicHandoverByIdAsync(id);

        if (mechanicHandover is null)
            return NotFound($"MechanicHandover with id: {id} does not exist.");

        return Ok(mechanicHandover);
    }

    [HttpPost("handover")]
    public async Task<ActionResult> PostAsync([FromBody] MechanicHandoverForCreateDto mechanicHandoverforCreateDto)
    {
        var createdMechanicHandover = await _mechanicHandoverService.CreateMechanicHandoverAsync(mechanicHandoverforCreateDto);

        return CreatedAtAction(nameof(GetMechanicHandoverByIdAsync), new { createdMechanicHandover.Id }, createdMechanicHandover);
    }

    [HttpPut("handover/{id}")]
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

    [HttpDelete("handover/{id}")]
    public async Task<ActionResult> DeleteHandover(int id)
    {
        await _mechanicHandoverService.DeleteMechanicHandoverAsync(id);

        return NoContent();
    }
}

