using CheckDrive.Domain.DTOs.Operator;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.ResourceParameters;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;
[ApiController]
[Route("[controller]")]

public class OperatorsController : Controller
{
    private readonly IOperatorService _operatorService;

    public OperatorsController(IOperatorService operatorService)
    {
        _operatorService = operatorService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OperatorDto>>> GetOperatorsAsync(
    [FromQuery] OperatorResourceParameters operatorResource)
    {
        var mechanics = await _operatorService.GetOperatorsAsync(operatorResource);

        return Ok(mechanics);
    }

    [HttpGet("{id}", Name = "GetOperatorByIdAsync")]
    public async Task<ActionResult<OperatorDto>> GetOperatorByIdAsync(int id)
    {
        var _operator = await _operatorService.GetOperatorByIdAsync(id);

        if (_operator is null)
            return NotFound($"Operator with id: {id} does not exist.");

        return Ok(_operator);
    }

    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] OperatorForCreateDto operatorForCreate)
    {
        var createdOperator = await _operatorService.CreateOperatorAsync(operatorForCreate);

        return CreatedAtAction(nameof(GetOperatorByIdAsync), new { createdOperator.Id }, createdOperator);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] OperatorForUpdateDto operatorForUpdate)
    {
        if (id != operatorForUpdate.Id)
        {
            return BadRequest(
                $"Route id: {id} does not match with parameter id: {operatorForUpdate.Id}.");
        }

        var updatedOperator = await _operatorService.UpdateOperatorAsync(operatorForUpdate);

        return Ok(updatedOperator);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _operatorService.DeleteOperatorAsync(id);

        return NoContent();
    }
}

