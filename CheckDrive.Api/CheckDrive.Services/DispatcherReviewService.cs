using AutoMapper;
using CheckDrive.ApiContracts.Car;
using CheckDrive.ApiContracts.DispatcherReview;
using CheckDrive.ApiContracts.MechanicAcceptance;
using CheckDrive.ApiContracts.MechanicHandover;
using CheckDrive.ApiContracts.OperatorReview;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.Pagniation;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDrive.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Services;

public class DispatcherReviewService : IDispatcherReviewService
{
    private readonly IMapper _mapper;
    private readonly CheckDriveDbContext _context;

    public DispatcherReviewService(IMapper mapper, CheckDriveDbContext context)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetBaseResponse<DispatcherReviewDto>> GetDispatcherReviewsAsync(DispatcherReviewResourceParameters resourceParameters)
    {
        var query = GetQueryDispatcherReviewResParameters(resourceParameters);

        var dispatcherReviews = await query.ToPaginatedListAsync(resourceParameters.PageSize, resourceParameters.PageNumber);

        var dispatcherReviewsDto = _mapper.Map<List<DispatcherReviewDto>>(dispatcherReviews);

        var paginatedResult = new PaginatedList<DispatcherReviewDto>(dispatcherReviewsDto, dispatcherReviews.TotalCount, dispatcherReviews.CurrentPage, dispatcherReviews.PageSize);

        return paginatedResult.ToResponse();
    }

    public async Task<DispatcherReviewDto?> GetDispatcherReviewByIdAsync(int id)
    {
        var dispatcherReview = await _context.DispatchersReviews
            .AsNoTracking()
            .Include(x => x.Car)
            .Include(d => d.Driver)
            .ThenInclude(d => d.Account)
            .Include(d => d.Mechanic)
            .ThenInclude(d => d.Account)
            .Include(d => d.Operator)
            .ThenInclude(d => d.Account)
            .Include(d => d.Dispatcher)
            .ThenInclude(d => d.Account)
            .FirstOrDefaultAsync(x => x.Id == id);

        var dispatcherReviewDto = _mapper.Map<DispatcherReviewDto>(dispatcherReview);

        return dispatcherReviewDto;
    }

    public async Task<DispatcherReviewDto> CreateDispatcherReviewAsync(DispatcherReviewForCreateDto dispatcherReviewForCreate)
    {
        var dispatcherEntity = _mapper.Map<DispatcherReview>(dispatcherReviewForCreate);

        await _context.DispatchersReviews.AddAsync(dispatcherEntity);
        await _context.SaveChangesAsync();

        var dispatcherReviewDto = _mapper.Map<DispatcherReviewDto>(dispatcherEntity);

        return dispatcherReviewDto;
    }

    public async Task<DispatcherReviewDto> UpdateDispatcherReviewAsync(DispatcherReviewForUpdateDto dispatcherReviewForUpdate)
    {
        var dispatcherEntity = _mapper.Map<DispatcherReview>(dispatcherReviewForUpdate);

        _context.DispatchersReviews.Update(dispatcherEntity);
        await _context.SaveChangesAsync();

        var dispatcherReviewDto = _mapper.Map<DispatcherReviewDto>(dispatcherEntity);

        return dispatcherReviewDto;
    }

    public async Task DeleteDispatcherReviewAsync(int id)
    {
        var dispatcherReview = await _context.DispatchersReviews.FirstOrDefaultAsync(x => x.Id == id);

        if (dispatcherReview is not null)
        {
            _context.DispatchersReviews.Remove(dispatcherReview);
        }

        await _context.SaveChangesAsync();
    }

    private IQueryable<DispatcherReview> GetQueryDispatcherReviewResParameters(
       DispatcherReviewResourceParameters dispatcherReviewParameters)
    {
        var query = _context.DispatchersReviews
            .AsNoTracking()
            .Include(ma => ma.MechanicAcceptance)
            .Include(mh => mh.MechanicHandover)
            .Include(o => o.OperatorReview)
            .Include(d => d.Driver)
            .ThenInclude(d => d.Account)
            .Include(d => d.Mechanic)
            .ThenInclude(d => d.Account)
            .Include(d => d.Operator)
            .ThenInclude(d => d.Account)
            .Include(d => d.Dispatcher)
            .ThenInclude(d => d.Account)
            .Include(d => d.Car)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(dispatcherReviewParameters.SearchString))
            query = query.Where(
                x => x.Driver.Account.FirstName.Contains(dispatcherReviewParameters.SearchString) ||
                x.Driver.Account.LastName.Contains(dispatcherReviewParameters.SearchString) ||
                x.Mechanic.Account.FirstName.Contains(dispatcherReviewParameters.SearchString) ||
                x.Mechanic.Account.LastName.Contains(dispatcherReviewParameters.SearchString) ||
                x.Operator.Account.FirstName.Contains(dispatcherReviewParameters.SearchString) ||
                x.Operator.Account.LastName.Contains(dispatcherReviewParameters.SearchString) ||
                x.Dispatcher.Account.FirstName.Contains(dispatcherReviewParameters.SearchString) ||
                x.Dispatcher.Account.LastName.Contains(dispatcherReviewParameters.SearchString));

        if (dispatcherReviewParameters.Date is not null)
            query = query.Where(x => x.Date.Date == dispatcherReviewParameters.Date.Value.Date);

        //FuelSpended
        if (dispatcherReviewParameters.DriverId is not null)
        {
            query = query.Where(x => x.DriverId == dispatcherReviewParameters.DriverId);
        }
        if (dispatcherReviewParameters.FuelSpended is not null)
        {
            query = query.Where(x => x.FuelSpended == dispatcherReviewParameters.FuelSpended);
        }
        if (dispatcherReviewParameters.FuelSpendedLessThan is not null)
        {
            query = query.Where(x => x.FuelSpended < dispatcherReviewParameters.FuelSpendedLessThan);
        }
        if (dispatcherReviewParameters.FuelSpendedGreaterThan is not null)
        {
            query = query.Where(x => x.FuelSpended > dispatcherReviewParameters.FuelSpendedGreaterThan);
        }

        //DistanceCovered
        if (dispatcherReviewParameters.DistanceCovered is not null)
        {
            query = query.Where(x => x.DistanceCovered == dispatcherReviewParameters.DistanceCovered);
        }
        if (dispatcherReviewParameters.DistanceCoveredLessThan is not null)
        {
            query = query.Where(x => x.DistanceCovered < dispatcherReviewParameters.DistanceCoveredLessThan);
        }
        if (dispatcherReviewParameters.DistanceCoveredGreaterThan is not null)
        {
            query = query.Where(x => x.DistanceCovered > dispatcherReviewParameters.DistanceCoveredGreaterThan);
        }

        if (!string.IsNullOrEmpty(dispatcherReviewParameters.OrderBy))
        {
            query = dispatcherReviewParameters.OrderBy.ToLowerInvariant() switch
            {
                "date" => query.OrderBy(x => x.Date),
                "datedesc" => query.OrderByDescending(x => x.Date),
                _ => query.OrderBy(x => x.Id),
            };
        }
        return query;
    }

    public async Task<GetBaseResponse<DispatcherReviewDto>> GetDispatcherReviewsForDispatcherAsync(DispatcherReviewResourceParameters resourceParameters)
    {
        var reviewsResponse = await _context.DispatchersReviews
            .AsNoTracking()
            .Where(x => x.Date.Date == DateTime.UtcNow.Date)
            .Include(x => x.Car)
            .Include(x => x.Mechanic)
            .ThenInclude(x => x.Account)
            .Include(x => x.Driver)
            .ThenInclude(x => x.Account)
            .Include(x => x.Operator)
            .ThenInclude(x => x.Account)
            .Include(x => x.MechanicAcceptance)
            .Include(x => x.MechanicHandover)
            .Include(x => x.OperatorReview)
            .ToListAsync();

        var mechanicAcceptanceResponse = await _context.MechanicsAcceptances
            .AsNoTracking()
            .Where(x => x.Date.Date == DateTime.UtcNow.Date && x.Status == Status.Completed)
            .Include(x => x.Mechanic)
            .ThenInclude(x => x.Account)
            .Include(x => x.Car)
            .Include(x => x.Driver)
            .ThenInclude(x => x.Account)
            .ToListAsync();

        var mechanicHandoverResponse = await _context.MechanicsHandovers
            .AsNoTracking()
            .Where(x => x.Date.Date == DateTime.UtcNow.Date && x.Status == Status.Completed)
            .Include(x => x.Mechanic)
            .ThenInclude(x => x.Account)
            .Include(x => x.Car)
            .Include(x => x.Driver)
            .ThenInclude(x => x.Account)
            .ToListAsync();

        var operatorResponse = await _context.OperatorReviews
            .AsNoTracking()
            .Where(x => x.Date.Date == DateTime.UtcNow.Date && x.Status == Status.Completed)
            .Include(x => x.Operator)
            .ThenInclude(x => x.Account)
            .Include(x => x.Driver)
            .ThenInclude(x => x.Account)
            .Include(x => x.Car)
            .ToListAsync();

        var carResponse = await _context.Cars
            .ToListAsync();

        var dispatchers = new List<DispatcherReviewDto>();

        foreach (var mechanicAcceptance in mechanicAcceptanceResponse)
        {
            var mechanicHandoverReview = mechanicHandoverResponse.FirstOrDefault(m => m.DriverId == mechanicAcceptance.DriverId && m.Date.Date == DateTime.UtcNow.Date);
            var operatorReview = operatorResponse.FirstOrDefault(m => m.DriverId == mechanicAcceptance.DriverId && m.Date.Date == DateTime.UtcNow.Date);
            var carReview = carResponse.FirstOrDefault(c => c.Id == mechanicAcceptance.CarId);
            var review = reviewsResponse.FirstOrDefault(r => r.DriverId == mechanicAcceptance.DriverId);

            var mechanicHandoverReviewDto = _mapper.Map<MechanicHandoverDto>(mechanicHandoverReview);
            var mechanicAcceptanceDto = _mapper.Map<MechanicAcceptanceDto>(mechanicAcceptance);
            var operatorReviewDto = _mapper.Map<OperatorReviewDto>(operatorReview);
            var carReviewDto = _mapper.Map<CarDto>(carReview);
            var reviewDto = _mapper.Map<DispatcherReviewDto>(review);

            if (review != null)
            {
                dispatchers.Add(new DispatcherReviewDto
                {
                    DriverId = reviewDto.DriverId,
                    DriverName = mechanicAcceptanceDto.DriverName,
                    CarId = reviewDto.CarId,
                    CarName = reviewDto.CarName,
                    CarMeduimFuelConsumption = reviewDto.CarMeduimFuelConsumption,
                    FuelSpended = reviewDto.FuelSpended,
                    DistanceCovered = reviewDto.DistanceCovered,
                    InitialDistance = reviewDto.InitialDistance,
                    FinalDistance = reviewDto.FinalDistance,
                    PouredFuel = reviewDto.PouredFuel,
                    OperatorName = reviewDto.OperatorName,
                    OperatorReviewId = reviewDto.OperatorReviewId,
                    DispatcherName = reviewDto.DispatcherName,
                    MechanicName = reviewDto.MechanicName,
                    Date = reviewDto.Date,
                    DispatcherId = reviewDto.DispatcherId,
                    MechanicAcceptanceId = reviewDto.MechanicAcceptanceId,
                    MechanicHandoverId = reviewDto.MechanicHandoverId,
                    OperatorId = reviewDto.OperatorId,
                    MechanicId = reviewDto.MechanicId,
                });
            }
            else
            {
                dispatchers.Add(new DispatcherReviewDto
                {
                    DriverId = mechanicAcceptanceDto.DriverId,
                    DriverName = mechanicAcceptanceDto.DriverName,
                    CarId = mechanicAcceptanceDto.CarId,
                    CarName = mechanicAcceptanceDto.CarName,
                    CarMeduimFuelConsumption = carReviewDto.MeduimFuelConsumption,
                    FuelSpended = (mechanicAcceptanceDto.Distance - mechanicHandoverReviewDto.Distance) / carReviewDto.MeduimFuelConsumption,
                    DistanceCovered = mechanicAcceptanceDto.Distance - mechanicHandoverReviewDto.Distance,
                    InitialDistance = mechanicHandoverReviewDto.Distance,
                    FinalDistance = mechanicAcceptanceDto.Distance,
                    PouredFuel = operatorReviewDto.OilAmount ?? 0,
                    OperatorName = operatorReviewDto.OperatorName,
                    OperatorReviewId = operatorReviewDto.Id,
                    DispatcherName = "",
                    MechanicName = mechanicAcceptanceDto.MechanicName,
                    Date = DateTime.UtcNow.Date,
                    MechanicAcceptanceId = mechanicAcceptanceDto.Id,
                    MechanicHandoverId = mechanicHandoverReviewDto.Id,
                    OperatorId = operatorReviewDto.OperatorId,
                    MechanicId = mechanicAcceptanceDto.MechanicId
                });
            }
        }

        var filteredReviews = ApplyFilters(resourceParameters, dispatchers);
        var paginatedResult = PaginateReviews(filteredReviews, resourceParameters.PageSize, resourceParameters.PageNumber);

        return paginatedResult.ToResponse();
    }

    private List<DispatcherReviewDto> ApplyFilters(DispatcherReviewResourceParameters dispatcherReviewParameters, List<DispatcherReviewDto> reviews)
    {
        var query = reviews.AsQueryable();

        if (!string.IsNullOrWhiteSpace(dispatcherReviewParameters.SearchString))
        {
            var searchString = dispatcherReviewParameters.SearchString.ToLowerInvariant();
            query = query.Where(x =>
                (!string.IsNullOrEmpty(x.DriverName) && x.DriverName.ToLowerInvariant().Contains(searchString)) ||
                (!string.IsNullOrEmpty(x.MechanicName) && x.MechanicName.ToLowerInvariant().Contains(searchString)) ||
                (!string.IsNullOrEmpty(x.OperatorName) && x.OperatorName.ToLowerInvariant().Contains(searchString)) ||
                (!string.IsNullOrEmpty(x.DispatcherName) && x.DispatcherName.ToLowerInvariant().Contains(searchString) ||
                (!string.IsNullOrEmpty(x.CarName) && x.CarName.ToLowerInvariant().Contains(searchString))));
        }

        if (dispatcherReviewParameters.Date is not null)
            query = query.Where(x => x.Date.Date == dispatcherReviewParameters.Date.Value.Date);

        if (dispatcherReviewParameters.DriverId is not null)
            query = query.Where(x => x.DriverId == dispatcherReviewParameters.DriverId);
        if (dispatcherReviewParameters.FuelSpended is not null)
            query = query.Where(x => x.FuelSpended == dispatcherReviewParameters.FuelSpended);
        if (dispatcherReviewParameters.FuelSpendedLessThan is not null)
            query = query.Where(x => x.FuelSpended < dispatcherReviewParameters.FuelSpendedLessThan);
        if (dispatcherReviewParameters.FuelSpendedGreaterThan is not null)
            query = query.Where(x => x.FuelSpended > dispatcherReviewParameters.FuelSpendedGreaterThan);

        if (dispatcherReviewParameters.DistanceCovered is not null)
            query = query.Where(x => x.DistanceCovered == dispatcherReviewParameters.DistanceCovered);
        if (dispatcherReviewParameters.DistanceCoveredLessThan is not null)
            query = query.Where(x => x.DistanceCovered < dispatcherReviewParameters.DistanceCoveredLessThan);
        if (dispatcherReviewParameters.DistanceCoveredGreaterThan is not null)
            query = query.Where(x => x.DistanceCovered > dispatcherReviewParameters.DistanceCoveredGreaterThan);

        if (!string.IsNullOrEmpty(dispatcherReviewParameters.OrderBy))
        {
            query = dispatcherReviewParameters.OrderBy.ToLowerInvariant() switch
            {
                "date" => query.OrderBy(x => x.Date),
                "datedesc" => query.OrderByDescending(x => x.Date),
                _ => query.OrderBy(x => x.DriverId),
            };
        }

        return query.ToList();
    }

    private PaginatedList<DispatcherReviewDto> PaginateReviews(List<DispatcherReviewDto> reviews, int pageSize, int pageNumber)
    {
        var totalCount = reviews.Count;
        var items = reviews.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        return new PaginatedList<DispatcherReviewDto>(items, totalCount, pageNumber, pageSize);
    }

    public async Task<IEnumerable<DispatcherReviewDto>> GetDispatcherHistories(int? Id)
    {
        var dispatcher = await _context.Dispatchers
                .Where(x => x.AccountId == Id)
                .FirstOrDefaultAsync();

        var dispatcherHistories = _context.DispatchersReviews
            .AsNoTracking()
            .Include(ma => ma.MechanicAcceptance)
            .Include(mh => mh.MechanicHandover)
            .Include(o => o.OperatorReview)
            .Include(d => d.Driver)
            .ThenInclude(d => d.Account)
            .Include(d => d.Mechanic)
            .ThenInclude(d => d.Account)
            .Include(d => d.Operator)
            .ThenInclude(d => d.Account)
            .Include(d => d.Dispatcher)
            .ThenInclude(d => d.Account)
            .Include(d => d.Car)
            .Where(x => x.DispatcherId == dispatcher.Id)
            .OrderByDescending(x => x.Date)
            .AsQueryable();

        var dispatcherReviewDto = _mapper.Map<IEnumerable<DispatcherReviewDto>>(dispatcherHistories);

        return dispatcherReviewDto;
    }
}

