using AutoMapper;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.Pagniation;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDrive.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using CheckDrive.ApiContracts.MechanicAcceptance;

namespace CheckDrive.Services;

public class MechanicAcceptanceService : IMechanicAcceptanceService
{
    private readonly IMapper _mapper;
    private readonly CheckDriveDbContext _context;

    public MechanicAcceptanceService(IMapper mapper, CheckDriveDbContext context)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetBaseResponse<MechanicAcceptanceDto>> GetMechanicAcceptencesAsync(MechanicAcceptanceResourceParameters resourceParameters)
    {
        var query = GetQueryMechanicAcceptanceResParameters(resourceParameters);

        var mechanicAcceptances = await query.ToPaginatedListAsync(resourceParameters.PageSize, resourceParameters.PageNumber);

        var mechanicAcceptanceDtos = _mapper.Map<List<MechanicAcceptanceDto>>(mechanicAcceptances);

        var paginatedResult = new PaginatedList<MechanicAcceptanceDto>(mechanicAcceptanceDtos, mechanicAcceptances.TotalCount, mechanicAcceptances.CurrentPage, mechanicAcceptances.PageSize);

        return paginatedResult.ToResponse();
    }

    public async Task<MechanicAcceptanceDto?> GetMechanicAcceptenceByIdAsync(int id)
    {
        var mechanicAcceptance = await _context.MechanicsAcceptances
            .Include(d => d.Car)
            .Include(a => a.Driver)
            .ThenInclude(a => a.Account)
            .Include(m => m.Mechanic)
            .ThenInclude(m => m.Account)
            .FirstOrDefaultAsync(x => x.Id == id);


        var mechanicAcceptanceDto = _mapper.Map<MechanicAcceptanceDto>(mechanicAcceptance);

        return mechanicAcceptanceDto;
    }
    public async Task<MechanicAcceptanceDto> CreateMechanicAcceptenceAsync(MechanicAcceptanceForCreateDto acceptanceForCreateDto)
    {
        var mechanicAcceptanceEntity = _mapper.Map<MechanicAcceptance>(acceptanceForCreateDto);

        await _context.MechanicsAcceptances.AddAsync(mechanicAcceptanceEntity);
        await _context.SaveChangesAsync();

        var mechanicAcceptanceDto = _mapper.Map<MechanicAcceptanceDto>(mechanicAcceptanceEntity);

        return mechanicAcceptanceDto;
    }

    public async Task<MechanicAcceptanceDto> UpdateMechanicAcceptenceAsync(MechanicAcceptanceForUpdateDto acceptanceForUpdateDto)
    {
        var mechanicAcceptanceEntity = _mapper.Map<MechanicAcceptance>(acceptanceForUpdateDto);

        _context.MechanicsAcceptances.Update(mechanicAcceptanceEntity);
        await _context.SaveChangesAsync();

        var mechanicAcceptanceDto = _mapper.Map<MechanicAcceptanceDto>(mechanicAcceptanceEntity);

        return mechanicAcceptanceDto;
    }

    public async Task DeleteMechanicAcceptenceAsync(int id)
    {
        var mechanicAcceptance = await _context.MechanicsAcceptances.FirstOrDefaultAsync(x => x.Id == id);

        if (mechanicAcceptance is not null)
        {
            _context.MechanicsAcceptances.Remove(mechanicAcceptance);
        }

        await _context.SaveChangesAsync();
    }

    private IQueryable<MechanicAcceptance> GetQueryMechanicAcceptanceResParameters(
       MechanicAcceptanceResourceParameters resourceParameters)
    {
        var query = _context.MechanicsAcceptances
           .Include(d => d.Car)
            .Include(a => a.Driver)
            .ThenInclude(a => a.Account)
            .Include(m => m.Mechanic)
            .ThenInclude(m => m.Account)
            .AsQueryable();

        if (resourceParameters.Date is not null)
        {
            query = query.Where(x => x.Date.Date == resourceParameters.Date.Value.Date);
        }

        if (resourceParameters.Status is not null)
        {
            query = query.Where(x => x.Status == resourceParameters.Status);
        }

        if (resourceParameters.IsAccepted is not null)
        {
            query = query.Where(x => x.IsAccepted == resourceParameters.IsAccepted);
        }

        //Distance
        if (resourceParameters.Distance is not null)
        {
            query = query.Where(x => x.Distance == resourceParameters.Distance);
        }
        if (resourceParameters.DistanceLessThan is not null)
        {
            query = query.Where(x => x.Distance < resourceParameters.DistanceLessThan);
        }
        if (resourceParameters.DistanceGreaterThan is not null)
        {
            query = query.Where(x => x.Distance > resourceParameters.DistanceGreaterThan);
        }

        if (resourceParameters.DriverId is not null)
        {
            query = query.Where(x => x.DriverId == resourceParameters.DriverId);
        }
        if (!string.IsNullOrEmpty(resourceParameters.OrderBy))
        {
            query = resourceParameters.OrderBy.ToLowerInvariant() switch
            {
                "date" => query.OrderBy(x => x.Date),
                "datedesc" => query.OrderByDescending(x => x.Date),
                _ => query.OrderBy(x => x.Id),
            };
        }

        return query;
    }
}

