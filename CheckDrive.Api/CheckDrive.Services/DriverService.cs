using AutoMapper;
using CheckDrive.ApiContracts.Driver;
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

        if (resourceParameters.RoleId == 1)
        {
            var countOfDrivers = query.Count();
            resourceParameters.PageSize = countOfDrivers;
        }

        var drivers = await query.ToPaginatedListAsync(resourceParameters.PageSize, resourceParameters.PageNumber);

        var driverDtos = _mapper.Map<List<DriverDto>>(drivers);



        var paginatedResult = new PaginatedList<DriverDto>(driverDtos, drivers.TotalCount, drivers.CurrentPage, drivers.PageSize);

        return paginatedResult.ToResponse();
    }

    public async Task<DriverDto?> GetDriverByIdAsync(int id)
    {
        var driver = await _context.Drivers
            .AsNoTracking()
            .Include(x => x.Account)
            .FirstOrDefaultAsync(x => x.Id == id);

        var driverDto = _mapper.Map<DriverDto>(driver);

        return driverDto;
    }
    public async Task<Driver?> GetDriverIdByAccountId(int accountId)
    {
        var driver = await _context.Drivers
            .Where(x => x.AccountId == accountId)
            .FirstOrDefaultAsync();

        return driver;
    }
    public async Task<DriverDto> CreateDriverAsync(DriverForCreateDto driverForCreate)
    {
        var accountEntity = _mapper.Map<Account>(driverForCreate);
        await _context.Accounts.AddAsync(accountEntity);
        await _context.SaveChangesAsync();

        var driver = new Driver() { AccountId = accountEntity.Id };
        await _context.Drivers.AddAsync(driver);
        await _context.SaveChangesAsync();

        var accountDto = _mapper.Map<DriverDto>(accountEntity);

        return accountDto;
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
        var query = _context.Drivers
            .AsNoTracking()
            .Include(x => x.Account)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(resourceParameters.SearchString))
        {
            query = query.Where(x => x.Account.FirstName.Contains(resourceParameters.SearchString)
            || x.Account.LastName.Contains(resourceParameters.SearchString));
        }

        if (resourceParameters.AccountId != 0 && resourceParameters.AccountId is not null)
        {
            query = query.Where(x => x.AccountId == resourceParameters.AccountId);
        }
        return query;
    }

    public async Task<IEnumerable<DriverHistoryDto>> GetDriverHistories(int? driverId)
    {

        var driverHistory = new List<DriverHistoryDto>();

        var doctorReview = await _context.DoctorReviews
            .AsNoTracking()
            .Where(x => x.DriverId == driverId)
            .OrderByDescending(x => x.Date)
            .Take(20)
            .ToArrayAsync();


        var mechanicHandovers = await _context.MechanicsHandovers
            .AsNoTracking()
            .Where(x => x.DriverId == driverId)
            .OrderByDescending(x => x.Date)
            .Take(20)
            .ToArrayAsync();

        var operatorReviews = await _context.OperatorReviews
            .AsNoTracking()
            .Where(x => x.DriverId == driverId)
            .OrderByDescending(x => x.Date)
            .Take(20)
            .ToArrayAsync();

        var mechanicAcceptance = await _context.MechanicsAcceptances
            .AsNoTracking()
            .Where(x => x.DriverId == driverId)
            .OrderByDescending(x => x.Date)
            .Take(20)
            .ToArrayAsync();

        foreach (var item in doctorReview)
        {
            bool mechanicHandover = false;
            bool operatorReview = false;
            bool mechanicAccept = false;
            int mechanichandOverId = 0;
            int operatorId = 0;
            int mechanicAcceptId = 0;

            if (item.Date.Date == DateTime.Today)
            {
                continue;
            }
            var mechanicHandoverThisDay = mechanicHandovers.FirstOrDefault(m => m.Date.Date == item.Date.Date);
            if (mechanicHandoverThisDay != null && mechanicHandoverThisDay.Status == Status.Completed)
            {
                mechanichandOverId = mechanicHandoverThisDay.Id;
                mechanicHandover = true;
            }
            else if (mechanicHandoverThisDay != null)
            {
                mechanichandOverId = mechanicHandoverThisDay.Id;
            }
            var operatorReviewThisDay = operatorReviews.FirstOrDefault(o => o.Date.Date == item.Date.Date);
            if (operatorReviewThisDay != null && operatorReviewThisDay.Status == Status.Completed)
            {
                operatorId = operatorReviewThisDay.Id;
                operatorReview = true;
            }
            else if (operatorReviewThisDay != null)
            {
                operatorId = operatorReviewThisDay.Id;
            }

            var mechanicAcceptThisDay = mechanicAcceptance.FirstOrDefault(o => o.Date.Date == item.Date.Date);
            if (mechanicAcceptThisDay != null && mechanicAcceptThisDay.Status == Status.Completed)
            {
                mechanicAcceptId = mechanicAcceptThisDay.Id;
                mechanicAccept = true;
            }
            else if (mechanicAcceptThisDay != null)
            {
                mechanicAcceptId = mechanicAcceptThisDay.Id;
            }

            driverHistory.Add(new DriverHistoryDto()
            {
                Date = item.Date,
                DoctorReviewId = item.Id,
                IsHealthy = item.IsHealthy,
                IsHanded = mechanicHandover,
                MechanicHandoverId = mechanichandOverId,
                IsGiven = operatorReview,
                OperatorReviewId = operatorId,
                IsAccepted = mechanicAccept,
                MechanicAcceptanceId = mechanicAcceptId,
            });
        }
        return driverHistory;
    }
}

