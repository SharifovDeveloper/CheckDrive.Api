using AutoMapper;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.Pagniation;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDrive.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using CheckDrive.ApiContracts.Car;

namespace CheckDrive.Services;

public class CarService : ICarService
{
    private readonly IMapper _mapper;
    private readonly CheckDriveDbContext _context;

    public CarService(IMapper mapper, CheckDriveDbContext context)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task<GetBaseResponse<CarDto>> GetCarsAsync(CarResourceParameters resourceParameters)
    {
        var query = GetQueryCarResParameters(resourceParameters);

        var cars = await query.ToPaginatedListAsync(resourceParameters.PageSize, resourceParameters.PageNumber);

        var carDtos = _mapper.Map<List<CarDto>>(cars);

        var paginatedResult = new PaginatedList<CarDto>(carDtos, cars.TotalCount, cars.CurrentPage, cars.PageSize);

        return paginatedResult.ToResponse();
    }

    public async Task<CarDto?> GetCarByIdAsync(int id)
    {
        var car = await _context.Cars.FirstOrDefaultAsync(x => x.Id == id);

        var carDto = _mapper.Map<CarDto>(car);

        return carDto;
    }

    public async Task<CarDto> CreateCarAsync(CarForCreateDto carForCreate)
    {
        var carEntity = _mapper.Map<Car>(carForCreate);

        await _context.Cars.AddAsync(carEntity);
        await _context.SaveChangesAsync();

        var carDto = _mapper.Map<CarDto>(carEntity);

        return carDto;
    }

    public async Task<CarDto> UpdateCarAsync(CarForUpdateDto carForUpdate)
    {
        var carEntity = _mapper.Map<Car>(carForUpdate);

        _context.Cars.Update(carEntity);
        await _context.SaveChangesAsync();

        var carDto = _mapper.Map<CarDto>(carEntity);

        return carDto;
    }

    public async Task DeleteCarAsync(int id)
    {
        var car = await _context.Cars.FirstOrDefaultAsync(x => x.Id == id);

        if (car is not null)
        {
            _context.Cars.Remove(car);
        }

        await _context.SaveChangesAsync();
    }

    private IQueryable<Car> GetQueryCarResParameters(
           CarResourceParameters resourceParameters)
    {
        var query = _context.Cars.AsQueryable();

        //MeduimFuelConsumption
        if (resourceParameters.MeduimFuelConsumption is not null)
        {
            query = query.Where(x => x.MeduimFuelConsumption == resourceParameters.MeduimFuelConsumption);
        }
        if (resourceParameters.ManufacturedYearLessThan is not null)
        {
            query = query.Where(x => x.MeduimFuelConsumption < resourceParameters.MeduimFuelConsumptionLessThan);
        }
        if (resourceParameters.MeduimFuelConsumptionGreaterThan is not null)
        {
            query = query.Where(x => x.MeduimFuelConsumption > resourceParameters.MeduimFuelConsumptionGreaterThan);
        }

        //FuelTankCapacity
        if (resourceParameters.FuelTankCapacity is not null)
        {
            query = query.Where(x => x.FuelTankCapacity == resourceParameters.FuelTankCapacity);
        }
        if (resourceParameters.FuelTankCapacityLessThan is not null)
        {
            query = query.Where(x => x.FuelTankCapacity < resourceParameters.FuelTankCapacityLessThan);
        }
        if (resourceParameters.FuelTankCapacityThan is not null)
        {
            query = query.Where(x => x.FuelTankCapacity > resourceParameters.FuelTankCapacityThan);
        }

        //ManufacturedYear
        if (resourceParameters.ManufacturedYear is not null)
        {
            query = query.Where(x => x.ManufacturedYear == resourceParameters.ManufacturedYear);
        }
        if (resourceParameters.ManufacturedYearLessThan is not null)
        {
            query = query.Where(x => x.ManufacturedYear < resourceParameters.ManufacturedYearLessThan);
        }
        if (resourceParameters.ManufacturedYearGreaterThan is not null)
        {
            query = query.Where(x => x.ManufacturedYear > resourceParameters.ManufacturedYearLessThan);
        }

        return query;
    }
}

