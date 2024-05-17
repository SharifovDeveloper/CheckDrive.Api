using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.ResourceParameters;
using Microsoft.AspNetCore.Mvc;
using CheckDrive.ApiContracts.DoctorReview;

namespace CheckDrive.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DoctorReviewsController : Controller
{
    private readonly IDoctorReviewService _doctorReviewService;
    public DoctorReviewsController(IDoctorReviewService doctorReviewService)
    {
        _doctorReviewService = doctorReviewService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DoctorReviewDto>>> GetDoctorReviewsAsync(
        [FromQuery] DoctorReviewResourceParameters resourceParameters)
    {
        var doctorReviews = await _doctorReviewService.GetDoctorReviewsAsync(resourceParameters);

        return Ok(doctorReviews);
    }
    [HttpGet("{id}", Name = "GetDoctorReviewByIdAsync")]
    public async Task<ActionResult<DoctorReviewDto>> GetDoctorReviewByIdAsync(int id)
    {
        var doctorReview = await _doctorReviewService.GetDoctorReviewByIdAsync(id);

        if (doctorReview is null)
            return NotFound($"DoctorReview with id: {id} does not exist.");

        return Ok(doctorReview);
    }
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] DoctorReviewForCreateDto doctorReview)
    {
        var createdDoctorReview = await _doctorReviewService.CreateDoctorReviewAsync(doctorReview);

        return CreatedAtAction(nameof(GetDoctorReviewByIdAsync), new { createdDoctorReview.Id }, createdDoctorReview);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] DoctorReviewForUpdateDto doctorReview)
    {
        if (id != doctorReview.Id)
        {
            return BadRequest(
                $"Route id: {id} does not match with parameter id: {doctorReview.Id}.");
        }

        var updateDoctorReview = await _doctorReviewService.UpdateDoctorReviewAsync(doctorReview);

        return Ok(updateDoctorReview);
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _doctorReviewService.DeleteDoctorReviewAsync(id);

        return NoContent();
    }
}

