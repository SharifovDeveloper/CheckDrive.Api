using AutoMapper;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.Pagniation;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDrive.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using CheckDrive.ApiContracts.MechanicHandover;

namespace CheckDrive.Services;

public class MechanicHandoverService : IMechanicHandoverService
{
    private readonly IMapper _mapper;
    private readonly CheckDriveDbContext _context;

    public MechanicHandoverService(IMapper mapper, CheckDriveDbContext context)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetBaseResponse<MechanicHandoverDto>> GetMechanicHandoversAsync(MechanicHandoverResourceParameters resourceParameters)
    {
        var query = GetQueryMechanicHandoverResParameters(resourceParameters);

        var mechanicHandovers = await query.ToPaginatedListAsync(resourceParameters.PageSize, resourceParameters.PageNumber);

        var mechanicHandoverDtos = _mapper.Map<List<MechanicHandoverDto>>(mechanicHandovers);

        var paginatedResult = new PaginatedList<MechanicHandoverDto>(mechanicHandoverDtos, mechanicHandovers.TotalCount, mechanicHandovers.CurrentPage, mechanicHandovers.PageSize);

        return paginatedResult.ToResponse();
    }

    public async Task<MechanicHandoverDto?> GetMechanicHandoverByIdAsync(int id)
    {
        var mechanicHandover = await _context.MechanicsHandovers
            .Include(d => d.Car)
            .Include(a => a.Driver)
            .ThenInclude(a => a.Account)
            .Include(m => m.Mechanic)
            .ThenInclude(m => m.Account)
            .FirstOrDefaultAsync(x => x.Id == id);

        var mechanicHandoverDto = _mapper.Map<MechanicHandoverDto>(mechanicHandover);

        return mechanicHandoverDto;
    }

    public async Task<MechanicHandoverDto> CreateMechanicHandoverAsync(MechanicHandoverForCreateDto handoverForCreateDto)
    {
        var mechanicHandoverEntity = _mapper.Map<MechanicHandover>(handoverForCreateDto);

        await _context.MechanicsHandovers.AddAsync(mechanicHandoverEntity);
        await _context.SaveChangesAsync();

        var mechanicHandoverDto = _mapper.Map<MechanicHandoverDto>(mechanicHandoverEntity);

        return mechanicHandoverDto;
    }

    public async Task<MechanicHandoverDto> UpdateMechanicHandoverAsync(MechanicHandoverForUpdateDto handoverForUpdateDto)
    {
        var mechanicHandoverEntity = _mapper.Map<MechanicHandover>(handoverForUpdateDto);

        _context.MechanicsHandovers.Update(mechanicHandoverEntity);
        await _context.SaveChangesAsync();

        var mechanicHandoverDto = _mapper.Map<MechanicHandoverDto>(mechanicHandoverEntity);

        return mechanicHandoverDto;
    }

    public async Task DeleteMechanicHandoverAsync(int id)
    {
        var mechanicHandover = await _context.MechanicsHandovers.FirstOrDefaultAsync(x => x.Id == id);

        if (mechanicHandover is not null)
        {
            _context.MechanicsHandovers.Remove(mechanicHandover);
        }

        await _context.SaveChangesAsync();
    }

    private IQueryable<MechanicHandover> GetQueryMechanicHandoverResParameters(
   MechanicHandoverResourceParameters resourceParameters)
    {
        var query = _context.MechanicsHandovers
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

        if (resourceParameters.IsHanded is not null)
        {
            query = query.Where(x => x.IsHanded == resourceParameters.IsHanded);
        }

        return query;
    }
}

