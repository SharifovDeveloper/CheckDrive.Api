using CheckDrive.ApiContracts.Account;
using CheckDrive.ApiContracts.DoctorReview;
using CheckDrive.ApiContracts.Operator;
using CheckDrive.ApiContracts.OperatorReview;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;

//[Authorize]
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

    [Authorize(Policy = "AdminOrOperator")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OperatorDto>>> GetOperatorsAsync(
    [FromQuery] OperatorResourceParameters operatorResource)
    {
        var mechanics = await _operatorService.GetOperatorsAsync(operatorResource);

        return Ok(mechanics);
    }

    [Authorize(Policy = "AdminOrOperator")]
    [HttpGet("{id}", Name = "GetOperatorById")]
    public async Task<ActionResult<OperatorDto>> GetOperatorByIdAsync(int id)
    {
        var _operator = await _operatorService.GetOperatorByIdAsync(id);

        if (_operator is null)
            return NotFound($"Operator with id: {id} does not exist.");

        return Ok(_operator);
    }

    [Authorize(Policy = "AdminOrOperator")]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] OperatorForCreateDto operatorForCreate)
    {
        var createdOperator = await _operatorService.CreateOperatorAsync(operatorForCreate);

        return CreatedAtAction("GetOperatorById", new { createdOperator.Id }, createdOperator);
    }

    [Authorize(Policy = "AdminOrOperator")]
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

    [Authorize(Policy = "AdminOrOperator")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _operatorService.DeleteOperatorAsync(id);

        return NoContent();
    }

    [Authorize]
    [HttpGet("reviews")]
    public async Task<ActionResult<IEnumerable<OperatorReviewDto>>> GetOperatorReviewsAsync(
    [FromQuery] OperatorReviewResourceParameters resourceParameters)
    {
        var operatorReviews = new GetBaseResponse<OperatorReviewDto>();

        if (resourceParameters.RoleId == 4)
        {
            operatorReviews = await _operatorReviewService.GetOperatorReviewsForOperatorAsync(resourceParameters);
        }
        else
        {
            operatorReviews = await _operatorReviewService.GetOperatorReviewsAsync(resourceParameters);
        }

        return Ok(operatorReviews);
    }

    [Authorize(Policy = "AdminOrOperator")]
    [HttpGet("review/{id}", Name = "GetOperatorReviewById")]
    public async Task<ActionResult<OperatorReviewDto>> GetOperatorReviewByIdAsync(int id)
    {
        var operatorReview = await _operatorReviewService.GetOperatorReviewByIdAsync(id);

        if (operatorReview is null)
            return NotFound($"DoctorReview with id: {id} does not exist.");

        return Ok(operatorReview);
    }

    [Authorize(Policy = "AdminOrOperator")]
    [HttpPost("review")]
    public async Task<ActionResult> PostAsync([FromBody] OperatorReviewForCreateDto operatorReview)
    {
        var createdOperatorReview = await _operatorReviewService.CreateOperatorReviewAsync(operatorReview);

        return CreatedAtAction("GetOperatorReviewById", new { createdOperatorReview.Id }, createdOperatorReview);
    }

    [Authorize(Policy = "AdminOrOperator")]
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

    [Authorize(Policy = "AdminOrOperator")]
    [HttpDelete("review/{id}")]
    public async Task<ActionResult> DeleteReview(int id)
    {
        await _operatorReviewService.DeleteOperatorReviewAsync(id);

        return NoContent();
    }
}

