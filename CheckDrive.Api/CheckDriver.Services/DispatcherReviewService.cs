using AutoMapper;
using CheckDricer.Infrastructure.Persistence;
using CheckDrive.Domain.DTOs.DispatcherReview;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.Pagniation;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDriver.Domain.Entities;
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
        var query = GetQueryCarResParameters(resourceParameters);

        var dispatcherReviews = await query.ToPaginatedListAsync(resourceParameters.PageSize, resourceParameters.PageNumber);

        var dispatcherReviewsDto = _mapper.Map<List<DispatcherReviewDto>>(dispatcherReviews);

        var paginatedResult = new PaginatedList<DispatcherReviewDto>(dispatcherReviewsDto, dispatcherReviews.TotalCount, dispatcherReviews.CurrentPage, dispatcherReviews.PageSize);

        return paginatedResult.ToResponse();
    }

    public async Task<DispatcherReviewDto?> GetDispatcherReviewByIdAsync(int id)
    {
        var dispatcherReview = await _context.DispatchersReviews.FirstOrDefaultAsync(x => x.Id == id);

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

    private IQueryable<DispatcherReview> GetQueryCarResParameters(
       DispatcherReviewResourceParameters dispatcherReviewParameters)
    {
        var query = _context.DispatchersReviews.AsQueryable();

        //FuelSpended
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

        return query;
    }
}

