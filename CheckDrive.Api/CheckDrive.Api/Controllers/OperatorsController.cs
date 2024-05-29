using CheckDrive.ApiContracts.Account;
using CheckDrive.ApiContracts.Operator;
using CheckDrive.ApiContracts.OperatorReview;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.ResourceParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;

[Authorize(Policy = "AdminOrOperator")]
[ApiController]
[Route("api/operators")]
public class OperatorsController : Controller
{
    private readonly IOperatorService _operatorService;
    private readonly IOperatorReviewService _operatorReviewService;
    private readonly IAccountService _accountService;

    public OperatorsController(IOperatorService operatorService, IAccountService accountService, IOperatorReviewService operatorReviewService)
    {
        _operatorService = operatorService;
        _accountService = accountService;
        _operatorReviewService = operatorReviewService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OperatorDto>>> GetOperatorsAsync(
    [FromQuery] OperatorResourceParameters operatorResource)
    {
        var mechanics = await _operatorService.GetOperatorsAsync(operatorResource);

        return Ok(mechanics);
    }

    [HttpGet("{id}", Name = "GetOperatorById")]
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

        return CreatedAtAction("GetOperatorById", new { createdOperator.Id }, createdOperator);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] AccountForUpdateDto operatorForUpdate)
    {
        if (id != operatorForUpdate.Id)
        {
            return BadRequest(
                $"Route id: {id} does not match with parameter id: {operatorForUpdate.Id}.");
        }

        var updatedOperator = await _accountService.UpdateAccountAsync(operatorForUpdate);

        return Ok(updatedOperator);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _operatorService.DeleteOperatorAsync(id);

        return NoContent();
    }

    [HttpGet("reviews")]
    public async Task<ActionResult<IEnumerable<OperatorReviewDto>>> GetOperatorReviewsAsync(
    [FromQuery] OperatorReviewResourceParameters resourceParameters)
    {
        var operatorReviews = await _operatorReviewService.GetOperatorReviewsAsync(resourceParameters);

        return Ok(operatorReviews);
    }

    [HttpGet("review/{id}", Name = "GetOperatorReviewById")]
    public async Task<ActionResult<OperatorReviewDto>> GetOperatorReviewByIdAsync(int id)
    {
        var operatorReview = await _operatorReviewService.GetOperatorReviewByIdAsync(id);

        if (operatorReview is null)
            return NotFound($"DoctorReview with id: {id} does not exist.");

        return Ok(operatorReview);
    }

    [HttpPost("review")]
    public async Task<ActionResult> PostAsync([FromBody] OperatorReviewForCreateDto operatorReview)
    {
        var createdOperatorReview = await _operatorReviewService.CreateOperatorReviewAsync(operatorReview);

        return CreatedAtAction("GetOperatorReviewById", new { createdOperatorReview.Id }, createdOperatorReview);
    }

    [HttpPut("review/{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] OperatorReviewForUpdateDto operatorReview)
    {
        if (id != operatorReview.Id)
        {
            return BadRequest(
                $"Route id: {id} does not match with parameter id: {operatorReview.Id}.");
        }

        var updateOreratorReview = await _operatorReviewService.UpdateOperatorReviewAsync(operatorReview);

        return Ok(updateOreratorReview);
    }

    [HttpDelete("review/{id}")]
    public async Task<ActionResult> DeleteReview(int id)
    {
        await _operatorReviewService.DeleteOperatorReviewAsync(id);

        return NoContent();
    }
}

