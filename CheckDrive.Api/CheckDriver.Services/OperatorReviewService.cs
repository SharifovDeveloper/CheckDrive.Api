using AutoMapper;
using CheckDrive.Domain.DTOs.OperatorReview;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.Pagniation;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDriver.Domain.Entities;
using CheckDriver.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Services
{
    public class OperatorReviewService : IOperatorReviewService
    {
        private readonly IMapper _mapper;
        private readonly CheckDriveDbContext _context;

        public OperatorReviewService(IMapper mapper, CheckDriveDbContext context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<GetBaseResponse<OperatorReviewDto>> GetOperatorReviewsAsync(OperatorReviewResourceParameters resourceParameters)
        {
            var query = GetQueryOperatorReviewResParameters(resourceParameters);

            var operatorReviews = await query.ToPaginatedListAsync(resourceParameters.PageSize, resourceParameters.PageNumber);

            var operatorReviewsDto = _mapper.Map<List<OperatorReviewDto>>(operatorReviews);

            var paginatedResult = new PaginatedList<OperatorReviewDto>(operatorReviewsDto, operatorReviews.TotalCount, operatorReviews.CurrentPage, operatorReviews.PageSize);

            return paginatedResult.ToResponse();
        }

        public async Task<OperatorReviewDto?> GetOperatorReviewByIdAsync(int id)
        {
            var operatorReview = await _context.OperatorReviews.FirstOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<OperatorReviewDto>(operatorReview);
        }

        public async Task<OperatorReviewDto> CreateOperatorReviewAsync(OperatorReviewForCreateDto reviewForCreateDto)
        {
            var operatorReviewEntity = _mapper.Map<OperatorReview>(reviewForCreateDto);

            await _context.OperatorReviews.AddAsync(operatorReviewEntity);
            await _context.SaveChangesAsync();

            return _mapper.Map<OperatorReviewDto>(operatorReviewEntity);
        }

        public async Task<OperatorReviewDto> UpdateOperatorReviewAsync(OperatorReviewForUpdateDto reviewForUpdateDto)
        {
            var operatorReviewEntity = _mapper.Map<OperatorReview>(reviewForUpdateDto);

            _context.OperatorReviews.Update(operatorReviewEntity);
            await _context.SaveChangesAsync();

            return _mapper.Map<OperatorReviewDto>(operatorReviewEntity);
        }

        public async Task DeleteOperatorReviewAsync(int id)
        {
            var operatorReview = await _context.OperatorReviews.FirstOrDefaultAsync(x => x.Id == id);

            if (operatorReview is not null)
            {
                _context.OperatorReviews.Remove(operatorReview);
            }

            await _context.SaveChangesAsync();
        }

        private IQueryable<OperatorReview> GetQueryOperatorReviewResParameters(
       OperatorReviewResourceParameters operatorReviewResource)
        {
            var query = _context.OperatorReviews.AsQueryable();

            if (operatorReviewResource.Date is not null)
            {
                query = query.Where(x => x.Date.Date == operatorReviewResource.Date.Value.Date);
            }
            if (operatorReviewResource.OilAmount is not null)
            {
                query = query.Where(x => x.OilAmount == operatorReviewResource.OilAmount);
            }
            if (operatorReviewResource.OilAmountLessThan is not null)
            {
                query = query.Where(x => x.OilAmount < operatorReviewResource.OilAmountLessThan);
            }
            if (operatorReviewResource.OilAmountGreaterThan is not null)
            {
                query = query.Where(x => x.OilAmount > operatorReviewResource.OilAmountGreaterThan);
            }
            return query;
        }
    }
}
