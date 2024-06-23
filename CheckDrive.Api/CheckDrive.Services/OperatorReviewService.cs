using AutoMapper;
using CheckDrive.ApiContracts.OperatorReview;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Hubs;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.Pagniation;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDrive.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Services
{
    public class OperatorReviewService : IOperatorReviewService
    {
        private readonly IMapper _mapper;
        private readonly CheckDriveDbContext _context;
        private readonly IChatHub _chat;

        public OperatorReviewService(IMapper mapper, CheckDriveDbContext context, IChatHub chatHub)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _chat = chatHub ?? throw new ArgumentNullException(nameof(chatHub));
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
            var operatorReview = await _context.OperatorReviews
                .Include(a => a.Driver)
                .ThenInclude(a => a.Account)
                .Include(o => o.Operator)
                .ThenInclude(o => o.Account)
                .Include(o => o.Car)
                .FirstOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<OperatorReviewDto>(operatorReview);
        }

        public async Task<OperatorReviewDto> CreateOperatorReviewAsync(OperatorReviewForCreateDto reviewForCreateDto)
        {
            var operatorReviewEntity = _mapper.Map<OperatorReview>(reviewForCreateDto);

            await _context.OperatorReviews.AddAsync(operatorReviewEntity);
            await _context.SaveChangesAsync();

            if (operatorReviewEntity.IsGiven = true)
            {
                var data = await GetOperatorReviewByIdAsync(operatorReviewEntity.Id);
                
                await _chat.SendPrivateRequest
                    (SendingMessageStatus.OperatorReview, operatorReviewEntity.Id, data.AccountDriverId.ToString(), $"Shuncha benzin kuildimi{operatorReviewEntity.OilAmount}, benzin markasi{operatorReviewEntity.OilMarks}");
            }

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
            var query = _context.OperatorReviews
                .Include(a => a.Operator)
                .ThenInclude(a => a.Account)
                .Include(o => o.Driver)
                .ThenInclude(o => o.Account)
                .Include(o => o.Car)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(operatorReviewResource.SearchString))
            {
                query = query.Where(x => x.Driver.Account.FirstName.Contains(operatorReviewResource.SearchString)
                || x.Driver.Account.LastName.Contains(operatorReviewResource.SearchString));
            }

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
            if (operatorReviewResource.IsGiven is not null)
            {
                query = query.Where(x => x.IsGiven == operatorReviewResource.IsGiven);
            }
            if (operatorReviewResource.DriverId is not null)
            {
                query = query.Where(x => x.DriverId == operatorReviewResource.DriverId);
            }
            if (operatorReviewResource.CarId is not null)
            {
                query = query.Where(x => x.CarId == operatorReviewResource.CarId);
            }
            if (!string.IsNullOrEmpty(operatorReviewResource.OrderBy))
            {
                query = operatorReviewResource.OrderBy.ToLowerInvariant() switch
                {
                    "date" => query.OrderBy(x => x.Date),
                    "datedesc" => query.OrderByDescending(x => x.Date),
                    _ => query.OrderBy(x => x.Id),
                };
            }

            return query;
        }
    }
}
