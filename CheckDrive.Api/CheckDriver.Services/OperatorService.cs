using AutoMapper;
using CheckDrive.Domain.DTOs.Operator;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.Pagniation;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDriver.Domain.Entities;
using CheckDriver.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Services;
public class OperatorService : IOperatorService
{
    private readonly IMapper _mapper;
    private readonly CheckDriveDbContext _context;

    public OperatorService(IMapper mapper, CheckDriveDbContext context)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task<GetBaseResponse<OperatorDto>> GetOperatorsAsync(OperatorResourceParameters resourceParameters)
    {
        var query = GetQueryOperatorResParameters(resourceParameters);

        var _operator = await query.ToPaginatedListAsync(resourceParameters.PageSize, resourceParameters.PageNumber);

        var operatorDtos = _mapper.Map<List<OperatorDto>>(_operator);

        var paginatedResult = new PaginatedList<OperatorDto>(operatorDtos, _operator.TotalCount, _operator.CurrentPage, _operator.PageSize);

        return paginatedResult.ToResponse();
    }
    public async Task<OperatorDto?> GetOperatorByIdAsync(int id)
    {
        var _operator = await _context.Operators.FirstOrDefaultAsync(x => x.Id == id);

        return _mapper.Map<OperatorDto>(_operator);
    }
    public async Task<OperatorDto> CreateOperatorAsync(OperatorForCreateDto operatorForCreate)
    {
        var operatorEntity = _mapper.Map<Operator>(operatorForCreate);

        await _context.Operators.AddAsync(operatorEntity);
        await _context.SaveChangesAsync();

        return _mapper.Map<OperatorDto>(operatorEntity);
    }

    public async Task<OperatorDto> UpdateOperatorAsync(OperatorForUpdateDto operatorForUpdate)
    {
        var operatorEntity = _mapper.Map<Operator>(operatorForUpdate);

        _context.Operators.Update(operatorEntity);
        await _context.SaveChangesAsync();

        return _mapper.Map<OperatorDto>(operatorEntity);
    }

    public async Task DeleteOperatorAsync(int id)
    {
        var _operator = await _context.Operators.FirstOrDefaultAsync(x => x.Id == id);

        if (_operator is not null)
        {
            _context.Operators.Remove(_operator);
        }

        await _context.SaveChangesAsync();
    }
    private IQueryable<Operator> GetQueryOperatorResParameters(
       OperatorResourceParameters resourceParameters)
    {
        var query = _context.Operators.AsQueryable();

        if (resourceParameters.AccountId != 0 && resourceParameters.AccountId is not null)
        {
            query = query.Where(x => x.AccountId == resourceParameters.AccountId);
        }

        return query;
    }
}

