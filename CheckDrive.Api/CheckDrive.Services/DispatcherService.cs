using AutoMapper;
using CheckDrive.Domain.DTOs.Dispatcher;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.Pagniation;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDrive.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Services;

public class DispatcherService : IDispatcherService
{
    private readonly IMapper _mapper;
    private readonly CheckDriveDbContext _context;

    public DispatcherService(IMapper mapper, CheckDriveDbContext context)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task<GetBaseResponse<DispatcherDto>> GetDispatchersAsync(DispatcherResourceParameters resourceParameters)
    {
        var query = GetQueryDispatcherResParameters(resourceParameters);

        var dispatchers = await query.ToPaginatedListAsync(resourceParameters.PageSize, resourceParameters.PageNumber);

        var dispatcherDtos = _mapper.Map<List<DispatcherDto>>(dispatchers);

        var paginatedResult = new PaginatedList<DispatcherDto>(dispatcherDtos, dispatchers.TotalCount, dispatchers.CurrentPage, dispatchers.PageSize);

        return paginatedResult.ToResponse();
    }

    public async Task<DispatcherDto?> GetDispatcherByIdAsync(int id)
    {
        var dispatcher = await _context.Dispatchers.FirstOrDefaultAsync(x => x.Id == id);

        var dispatcherDto = _mapper.Map<DispatcherDto>(dispatcher);

        return dispatcherDto;
    }

    public async Task<DispatcherDto> CreateDispatcherAsync(DispatcherForCreateDto dispatcherForCreate)
    {
        var dispatcherEntity = _mapper.Map<Dispatcher>(dispatcherForCreate);

        await _context.Dispatchers.AddAsync(dispatcherEntity);
        await _context.SaveChangesAsync();

        var dispatcherDto = _mapper.Map<DispatcherDto>(dispatcherEntity);

        return dispatcherDto;
    }

    public async Task<DispatcherDto> UpdateDispatcherAsync(DispatcherForUpdateDto dispatcherForUpdate)
    {
        var dispatcherEntity = _mapper.Map<Dispatcher>(dispatcherForUpdate);

        _context.Dispatchers.Update(dispatcherEntity);
        await _context.SaveChangesAsync();

        var dispatcherDto = _mapper.Map<DispatcherDto>(dispatcherEntity);

        return dispatcherDto;
    }

    public async Task DeleteDispatcherAsync(int id)
    {
        var dispatcher = await _context.Dispatchers.FirstOrDefaultAsync(x => x.Id == id);

        if (dispatcher is not null)
        {
            _context.Dispatchers.Remove(dispatcher);
        }

        await _context.SaveChangesAsync();
    }

    private IQueryable<Dispatcher> GetQueryDispatcherResParameters(
           DispatcherResourceParameters resourceParameters)
    {
        var query = _context.Dispatchers.AsQueryable();

        if (resourceParameters.AccountId != 0 && resourceParameters.AccountId is not null)
        {
            query = query.Where(x => x.AccountId == resourceParameters.AccountId);
        }

        return query;
    }
}

