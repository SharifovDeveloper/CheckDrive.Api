using CheckDrive.Domain.DTOs.Doctor;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.ResourceParameters;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DoctorsController : Controller
{
    private readonly IDoctorService _doctorService;

    public DoctorsController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DoctorDto>>> GetDoctorsAsync(
    [FromQuery] DoctorResourceParameters doctorResource)
    {
        var doctors = await _doctorService.GetDoctorsAsync(doctorResource);

        return Ok(doctors);
    }

    [HttpGet("{id}", Name = "GetDoctorByIdAsync")]
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

        return CreatedAtAction(nameof(GetDoctorByIdAsync), new { createdDoctor.Id }, createdDoctor);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] DoctorForUpdateDto doctorforUpdateDto)
    {
        if (id != doctorforUpdateDto.Id)
        {
            return BadRequest(
                $"Route id: {id} does not match with parameter id: {doctorforUpdateDto.Id}.");
        }

        var updateDoctor = await _doctorService.UpdateDoctorAsync(doctorforUpdateDto);

        return Ok(updateDoctor);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _doctorService.DeleteDoctorAsync(id);

        return NoContent();
    }
}

