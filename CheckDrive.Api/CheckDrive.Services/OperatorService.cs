using AutoMapper;
using CheckDrive.ApiContracts.Operator;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Auth;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.Pagniation;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDrive.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Services;
public class OperatorService : IOperatorService
{
    private readonly IMapper _mapper;
    private readonly CheckDriveDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public OperatorService(IMapper mapper, CheckDriveDbContext context, IPasswordHasher passwordHasher)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
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
        var _operator = await _context.Operators.Include(x => x.Account).FirstOrDefaultAsync(x => x.Id == id);

        return _mapper.Map<OperatorDto>(_operator);
    }
    public async Task<OperatorDto> CreateOperatorAsync(OperatorForCreateDto operatorForCreate)
    {
        operatorForCreate.Password = _passwordHasher.Generate(operatorForCreate.Password);
        var accountEntity = _mapper.Map<Account>(operatorForCreate);
        await _context.Accounts.AddAsync(accountEntity);
        await _context.SaveChangesAsync();

        var _operator = new Operator() { AccountId = accountEntity.Id };
        await _context.Operators.AddAsync(_operator);
        await _context.SaveChangesAsync();

        var accountDto = _mapper.Map<OperatorDto>(accountEntity);

        return accountDto;
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
        var query = _context.Operators.Include(x => x.Account).AsQueryable();

        if (resourceParameters.AccountId != 0 && resourceParameters.AccountId is not null)
        {
            query = query.Where(x => x.AccountId == resourceParameters.AccountId);
        }

        return query;
    }
}

