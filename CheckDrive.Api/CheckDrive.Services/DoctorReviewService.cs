﻿using AutoMapper;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.Pagniation;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDrive.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using CheckDrive.ApiContracts.DoctorReview;
using CheckDrive.Domain.Interfaces.Hubs;
using CheckDrive.ApiContracts.Driver;

namespace CheckDrive.Services;

public class DoctorReviewService : IDoctorReviewService
{
    private readonly IMapper _mapper;
    private readonly CheckDriveDbContext _context;
    private readonly IChatHub _chatHub;

    public DoctorReviewService(IMapper mapper, CheckDriveDbContext context)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetBaseResponse<DoctorReviewDto>> GetDoctorReviewsAsync(DoctorReviewResourceParameters resourceParameters)
    {
        var query = GetQueryDoctorReviewResParameters(resourceParameters);

        var doctorReviews = await query.ToPaginatedListAsync(resourceParameters.PageSize, resourceParameters.PageNumber);

        var doctorReviewsDto = _mapper.Map<List<DoctorReviewDto>>(doctorReviews);

        var paginatedResult = new PaginatedList<DoctorReviewDto>(doctorReviewsDto, doctorReviews.TotalCount, doctorReviews.CurrentPage, doctorReviews.PageSize);

        return paginatedResult.ToResponse();
    }

    public async Task<GetBaseResponse<DoctorReviewDto>> GetDoctorReviewsForDoctorAsync(DoctorReviewResourceParameters resourceParameters)
    {
        var reviewsResponse = await _context.DoctorReviews
            .AsNoTracking()
            .Where(x => x.Date.Date == DateTime.Today)
            .Include(x => x.Doctor)
            .ThenInclude(x => x.Account)
            .Include(x => x.Driver)
            .ThenInclude(x => x.Account)
            .ToListAsync();

        var driversResponse = await _context.Drivers
            .AsNoTracking()
            .Include(x => x.Account)
            .ToListAsync();

        var doctorReviews = new List<DoctorReviewDto>();

        foreach (var driver in driversResponse)
        {
            var review = reviewsResponse.FirstOrDefault(r => r.DriverId == driver.Id);
            var driverDto = _mapper.Map<DriverDto>(driver);
            var reviewDto = _mapper.Map<DoctorReviewDto>(review);
            if (review != null)
            {
                doctorReviews.Add(new DoctorReviewDto
                {
                    Id = review.Id,
                    DriverId = driverDto.Id,
                    DriverName = $"{driverDto.FirstName} {driverDto.LastName}",
                    DoctorId = reviewDto.DoctorId,
                    DoctorName = reviewDto.DoctorName,
                    IsHealthy = reviewDto.IsHealthy,
                    Comments = reviewDto.Comments,
                    Date = reviewDto.Date
                });
            }
            else
            {
                doctorReviews.Add(new DoctorReviewDto
                {
                    DriverId = driverDto.Id,
                    DriverName = $"{driverDto.FirstName} {driverDto.LastName}",
                    DoctorName = "",
                    IsHealthy = false,
                    Comments = "",
                    Date = DateTime.Today
                });
            }
        }

        var filteredReviews = ApplyFilters(resourceParameters, doctorReviews);
        var paginatedResult = PaginateReviews(filteredReviews, resourceParameters.PageSize, resourceParameters.PageNumber);

        return paginatedResult.ToResponse();
    }

    public async Task<DoctorReviewDto?> GetDoctorReviewByIdAsync(int id)
    {
        var doctorReview = await _context.DoctorReviews
            .AsNoTracking()
            .Include(d => d.Driver)
            .ThenInclude(d => d.Account)
            .Include(a => a.Doctor)
            .ThenInclude(a => a.Account)
            .FirstOrDefaultAsync(x => x.Id == id);

        var doctorReviewDto = _mapper.Map<DoctorReviewDto>(doctorReview);

        return doctorReviewDto;
    }

    public async Task<DoctorReviewDto> CreateDoctorReviewAsync(DoctorReviewForCreateDto doctorReviewForCreate)
    {
        var doctorReviewEntity = _mapper.Map<DoctorReview>(doctorReviewForCreate);

        await _context.DoctorReviews.AddAsync(doctorReviewEntity);
        await _context.SaveChangesAsync();

        var doctorReviewDto = _mapper.Map<DoctorReviewDto>(doctorReviewEntity);

        return doctorReviewDto;
    }

    public async Task<DoctorReviewDto> UpdateDoctorReviewAsync(DoctorReviewForUpdateDto doctorReviewForUpdate)
    {
        var doctorReviewEntity = _mapper.Map<DoctorReview>(doctorReviewForUpdate);

        _context.DoctorReviews.Update(doctorReviewEntity);
        await _context.SaveChangesAsync();

        var doctorReviewDto = _mapper.Map<DoctorReviewDto>(doctorReviewEntity);

        return doctorReviewDto;
    }

    public async Task DeleteDoctorReviewAsync(int id)
    {
        var doctorReview = await _context.DoctorReviews.FirstOrDefaultAsync(x => x.Id == id);

        if (doctorReview is not null)
        {
            _context.DoctorReviews.Remove(doctorReview);
        }

        await _context.SaveChangesAsync();
    }

    private IQueryable<DoctorReview> GetQueryDoctorReviewResParameters(
       DoctorReviewResourceParameters doctorReviewResource)
    {
        var query = _context.DoctorReviews
            .AsNoTracking()
            .Include(d => d.Driver)
            .ThenInclude(d => d.Account)
            .Include(a => a.Doctor)
            .ThenInclude(a => a.Account)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(doctorReviewResource.SearchString))
            query = query.Where(
                x => x.Driver.Account.FirstName.Contains(doctorReviewResource.SearchString) ||
                x.Driver.Account.LastName.Contains(doctorReviewResource.SearchString) ||
                x.Doctor.Account.FirstName.Contains(doctorReviewResource.SearchString) ||
                x.Doctor.Account.LastName.Contains(doctorReviewResource.SearchString) ||
                x.Comments.Contains(doctorReviewResource.SearchString));
        
        if (doctorReviewResource.Date is not null)
            query = query.Where(x => x.Date.Date == doctorReviewResource.Date.Value.Date);
        
        if (doctorReviewResource.DriverId is not null)
            query = query.Where(x => x.DriverId == doctorReviewResource.DriverId);
        
        if (!string.IsNullOrEmpty(doctorReviewResource.OrderBy))
            query = doctorReviewResource.OrderBy.ToLowerInvariant() switch
            {
                "date" => query.OrderBy(x => x.Date),
                "datedesc" => query.OrderByDescending(x => x.Date),
                _ => query.OrderBy(x => x.Id),
            };

        return query;
    }

    private List<DoctorReviewDto> ApplyFilters(DoctorReviewResourceParameters parameters, List<DoctorReviewDto> reviews)
    {
        var query = reviews.AsQueryable();

        if (!string.IsNullOrWhiteSpace(parameters.SearchString))
            query = query.Where(
                x => x.DriverName.Contains(parameters.SearchString) ||
                x.DoctorName.Contains(parameters.SearchString) ||
                x.Comments.Contains(parameters.SearchString));

        if (parameters.Date != null)
            query = query.Where(x => x.Date.Date == parameters.Date.Value.Date);

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

    private PaginatedList<DoctorReviewDto> PaginateReviews(List<DoctorReviewDto> reviews, int pageSize, int pageNumber)
    {
        var totalCount = reviews.Count;
        var items = reviews.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        return new PaginatedList<DoctorReviewDto>(items, totalCount, pageNumber, pageSize);
    }
}

