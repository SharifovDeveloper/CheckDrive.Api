using AutoMapper;
using CheckDrive.ApiContracts.Mechanic;
using CheckDrive.ApiContracts.MechanicAcceptance;
using CheckDrive.ApiContracts.MechanicHandover;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.Pagniation;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDrive.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Services;

public class MechanicService : IMechanicService
{
    private readonly IMapper _mapper;
    private readonly CheckDriveDbContext _context;

    public MechanicService(IMapper mapper, CheckDriveDbContext context)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetBaseResponse<MechanicDto>> GetMechanicesAsync(MechanicResourceParameters resourceParameters)
    {
        var query = GetQueryMechanicResParameters(resourceParameters);

        var mechanics = await query.ToPaginatedListAsync(resourceParameters.PageSize, resourceParameters.PageNumber);

        var mechanicDtos = _mapper.Map<List<MechanicDto>>(mechanics);

        var paginatedResult = new PaginatedList<MechanicDto>(mechanicDtos, mechanics.TotalCount, mechanics.CurrentPage, mechanics.PageSize);

        return paginatedResult.ToResponse();
    }

    public async Task<MechanicDto?> GetMechanicByIdAsync(int id)
    {
        var machanic = await _context.Mechanics
            .AsNoTracking()
            .Include(x => x.Account)
            .FirstOrDefaultAsync(x => x.Id == id);

        return _mapper.Map<MechanicDto>(machanic);
    }

    public async Task<MechanicDto> CreateMechanicAsync(MechanicForCreateDto mechanicForCreate)
    {
        var accountEntity = _mapper.Map<Account>(mechanicForCreate);
        await _context.Accounts.AddAsync(accountEntity);
        await _context.SaveChangesAsync();

        var mechanic = new Mechanic() { AccountId = accountEntity.Id };
        await _context.Mechanics.AddAsync(mechanic);
        await _context.SaveChangesAsync();

        var accountDto = _mapper.Map<MechanicDto>(accountEntity);

        return accountDto;
    }

    public async Task DeleteMechanicAsync(int id)
    {
        var mechanic = await _context.Mechanics.FirstOrDefaultAsync(x => x.Id == id);

        if (mechanic is not null)
        {
            _context.Mechanics.Remove(mechanic);
        }

        await _context.SaveChangesAsync();
    }
    private IQueryable<Mechanic> GetQueryMechanicResParameters(
       MechanicResourceParameters resourceParameters)
    {
        var query = _context.Mechanics
            .AsNoTracking()
            .Include(x => x.Account)
            .AsQueryable();

        if (resourceParameters.AccountId != 0 && resourceParameters.AccountId is not null)
        {
            query = query.Where(x => x.AccountId == resourceParameters.AccountId);
        }

        return query;
    }

    public async Task<IEnumerable<MechanicHistororiesDto?>> GetMechanicHistories(int? Id)
    {
        var mechanicHistories = new List<MechanicHistororiesDto>();

        var mechanic = await _context.Mechanics
            .Where(x => x.AccountId == Id)
            .FirstOrDefaultAsync();

        var mechanicHandoverHistories = _context.MechanicsHandovers
            .AsNoTracking()
            .Include(x => x.Mechanic)
            .ThenInclude(x => x.Account)
            .Include(x => x.Car)
            .Include(x => x.Driver)
            .ThenInclude(x => x.Account)
            .Where(m => m.MechanicId == mechanic.Id)
            .OrderByDescending(d => d.Date)
            .AsQueryable();

        var mechanicHandoverDtos = _mapper.Map<List<MechanicHandoverDto>>(mechanicHandoverHistories);


        foreach (var item in mechanicHandoverDtos)
        {
            mechanicHistories.Add(new MechanicHistororiesDto
            {
                Date = item.Date,
                IsChecked = item.IsHanded,
                Position = "Topshiruvchi",
                CarName = item.CarName,
                Distance = item.Distance,
                DriverName = item.DriverName,
                Comments = item.Comments,
            });
        };

        var mechanicAcceptanceHistories = _context.MechanicsAcceptances
            .AsNoTracking()
            .Include(x => x.Mechanic)
            .ThenInclude(x => x.Account)
            .Include(x => x.Car)
            .Include(x => x.Driver)
            .ThenInclude(x => x.Account)
            .Where(m => m.MechanicId == mechanic.Id)
            .OrderByDescending(d => d.Date)
            .AsQueryable();

        var mechanicAcceptanceDtos = _mapper.Map<List<MechanicAcceptanceDto>>(mechanicAcceptanceHistories);

        foreach (var item in mechanicAcceptanceDtos)
        {
            mechanicHistories.Add(new MechanicHistororiesDto
            {
                Date = item.Date,
                IsChecked = item.IsAccepted,
                Position = "Qabul qiluvchi",
                CarName = item.CarName,
                Distance = item.Distance,
                DriverName = item.DriverName,
                Comments = item.Comments,
            });
        }

        return mechanicHistories;
    }
}

