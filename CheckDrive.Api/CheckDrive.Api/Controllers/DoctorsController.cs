using CheckDrive.ApiContracts.Account;
using CheckDrive.ApiContracts.Doctor;
using CheckDrive.ApiContracts.DoctorReview;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;

//[Authorize]
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

    [Authorize(Policy = "AdminOrDoctor")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DoctorDto>>> GetDoctorsAsync(
    [FromQuery] DoctorResourceParameters doctorResource)
    {
        var doctors = await _doctorService.GetDoctorsAsync(doctorResource);

        return Ok(doctors);
    }

    [Authorize(Policy = "AdminOrDoctor")]
    [HttpGet("{id}", Name = "GetDoctorById")]
    public async Task<ActionResult<DoctorDto>> GetDoctorByIdAsync(int id)
    {
        var doctor = await _doctorService.GetDoctorByIdAsync(id);

        if (doctor is null)
            return NotFound($"Doctor with id: {id} does not exist.");

        return Ok(doctor);
    }

    [Authorize(Policy = "AdminOrDoctor")]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] DoctorForCreateDto doctorforCreateDto)
    {
        var createdDoctor = await _doctorService.CreateDoctorAsync(doctorforCreateDto);

        return CreatedAtAction("GetDoctorById", new { createdDoctor.Id }, createdDoctor);
    }

    [Authorize(Policy = "AdminOrDoctor")]
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

    [Authorize(Policy = "AdminOrDoctor")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _doctorService.DeleteDoctorAsync(id);

        return NoContent();
    }

    [Authorize]
    [HttpGet("reviews")]
    public async Task<ActionResult<IEnumerable<DoctorReviewDto>>> GetDoctorReviewsAsync(
        [FromQuery] DoctorReviewResourceParameters resourceParameters)
    {
        var doctorReviews = new GetBaseResponse<DoctorReviewDto>();

        if (resourceParameters.RoleId == 3)
        {
            doctorReviews = await _reviewService.GetDoctorReviewsForDoctorAsync(resourceParameters);
        }
        else
        {
            doctorReviews = await _reviewService.GetDoctorReviewsAsync(resourceParameters);
        }

        return Ok(doctorReviews);
    }

    [Authorize(Policy = "AdminOrDoctor")]
    [HttpGet("review/{id}", Name = "GetDoctorReviewById")]
    public async Task<ActionResult<DoctorReviewDto>> GetDoctorReviewByIdAsync(int id)
    {
        var doctorReview = await _reviewService.GetDoctorReviewByIdAsync(id);

        if (doctorReview is null)
            return NotFound($"DoctorReview with id: {id} does not exist.");

        return Ok(doctorReview);
    }

    [Authorize(Policy = "AdminOrDoctor")]
    [HttpPost("review")]
    public async Task<ActionResult> PostAsync([FromBody] DoctorReviewForCreateDto doctorReview)
    {
        var createdDoctorReview = await _reviewService.CreateDoctorReviewAsync(doctorReview);

        return CreatedAtAction("GetDoctorReviewById", new { createdDoctorReview.Id }, createdDoctorReview);
    }

    [Authorize(Policy = "AdminOrDoctor")]
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

    [Authorize(Policy = "AdminOrDoctor")]
    [HttpDelete("review/{id}")]
    public async Task<ActionResult> DeleteReview(int id)
    {
        await _reviewService.DeleteDoctorReviewAsync(id);

        return NoContent();
    }
}

