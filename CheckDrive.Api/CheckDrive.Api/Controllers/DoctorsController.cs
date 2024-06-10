using CheckDrive.ApiContracts.Account;
using CheckDrive.ApiContracts.Doctor;
using CheckDrive.ApiContracts.DoctorReview;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.ResourceParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;

//[Authorize(Policy = "AdminOrDoctor")]
[ApiController]
[Route("api/doctors")]
public class DoctorsController : Controller
{
    private readonly IDoctorService _doctorService;
    private readonly IDoctorReviewService _reviewService;
    private readonly IAccountService _accountService;

    public DoctorsController(IDoctorService doctorService, IAccountService accountService, IDoctorReviewService reviewService)
    {
        _doctorService = doctorService;
        _accountService = accountService;
        _reviewService = reviewService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DoctorDto>>> GetDoctorsAsync(
    [FromQuery] DoctorResourceParameters doctorResource)
    {
        var doctors = await _doctorService.GetDoctorsAsync(doctorResource);

        return Ok(doctors);
    }

    [HttpGet("{id}", Name = "GetDoctorById")]
    public async Task<ActionResult<DoctorDto>> GetDoctorByIdAsync(int id)
    {
        var doctor = await _doctorService.GetDoctorByIdAsync(id);

        if (doctor is null)
            return NotFound($"Doctor with id: {id} does not exist.");

        return Ok(doctor);
    }

    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] DoctorForCreateDto doctorforCreateDto)
    {
        var createdDoctor = await _doctorService.CreateDoctorAsync(doctorforCreateDto);

        return CreatedAtAction("GetDoctorById", new { createdDoctor.Id }, createdDoctor);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] AccountForUpdateDto doctorforUpdateDto)
    {
        if (id != doctorforUpdateDto.Id)
        {
            return BadRequest(
                $"Route id: {id} does not match with parameter id: {doctorforUpdateDto.Id}.");
        }

        var updateDoctor = await _accountService.UpdateAccountAsync(doctorforUpdateDto);

        return Ok(updateDoctor);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _doctorService.DeleteDoctorAsync(id);

        return NoContent();
    }

    [HttpGet("reviews")]
    public async Task<ActionResult<IEnumerable<DoctorReviewDto>>> GetDoctorReviewsAsync(
        [FromQuery] DoctorReviewResourceParameters resourceParameters)
    {
        var doctorReviews = await _reviewService.GetDoctorReviewsAsync(resourceParameters);

        return Ok(doctorReviews);
    }

    [HttpGet("review/{id}", Name = "GetDoctorReviewById")]
    public async Task<ActionResult<DoctorReviewDto>> GetDoctorReviewByIdAsync(int id)
    {
        var doctorReview = await _reviewService.GetDoctorReviewByIdAsync(id);

        if (doctorReview is null)
            return NotFound($"DoctorReview with id: {id} does not exist.");

        return Ok(doctorReview);
    }

    [HttpPost("review")]
    public async Task<ActionResult> PostAsync([FromBody] DoctorReviewForCreateDto doctorReview)
    {
        var createdDoctorReview = await _reviewService.CreateDoctorReviewAsync(doctorReview);

        return CreatedAtAction("GetDoctorReviewById", new { createdDoctorReview.Id }, createdDoctorReview);
    }

    [HttpPut("review/{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] DoctorReviewForUpdateDto doctorReview)
    {
        if (id != doctorReview.Id)
        {
            return BadRequest(
                $"Route id: {id} does not match with parameter id: {doctorReview.Id}.");
        }

        var updateDoctorReview = await _reviewService.UpdateDoctorReviewAsync(doctorReview);

        return Ok(updateDoctorReview);
    }

    [HttpDelete("review/{id}")]
    public async Task<ActionResult> DeleteReview(int id)
    {
        await _reviewService.DeleteDoctorReviewAsync(id);

        return NoContent();
    }
}

