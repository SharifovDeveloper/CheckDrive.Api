using AutoMapper;
using CheckDrive.ApiContracts;
using CheckDrive.ApiContracts.DoctorReview;
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

        var mechanicHandovers = await query.ToPaginatedListAsync(resourceParameters.PageSize, resourceParameters.PageNumber);

        var mechanicHandoverDtos = _mapper.Map<List<MechanicHandoverDto>>(mechanicHandovers);

        if (resourceParameters.Status == Status.Completed)
        {
            var countOfHealthyDrivers = query.Count();
            mechanicHandovers.PageSize = countOfHealthyDrivers;
        }

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
        var car = _context.Cars.FirstOrDefault(x => x.Id == mechanicHandoverEntity.CarId);

        if (car != null)
        {
            car.Mileage = (int)mechanicHandoverEntity.Distance;
            _context.Cars.Update(car);

        }
        await _context.SaveChangesAsync();

        if (mechanicHandoverEntity.IsHanded == true)
        {
            var data = await GetMechanicHandoverByIdAsync(mechanicHandoverEntity.Id);
            var carData = await _context.Cars.FirstOrDefaultAsync(x => x.Id == data.CarId);

            await _chatHub.SendPrivateRequest(new UndeliveredMessageForDto
            {
                SendingMessageStatus = (SendingMessageStatusForDto)SendingMessageStatus.MechanicHandover,
                ReviewId = mechanicHandoverEntity.Id,
                UserId = data.AccountDriverId.ToString(),
                Message = $"Sizga {data.MechanicName} {data.CarName} ni {carData.RemainingFuel} l yoqilg'isi va {carData.Mileage} km bosib o'tilgan masofasi bilan topshirdimi ?"
            });
        }

        var mechanicHandoverDto = _mapper.Map<MechanicHandoverDto>(mechanicHandoverEntity);

        return mechanicHandoverDto;
    }

    public async Task<MechanicHandoverDto> UpdateMechanicHandoverAsync(MechanicHandoverForUpdateDto handoverForUpdateDto)
    {
        var mechanicHandoverEntity = _mapper.Map<MechanicHandover>(handoverForUpdateDto);

        _context.MechanicsHandovers.Update(mechanicHandoverEntity);
        var car = _context.Cars.FirstOrDefault(x => x.Id == mechanicHandoverEntity.CarId);

        if (car != null)
        {
            car.Mileage = (int)mechanicHandoverEntity.Distance;
            _context.Cars.Update(car);

        }
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

    public async Task<GetBaseResponse<MechanicHandoverDto>> GetMechanicHandoversForMechanicsAsync(MechanicHandoverResourceParameters resourceParameters)
    {
        var response = await _context.MechanicsHandovers
            .AsNoTracking()
            .Where(x => x.Date.Date == DateTime.UtcNow.Date)
            .Include(x => x.Mechanic)
            .ThenInclude(x => x.Account)
            .Include(x => x.Car)
            .Include(x => x.Driver)
            .ThenInclude(x => x.Account)
            .ToListAsync();

        var doctorReviewsResponse = await _context.DoctorReviews
            .AsNoTracking()
            .Where(x => x.IsHealthy == true && x.Date.Date == DateTime.UtcNow.Date)
            .Include(x => x.Doctor)
            .ThenInclude(x => x.Account)
            .Include(x => x.Driver)
            .ThenInclude(x => x.Account)
            .ToListAsync();

        var mechanicHandovers = new List<MechanicHandoverDto>();

        foreach (var doctor in doctorReviewsResponse)
        {
            var review = response.FirstOrDefault(r => r.DriverId == doctor.DriverId);
            var doctorDto = _mapper.Map<DoctorReviewDto>(doctor);
            var reviewDto = _mapper.Map<MechanicHandoverDto>(review);
            if (review != null)
            {
                mechanicHandovers.Add(new MechanicHandoverDto
                {
                    Id = reviewDto.Id,
                    DriverId = reviewDto.DriverId,
                    DriverName = doctorDto.DriverName,
                    MechanicName = reviewDto.MechanicName,
                    IsHanded = reviewDto.IsHanded,
                    Distance = reviewDto.Distance,
                    Comments = reviewDto.Comments,
                    Date = reviewDto.Date,
                    CarId = reviewDto.CarId,
                    CarName = reviewDto.CarName,
                    MechanicId = reviewDto.MechanicId,
                    Status = reviewDto.Status,
                });
            }
            else
            {
                mechanicHandovers.Add(new MechanicHandoverDto
                {
                    DriverId = doctorDto.DriverId,
                    DriverName = doctorDto.DriverName,
                    CarName = "",
                    MechanicName = "",
                    IsHanded = false,
                    Distance = 0,
                    Comments = "",
                    Date = DateTime.UtcNow.Date,
                    Status = ApiContracts.StatusForDto.Unassigned,
                });
            }
        }

        var filteredReviews = ApplyFilters(resourceParameters, mechanicHandovers);
        var paginatedResult = PaginateReviews(filteredReviews, resourceParameters.PageSize, resourceParameters.PageNumber);

        return paginatedResult.ToResponse();
    }

    private List<MechanicHandoverDto> ApplyFilters(MechanicHandoverResourceParameters parameters, List<MechanicHandoverDto> reviews)
    {
        var query = reviews.AsQueryable();

        if (!string.IsNullOrWhiteSpace(parameters.SearchString))
        {
            var searchString = parameters.SearchString.ToLowerInvariant();
            query = query.Where(x =>
                (!string.IsNullOrEmpty(x.DriverName) && x.DriverName.ToLowerInvariant().Contains(searchString)) ||
                (!string.IsNullOrEmpty(x.MechanicName) && x.MechanicName.ToLowerInvariant().Contains(searchString)) ||
                (!string.IsNullOrEmpty(x.CarName) && x.CarName.ToLowerInvariant().Contains(searchString)) ||
                (!string.IsNullOrEmpty(x.Comments) && x.Comments.ToLowerInvariant().Contains(searchString)));
        }

        if (parameters.Date is not null)
            query = query.Where(x => x.Date.Date == parameters.Date.Value.Date);

        if (parameters.IsHanded is not null)
            query = query.Where(x => x.IsHanded == parameters.IsHanded);

        if (parameters.Status is not null)
            query = query.Where(x => x.Status == (StatusForDto)parameters.Status);

        if (parameters.DriverId is not null)
            query = query.Where(x => x.DriverId == parameters.DriverId);

        if (!string.IsNullOrEmpty(parameters.OrderBy))
            query = parameters.OrderBy.ToLowerInvariant() switch
            {
                "date" => query.OrderBy(x => x.Date),
                "datedesc" => query.OrderByDescending(x => x.Date),
                _ => query.OrderBy(x => x.Id),
            };

        if (parameters.Distance is not null)
            query = query.Where(x => x.Distance == parameters.Distance);
        if (parameters.DistanceLessThan is not null)
            query = query.Where(x => x.Distance < parameters.DistanceLessThan);
        if (parameters.DistanceGreaterThan is not null)
            query = query.Where(x => x.Distance > parameters.DistanceGreaterThan);

        return query.ToList();
    }

    private PaginatedList<MechanicHandoverDto> PaginateReviews(List<MechanicHandoverDto> reviews, int pageSize, int pageNumber)
    {
        var totalCount = reviews.Count;
        var items = reviews.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        return new PaginatedList<MechanicHandoverDto>(items, totalCount, pageNumber, pageSize);
    }
}

