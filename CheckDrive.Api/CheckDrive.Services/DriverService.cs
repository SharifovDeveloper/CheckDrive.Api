using AutoMapper;
using CheckDrive.Domain.DTOs.Driver;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.Pagniation;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDrive.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Services;

public class DriverService : IDriverService
{
    private readonly IMapper _mapper;
    private readonly CheckDriveDbContext _context;

    public DriverService(IMapper mapper, CheckDriveDbContext context)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetBaseResponse<DriverDto>> GetDriversAsync(DriverResourceParameters resourceParameters)
    {
        var query = GetQueryDriverResParameters(resourceParameters);

        var drivers = await query.ToPaginatedListAsync(resourceParameters.PageSize, resourceParameters.PageNumber);

        var driverDtos = _mapper.Map<List<DriverDto>>(drivers);

        var paginatedResult = new PaginatedList<DriverDto>(driverDtos, drivers.TotalCount, drivers.CurrentPage, drivers.PageSize);

        return paginatedResult.ToResponse();
    }

    public async Task<DriverDto?> GetDriverByIdAsync(int id)
    {
        var driver = await _context.Drivers.FirstOrDefaultAsync(x => x.Id == id);

        var driverDto = _mapper.Map<DriverDto>(driver);

        return driverDto;
    }

    public async Task<DriverDto> CreateDriverAsync(DriverForCreateDto driverForCreate)
    {
        var driverEntity = _mapper.Map<Driver>(driverForCreate);

        await _context.Drivers.AddAsync(driverEntity);
        await _context.SaveChangesAsync();

        var driverDto = _mapper.Map<DriverDto>(driverEntity);

        return driverDto;
    }

    public async Task<DriverDto> UpdateDriverAsync(DriverForUpdateDto driverForUpdate)
    {
        var driverEntity = _mapper.Map<Driver>(driverForUpdate);

        _context.Drivers.Update(driverEntity);
        await _context.SaveChangesAsync();

        var driverDto = _mapper.Map<DriverDto>(driverEntity);

        return driverDto;
    }

    public async Task DeleteDriverAsync(int id)
    {
        var driver = await _context.Drivers.FirstOrDefaultAsync(x => x.Id == id);

        if (driver is not null)
        {
            _context.Drivers.Remove(driver);
        }

        await _context.SaveChangesAsync();
    }

    private IQueryable<Driver> GetQueryDriverResParameters(
       DriverResourceParameters resourceParameters)
    {
        var query = _context.Drivers.AsQueryable();

        if (resourceParameters.AccountId != 0 && resourceParameters.AccountId is not null)
        {
            query = query.Where(x => x.AccountId == resourceParameters.AccountId);
        }

        return query;
    }
}

