using AutoMapper;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.Pagniation;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDrive.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using CheckDrive.ApiContracts.MechanicAcceptance;
using CheckDrive.Domain.Interfaces.Hubs;
using CheckDrive.ApiContracts.OperatorReview;
using CheckDrive.ApiContracts.MechanicHandover;

namespace CheckDrive.Services;

public class MechanicAcceptanceService : IMechanicAcceptanceService
{
    private readonly IMapper _mapper;
    private readonly CheckDriveDbContext _context;
    private readonly IChatHub _chatHub;

    public MechanicAcceptanceService(IMapper mapper, CheckDriveDbContext context, IChatHub chatHub)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _chatHub = chatHub ?? throw new ArgumentNullException(nameof(chatHub));
    }

    public async Task<GetBaseResponse<MechanicAcceptanceDto>> GetMechanicAcceptencesAsync(MechanicAcceptanceResourceParameters resourceParameters)
    {
        var query = GetQueryMechanicAcceptanceResParameters(resourceParameters);

        query = query.OrderByDescending(item => item.Date);

        var mechanicAcceptances = await query.ToPaginatedListAsync(resourceParameters.PageSize, resourceParameters.PageNumber);

        var mechanicAcceptanceDtos = _mapper.Map<List<MechanicAcceptanceDto>>(mechanicAcceptances);

        var paginatedResult = new PaginatedList<MechanicAcceptanceDto>(mechanicAcceptanceDtos, mechanicAcceptances.TotalCount, mechanicAcceptances.CurrentPage, mechanicAcceptances.PageSize);

        return paginatedResult.ToResponse();

    }

    public async Task<MechanicAcceptanceDto?> GetMechanicAcceptenceByIdAsync(int id)
    {
        var mechanicAcceptance = await _context.MechanicsAcceptances
            .AsNoTracking()
            .Include(d => d.Car)
            .Include(a => a.Driver)
            .ThenInclude(a => a.Account)
            .Include(m => m.Mechanic)
            .ThenInclude(m => m.Account)
            .FirstOrDefaultAsync(x => x.Id == id);

        var mechanicAcceptanceDto = _mapper.Map<MechanicAcceptanceDto>(mechanicAcceptance);

        return mechanicAcceptanceDto;
    }

    public async Task<MechanicAcceptanceDto> CreateMechanicAcceptenceAsync(MechanicAcceptanceForCreateDto acceptanceForCreateDto)
    {
        var mechanicAcceptanceEntity = _mapper.Map<MechanicAcceptance>(acceptanceForCreateDto);

        await _context.MechanicsAcceptances.AddAsync(mechanicAcceptanceEntity);
        await _context.SaveChangesAsync();

        if (mechanicAcceptanceEntity.IsAccepted == true)
        {
            var data = await GetMechanicAcceptenceByIdAsync(mechanicAcceptanceEntity.Id);

            await _chatHub.SendPrivateRequest
                (SendingMessageStatus.MechanicAcceptance, mechanicAcceptanceEntity.Id, data.AccountDriverId.ToString(), 
                $"Siz shu {data.CarName} rusumli avtomobilni {data.MechanicName} ga topshirdizmi ?");
        }
        
        var mechanicAcceptanceDto = _mapper.Map<MechanicAcceptanceDto>(mechanicAcceptanceEntity);

        return mechanicAcceptanceDto;
    }

    public async Task<MechanicAcceptanceDto> UpdateMechanicAcceptenceAsync(MechanicAcceptanceForUpdateDto acceptanceForUpdateDto)
    {
        var mechanicAcceptanceEntity = _mapper.Map<MechanicAcceptance>(acceptanceForUpdateDto);

        _context.MechanicsAcceptances.Update(mechanicAcceptanceEntity);
        await _context.SaveChangesAsync();

        var mechanicAcceptanceDto = _mapper.Map<MechanicAcceptanceDto>(mechanicAcceptanceEntity);

        return mechanicAcceptanceDto;
    }

    public async Task DeleteMechanicAcceptenceAsync(int id)
    {
        var mechanicAcceptance = await _context.MechanicsAcceptances.FirstOrDefaultAsync(x => x.Id == id);

        if (mechanicAcceptance is not null)
        {
            _context.MechanicsAcceptances.Remove(mechanicAcceptance);
        }

        await _context.SaveChangesAsync();
    }

    private IQueryable<MechanicAcceptance> GetQueryMechanicAcceptanceResParameters(
       MechanicAcceptanceResourceParameters resourceParameters)
    {
        var query = _context.MechanicsAcceptances
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

        if (resourceParameters.IsAccepted is not null)
            query = query.Where(x => x.IsAccepted == resourceParameters.IsAccepted);

        //Distance
        if (resourceParameters.Distance is not null)
        {
            query = query.Where(x => x.Distance == resourceParameters.Distance);
        }
        if (resourceParameters.DistanceLessThan is not null)
        {
            query = query.Where(x => x.Distance < resourceParameters.DistanceLessThan);
        }
        if (resourceParameters.DistanceGreaterThan is not null)
        {
            query = query.Where(x => x.Distance > resourceParameters.DistanceGreaterThan);
        }

        if (resourceParameters.DriverId is not null)
        {
            query = query.Where(x => x.DriverId == resourceParameters.DriverId);
        }
        if (!string.IsNullOrEmpty(resourceParameters.OrderBy))
        {
            query = resourceParameters.OrderBy.ToLowerInvariant() switch
            {
                "date" => query.OrderBy(x => x.Date),
                "datedesc" => query.OrderByDescending(x => x.Date),
                _ => query.OrderBy(x => x.Id),
            };
        }

        return query;
    }

    public async Task<GetBaseResponse<MechanicAcceptanceDto>> GetMechanicAcceptencesForMechanicAsync(MechanicAcceptanceResourceParameters resourceParameters)
    {
        var response = await _context.MechanicsAcceptances
            .AsNoTracking()
            .Where(x => x.Date.Date == DateTime.Today)
            .Include(x => x.Mechanic)
            .ThenInclude(x => x.Account)
            .Include(x => x.Car)
            .Include(x => x.Driver)
            .ThenInclude(x => x.Account)
            .ToListAsync();

        var operatorReviewsResponse = await _context.OperatorReviews
            .AsNoTracking()
            .Where(dr => dr.Date.Date == DateTime.Today && dr.IsGiven == true)
            .Include(x => x.Operator)
            .ThenInclude(x => x.Account)
            .Include(x => x.Driver)
            .ThenInclude(x => x.Account)
            .Include(x => x.Car)
            .ToListAsync();

        var mechanicAcceptance = new List<MechanicAcceptanceDto>();

        foreach (var operatorr in operatorReviewsResponse)
        {
            var review = response.FirstOrDefault(r => r.DriverId == operatorr.DriverId);
            var reviewDto = _mapper.Map<MechanicAcceptanceDto>(review);
            var operatorReviewDto = _mapper.Map<OperatorReviewDto>(operatorr);
            if (review != null)
            {
                mechanicAcceptance.Add(new MechanicAcceptanceDto
                {
                    CarId = reviewDto.CarId,
                    CarName = reviewDto.CarName,
                    DriverId = reviewDto.DriverId,
                    DriverName = operatorReviewDto.DriverName,
                    MechanicName = reviewDto.MechanicName,
                    IsAccepted = reviewDto.IsAccepted,
                    Distance = reviewDto.Distance,
                    Comments = reviewDto.Comments,
                    Date = reviewDto.Date
                });
            }
            else
            {
                mechanicAcceptance.Add(new MechanicAcceptanceDto
                {
                    DriverId = operatorReviewDto.DriverId,
                    DriverName = operatorReviewDto.DriverName,
                    CarId = operatorReviewDto.CarId,
                    CarName = operatorReviewDto.CarModel,
                    MechanicName = "",
                    IsAccepted = false,
                    Distance = 0,
                    Comments = "",
                    Date = null
                });
            }
        }

        var filteredReviews = ApplyFilters(resourceParameters, mechanicAcceptance);
        var paginatedResult = PaginateReviews(filteredReviews, resourceParameters.PageSize, resourceParameters.PageNumber);

        return paginatedResult.ToResponse();
    }

    private List<MechanicAcceptanceDto> ApplyFilters(MechanicAcceptanceResourceParameters parameters, List<MechanicAcceptanceDto> reviews)
    {
        var query = reviews.AsQueryable();

        if (!string.IsNullOrWhiteSpace(parameters.SearchString))
            query = query.Where(
                x => x.DriverName.Contains(parameters.SearchString) ||
                x.MechanicName.Contains(parameters.SearchString) ||
                x.CarName.Contains(parameters.SearchString) ||
                x.Comments.Contains(parameters.SearchString));

        if (parameters.Date != null)
            query = query.Where(x => x.Date.Value.Date == parameters.Date.Value.Date);

        if (parameters.DriverId != null)
            query = query.Where(x => x.DriverId == parameters.DriverId);

        if (!string.IsNullOrEmpty(parameters.OrderBy))
            query = parameters.OrderBy.ToLowerInvariant() switch
            {
                "date" => query.OrderBy(x => x.Date),
                "datedesc" => query.OrderByDescending(x => x.Date),
                _ => query.OrderBy(x => x.DriverId),
            };

        return query.ToList();
    }

    private PaginatedList<MechanicAcceptanceDto> PaginateReviews(List<MechanicAcceptanceDto> reviews, int pageSize, int pageNumber)
    {
        var totalCount = reviews.Count;
        var items = reviews.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        return new PaginatedList<MechanicAcceptanceDto>(items, totalCount, pageNumber, pageSize);
    }
}

