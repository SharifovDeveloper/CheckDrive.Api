using CheckDrive.ApiContracts.Account;
using CheckDrive.ApiContracts.DoctorReview;
using CheckDrive.ApiContracts.Mechanic;
using CheckDrive.ApiContracts.MechanicAcceptance;
using CheckDrive.ApiContracts.MechanicHandover;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDrive.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;

[Authorize]
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


    [HttpGet("mechanicHistories")]
    public async Task<ActionResult<IEnumerable<MechanicHistororiesDto>>> GetOperatorHistory(int accountId)
    {
        var historymechanics = await _mechanicService.GetMechanicHistories(accountId);

        return Ok(historymechanics);
    }

    [Authorize(Policy = "AdminOrMechanic")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MechanicDto>>> GetMechanicsAsync(
    [FromQuery] MechanicResourceParameters mechanicResource)
    {
        var mechanics = await _mechanicService.GetMechanicesAsync(mechanicResource);

        return Ok(mechanics);
    }

    [Authorize(Policy = "AdminOrMechanic")]
    [HttpGet("{id}", Name = "GetMechanicById")]
    public async Task<ActionResult<MechanicDto>> GetMechanicByIdAsync(int id)
    {
        var mechanic = await _mechanicService.GetMechanicByIdAsync(id);

        if (mechanic is null)
            return NotFound($"Mechanic with id: {id} does not exist.");

        return Ok(mechanic);
    }

    [Authorize(Policy = "AdminOrMechanic")]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] MechanicForCreateDto mechanicForCreate)
    {
        var createdMechanic = await _mechanicService.CreateMechanicAsync(mechanicForCreate);

        return CreatedAtAction("GetMechanicById", new { createdMechanic.Id }, createdMechanic);
    }

    [Authorize(Policy = "AdminOrMechanic")]
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

    [Authorize(Policy = "AdminOrMechanic")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _mechanicService.DeleteMechanicAsync(id);

        return NoContent();
    }

    [Authorize]
    [HttpGet("acceptances")]
    public async Task<ActionResult<IEnumerable<MechanicAcceptanceDto>>> GetMechanicAcceptancesAsync(
    [FromQuery] MechanicAcceptanceResourceParameters mechanicAcceptanceResource)
    {
        var mechanicAcceptances = new GetBaseResponse<MechanicAcceptanceDto>();

        if (mechanicAcceptanceResource.RoleId == 6)
        {
            mechanicAcceptances = await _mechanicAcceptanceService.GetMechanicAcceptencesForMechanicAsync(mechanicAcceptanceResource);
        }
        else
        {
            mechanicAcceptances = await _mechanicAcceptanceService.GetMechanicAcceptencesAsync(mechanicAcceptanceResource);
        }

        return Ok(mechanicAcceptances);
    }

    [Authorize(Policy = "AdminOrMechanic")]
    [HttpGet("acceptance/{id}", Name = "GetMechanicAcceptanceById")]
    public async Task<ActionResult<MechanicAcceptanceDto>> GetMechanicAcceptanceByIdAsync(int id)
    {
        var mechanicAcceptance = await _mechanicAcceptanceService.GetMechanicAcceptenceByIdAsync(id);

        if (mechanicAcceptance is null)
            return NotFound($"MechanicAcceptance with id: {id} does not exist.");

        return Ok(mechanicAcceptance);
    }

    [Authorize(Policy = "AdminOrMechanic")]
    [HttpPost("acceptance")]
    public async Task<ActionResult> PostAsync([FromBody] MechanicAcceptanceForCreateDto mechanicAcceptanceforCreateDto)
    {
        var createdMechanicAcceptance = await _mechanicAcceptanceService.CreateMechanicAcceptenceAsync(mechanicAcceptanceforCreateDto);

        return CreatedAtAction("GetMechanicAcceptanceById", new { createdMechanicAcceptance.Id }, createdMechanicAcceptance);
    }

    [Authorize(Policy = "AdminOrMechanic")]
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

    [Authorize(Policy = "AdminOrMechanic")]
    [HttpDelete("acceptance/{id}")]
    public async Task<ActionResult> DeleteAcceptance(int id)
    {
        await _mechanicAcceptanceService.DeleteMechanicAcceptenceAsync(id);

        return NoContent();
    }

    [Authorize]
    [HttpGet("handovers")]
    public async Task<ActionResult<IEnumerable<MechanicHandoverDto>>> GetMechanichandoversAsync(
    [FromQuery] MechanicHandoverResourceParameters mechanicHandoverResource)
    {
        var mechanicHandovers = new GetBaseResponse<MechanicHandoverDto>();

        if (mechanicHandoverResource.RoleId == 6)
        {
            mechanicHandovers = await _mechanicHandoverService.GetMechanicHandoversForMechanicsAsync(mechanicHandoverResource);
        }
        else
        {
            mechanicHandovers = await _mechanicHandoverService.GetMechanicHandoversAsync(mechanicHandoverResource);
        }

        return Ok(mechanicHandovers);
    }

    [Authorize(Policy = "AdminOrMechanic")]
    [HttpGet("handover/{id}", Name = "GetMechanicHandoverById")]
    public async Task<ActionResult<MechanicHandoverDto>> GetMechanicHandoverByIdAsync(int id)
    {
        var mechanicHandover = await _mechanicHandoverService.GetMechanicHandoverByIdAsync(id);

        if (mechanicHandover is null)
            return NotFound($"MechanicHandover with id: {id} does not exist.");

        return Ok(mechanicHandover);
    }

    [Authorize(Policy = "AdminOrMechanic")]
    [HttpPost("handover")]
    public async Task<ActionResult> PostAsync([FromBody] MechanicHandoverForCreateDto mechanicHandoverforCreateDto)
    {
        var createdMechanicHandover = await _mechanicHandoverService.CreateMechanicHandoverAsync(mechanicHandoverforCreateDto);

        return CreatedAtAction("GetMechanicHandoverById", new { createdMechanicHandover.Id }, createdMechanicHandover);
    }

    [Authorize(Policy = "AdminOrMechanic")]
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

    [Authorize(Policy = "AdminOrMechanic")]
    [HttpDelete("handover/{id}")]
    public async Task<ActionResult> DeleteHandover(int id)
    {
        await _mechanicHandoverService.DeleteMechanicHandoverAsync(id);

        return NoContent();
    }
}

