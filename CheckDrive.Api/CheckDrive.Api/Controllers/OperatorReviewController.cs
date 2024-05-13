using CheckDrive.Domain.DTOs.OperatorReview;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.ResourceParameters;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;
[ApiController]
[Route("[controller]")]

public class OperatorReviewController : Controller
{
    private readonly IOperatorReviewService _operatorReviewService;
    public OperatorReviewController(IOperatorReviewService operatorReviewService)
    {
        _operatorReviewService = operatorReviewService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OperatorReviewDto>>> GetOperatorReviewsAsync(
        [FromQuery] OperatorReviewResourceParameters resourceParameters)
    {
        var operatorReviews = await _operatorReviewService.GetOperatorReviewsAsync(resourceParameters);

        return Ok(operatorReviews);
    }

    [HttpGet("{id}", Name = "GetOperatorReviewByIdAsync")]
    public async Task<ActionResult<OperatorReviewDto>> GetOperatorReviewByIdAsync(int id)
    {
        var operatorReview = await _operatorReviewService.GetOperatorReviewByIdAsync(id);

        if (operatorReview is null)
            return NotFound($"DoctorReview with id: {id} does not exist.");

        return Ok(operatorReview);
    }

    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] OperatorReviewForCreateDto operatorReview)
    {
        var createdOperatorReview = await _operatorReviewService.CreateOperatorReviewAsync(operatorReview);

        return CreatedAtAction(nameof(GetOperatorReviewByIdAsync), new { createdOperatorReview.Id }, createdOperatorReview);
    }

    [HttpPut("{id}")]
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
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _operatorReviewService.DeleteOperatorReviewAsync(id);

        return NoContent();
    }
}

