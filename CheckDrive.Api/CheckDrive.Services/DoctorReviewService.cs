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

    public async Task<DoctorReviewDto?> GetDoctorReviewByIdAsync(int id)
    {
        var doctorReview = await _context.DoctorReviews
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
            .Include(d => d.Driver)
            .ThenInclude(d => d.Account)
            .Include(a => a.Doctor)
            .ThenInclude(a => a.Account)
            .AsQueryable();

        if (doctorReviewResource.Date is not null)
        {
            query = query.Where(x => x.Date.Date == doctorReviewResource.Date.Value.Date);
        }
        if (doctorReviewResource.DriverId is not null)
        {
            query = query.Where(x => x.DriverId == doctorReviewResource.DriverId);
        }
        if (!string.IsNullOrEmpty(doctorReviewResource.OrderBy))
        {
            query = doctorReviewResource.OrderBy.ToLowerInvariant() switch
            {
                "date" => query.OrderBy(x => x.Date),
                "datedesc" => query.OrderByDescending(x => x.Date),
                _ => query.OrderBy(x => x.Id),
            };
        }

        return query;
    }
}

