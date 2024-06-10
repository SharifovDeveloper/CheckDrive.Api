using CheckDrive.ApiContracts.Car;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.ResourceParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;

//[Authorize]
[ApiController]
[Route("api/cars")]
public class CarsController : Controller
{
    private readonly ICarService _carService;

    public CarsController(ICarService carService)
    {
        _carService = carService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CarDto>>> GetCarsAsync(
        [FromQuery] CarResourceParameters carResource)
    {
        var cars = await _carService.GetCarsAsync(carResource);

        return Ok(cars);
    }

    [HttpGet("{id}", Name = "GetCarById")]
    public async Task<ActionResult<CarDto>> GetCarByIdAsync(int id)
    {
        var car = await _carService.GetCarByIdAsync(id);

        if (car is null)
            return NotFound($"Car with id: {id} does not exist.");

        return Ok(car);
    }
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] CarForCreateDto forCreateDto)
    {
        var createdCar = await _carService.CreateCarAsync(forCreateDto);

        return CreatedAtAction("GetCarById", new { id = createdCar.Id }, createdCar);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] CarForUpdateDto forUpdateDto)
    {
        if (id != forUpdateDto.Id)
        {
            return BadRequest(
                $"Route id: {id} does not match with parameter id: {forUpdateDto.Id}.");
        }

        var updateCar = await _carService.UpdateCarAsync(forUpdateDto);

        return Ok(updateCar);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _carService.DeleteCarAsync(id);

        return NoContent();
    }
}

