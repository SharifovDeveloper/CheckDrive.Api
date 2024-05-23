﻿using AutoMapper;
using CheckDrive.ApiContracts.DispatcherReview;
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
            .Include(d => d.Driver)
            .ThenInclude(d => d.Account)
            .Include(d => d.Mechanic)
            .ThenInclude(d => d.Account)
            .Include(d => d.Operator)
            .ThenInclude(d => d.Account)
            .Include(d => d.Dispatcher)
            .ThenInclude(d => d.Account)
            .AsQueryable();

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
                "datedesc" => query.OrderByDescending(x=>x.Date),
                _ => query.OrderBy(x => x.Id),
            };
        }
        return query;
    }
}

