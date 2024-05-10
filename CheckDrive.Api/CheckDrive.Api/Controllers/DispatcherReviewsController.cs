using CheckDrive.Domain.DTOs.DispatcherReview;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.ResourceParameters;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DispatcherReviewsController : Controller
{
    private readonly IDispatcherReviewService _dispatcherReviewService;

    public DispatcherReviewsController(IDispatcherReviewService dispatcherReviewService)
    {
        _dispatcherReviewService = dispatcherReviewService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DispatcherReviewDto>>> GetDispatcherReviewsAsync(
        [FromQuery] DispatcherReviewResourceParameters dispatcherReviewResource)
    {
        var dispatcherReviews = await _dispatcherReviewService.GetDispatcherReviewsAsync(dispatcherReviewResource);

        return Ok(dispatcherReviews);
    }

    [HttpGet("{id}", Name = "GetDispatcherReviewsByIdAsync")]
    public async Task<ActionResult<DispatcherReviewDto>> GetDispatcherReviewByIdAsync(int id)
    {
        var dispatcherReview = await _dispatcherReviewService.GetDispatcherReviewByIdAsync(id);

        if (dispatcherReview is null)
            return NotFound($"DispatcherReview with id: {id} does not exist.");

        return Ok(dispatcherReview);
    }
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] DispatcherReviewForCreateDto dispatcherReviewforCreateDto)
    {
        var createdDispatcherReview = await _dispatcherReviewService.CreateDispatcherReviewAsync(dispatcherReviewforCreateDto);

        return CreatedAtAction(nameof(GetDispatcherReviewByIdAsync), new { createdDispatcherReview.Id }, createdDispatcherReview);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] DispatcherReviewForUpdateDto dispatcherReviewforUpdateDto)
    {
        if (id != dispatcherReviewforUpdateDto.Id)
        {
            return BadRequest(
                $"Route id: {id} does not match with parameter id: {dispatcherReviewforUpdateDto.Id}.");
        }

        var updateDispatcherReview = await _dispatcherReviewService.UpdateDispatcherReviewAsync(dispatcherReviewforUpdateDto);

        return Ok(updateDispatcherReview);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _dispatcherReviewService.DeleteDispatcherReviewAsync(id);

        return NoContent();
    }
}

