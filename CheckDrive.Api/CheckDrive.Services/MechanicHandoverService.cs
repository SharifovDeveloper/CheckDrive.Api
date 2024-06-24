using AutoMapper;
using CheckDrive.ApiContracts.MechanicHandover;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Hubs;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.Pagniation;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDrive.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Services;

public class MechanicHandoverService : IMechanicHandoverService
{
    private readonly IMapper _mapper;
    private readonly CheckDriveDbContext _context;
    private readonly IChatHub _chatHub;

    public MechanicHandoverService(IMapper mapper, CheckDriveDbContext context, IChatHub chatHub)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _chatHub = chatHub ?? throw new ArgumentNullException(nameof(chatHub));
    }

    public async Task<GetBaseResponse<MechanicHandoverDto>> GetMechanicHandoversAsync(MechanicHandoverResourceParameters resourceParameters)
    {
        var query = GetQueryMechanicHandoverResParameters(resourceParameters);

        query = query.OrderByDescending(item => item.Date);

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

        if (mechanicHandoverEntity.IsHanded == true)
        {
            var data = await GetMechanicHandoverByIdAsync(mechanicHandoverEntity.Id);

            await _chatHub.SendPrivateRequest
                (SendingMessageStatus.MechanicHandover, mechanicHandoverEntity.Id, data.AccountDriverId.ToString(), $"Siz shu moshinani oldizmi {data.CarName}");
        }

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
            .AsNoTracking()
            .Include(d => d.Car)
            .Include(a => a.Driver)
            .ThenInclude(a => a.Account)
            .Include(m => m.Mechanic)
            .ThenInclude(m => m.Account)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(resourceParameters.SearchString))
            query = query.Where(
                x => x.Driver.Account.FirstName.Contains(resourceParameters.SearchString) ||
                x.Driver.Account.LastName.Contains(resourceParameters.SearchString) ||
                x.Mechanic.Account.FirstName.Contains(resourceParameters.SearchString) ||
                x.Mechanic.Account.LastName.Contains(resourceParameters.SearchString) ||
                x.Comments.Contains(resourceParameters.SearchString));

        if (resourceParameters.Date is not null)
            query = query.Where(x => x.Date.Date == resourceParameters.Date.Value.Date);

        if (resourceParameters.Status is not null)
            query = query.Where(x => x.Status == resourceParameters.Status);

        if (resourceParameters.IsHanded is not null)
            query = query.Where(x => x.IsHanded == resourceParameters.IsHanded);

        if (resourceParameters.DriverId is not null)
            query = query.Where(x => x.DriverId == resourceParameters.DriverId);

        if (!string.IsNullOrEmpty(resourceParameters.OrderBy))
            query = resourceParameters.OrderBy.ToLowerInvariant() switch
            {
                "date" => query.OrderBy(x => x.Date),
                "datedesc" => query.OrderByDescending(x => x.Date),
                _ => query.OrderBy(x => x.Id),
            };

        if (resourceParameters.Distance is not null)
            query = query.Where(x => x.Distance == resourceParameters.Distance);
        if (resourceParameters.DistanceLessThan is not null)
            query = query.Where(x => x.Distance < resourceParameters.DistanceLessThan);
        if (resourceParameters.DistanceGreaterThan is not null)
            query = query.Where(x => x.Distance > resourceParameters.DistanceGreaterThan);

        return query;
    }
}

