using AutoMapper;
using CheckDrive.ApiContracts.Mechanic;
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
}

