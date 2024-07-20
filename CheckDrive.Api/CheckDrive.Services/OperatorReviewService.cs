using AutoMapper;
using CheckDrive.ApiContracts;
using CheckDrive.ApiContracts.Car;
using CheckDrive.ApiContracts.MechanicHandover;
using CheckDrive.ApiContracts.OperatorReview;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Hubs;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.Pagniation;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDrive.Infrastructure.Persistence;
using CheckDrive.Services.Hubs;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Services
{
    public class OperatorReviewService : IOperatorReviewService
    {
        private readonly IMapper _mapper;
        private readonly CheckDriveDbContext _context;
        private readonly IChatHub _chat;
        private readonly ICarService _carService;

        public OperatorReviewService(IMapper mapper, CheckDriveDbContext context, IChatHub chatHub, ICarService carService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _chat = chatHub ?? throw new ArgumentNullException(nameof(chatHub));
            _carService = carService ?? throw new ArgumentNullException(nameof(carService));
        }

        public async Task<GetBaseResponse<OperatorReviewDto>> GetOperatorReviewsAsync(OperatorReviewResourceParameters resourceParameters)
        {
            var query = GetQueryOperatorReviewResParameters(resourceParameters);

            var operatorReviews = await query.ToPaginatedListAsync(resourceParameters.PageSize, resourceParameters.PageNumber);

            var operatorReviewsDto = _mapper.Map<List<OperatorReviewDto>>(operatorReviews);

            if (resourceParameters.Status == Status.Completed)
            {
                var countOfHealthyDrivers = query.Count();
                operatorReviews.PageSize = countOfHealthyDrivers;
            }

            var paginatedResult = new PaginatedList<OperatorReviewDto>(operatorReviewsDto, operatorReviews.TotalCount, operatorReviews.CurrentPage, operatorReviews.PageSize);

            return paginatedResult.ToResponse();
        }

        public async Task<OperatorReviewDto?> GetOperatorReviewByIdAsync(int id)
        {
            var operatorReview = await _context.OperatorReviews
                .AsNoTracking()
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

                await _chat.SendPrivateRequest(new UndeliveredMessageForDto
                {
                    SendingMessageStatus = (SendingMessageStatusForDto)SendingMessageStatus.MechanicHandover,
                    ReviewId = operatorReviewEntity.Id,
                    UserId = data.AccountDriverId.ToString(),
                    Message = $"Sizga {data.OperatorName} shu {data.CarModel} avtomobilga {data.OilMarks} markali {data.OilAmount} litr benzin quydimi ?"
                });
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
                .AsNoTracking()
                .Include(a => a.Operator)
                .ThenInclude(a => a.Account)
                .Include(o => o.Driver)
                .ThenInclude(o => o.Account)
                .Include(o => o.Car)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(operatorReviewResource.SearchString))
                query = query.Where(
                    x => x.Driver.Account.FirstName.Contains(operatorReviewResource.SearchString) ||
                    x.Driver.Account.LastName.Contains(operatorReviewResource.SearchString) ||
                    x.Operator.Account.FirstName.Contains(operatorReviewResource.SearchString) ||
                    x.Operator.Account.LastName.Contains(operatorReviewResource.SearchString) ||
                    x.Comments.Contains(operatorReviewResource.SearchString));

            if (operatorReviewResource.Status is not null)
                query = query.Where(x => x.Status == operatorReviewResource.Status);

            if (operatorReviewResource.Date is not null)
                query = query.Where(x => x.Date.Date == operatorReviewResource.Date.Value.Date);

            if (operatorReviewResource.OilAmount is not null)
                query = query.Where(x => x.OilAmount == operatorReviewResource.OilAmount);

            if (operatorReviewResource.OilAmountLessThan is not null)
                query = query.Where(x => x.OilAmount < operatorReviewResource.OilAmountLessThan);

            if (operatorReviewResource.OilAmountGreaterThan is not null)
                query = query.Where(x => x.OilAmount > operatorReviewResource.OilAmountGreaterThan);

            if (operatorReviewResource.IsGiven is not null)
                query = query.Where(x => x.IsGiven == operatorReviewResource.IsGiven);

            if (operatorReviewResource.DriverId is not null)
                query = query.Where(x => x.DriverId == operatorReviewResource.DriverId);

            if (operatorReviewResource.CarId is not null)
                query = query.Where(x => x.CarId == operatorReviewResource.CarId);

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

        public async Task<GetBaseResponse<OperatorReviewDto>> GetOperatorReviewsForOperatorAsync(OperatorReviewResourceParameters resourceParameters)
        {
            var reviewsResponse = await _context.OperatorReviews
                .AsNoTracking()
                .Where(x => x.Date.Date == DateTime.Today)
                .Include(x => x.Operator)
                .ThenInclude(x => x.Account)
                .Include(x => x.Driver)
                .ThenInclude(x => x.Account)
                .Include(x => x.Car)
                .ToListAsync();

            var mechanicHandoverResponse = await _context.MechanicsHandovers
                .AsNoTracking()
                .Where(x => x.Date.Date == DateTime.Today && x.Status == Status.Completed)
                .Include(x => x.Mechanic)
                .ThenInclude(x => x.Account)
                .Include(x => x.Car)
                .Include(x => x.Driver)
                .ThenInclude(x => x.Account)
                .ToListAsync();

            var cars = await _context.Cars
                .ToListAsync();

            var operators = new List<OperatorReviewDto>();

            foreach (var mechanicHandover in mechanicHandoverResponse)
            {
                var car = cars.FirstOrDefault(c => c.Id == mechanicHandover.CarId);
                var review = reviewsResponse.FirstOrDefault(r => r.DriverId == mechanicHandover.DriverId);
                var carDto = _mapper.Map<CarDto>(car);
                var reviewDto = _mapper.Map<OperatorReviewDto>(review);
                var mechanicHandoverDto = _mapper.Map<MechanicHandoverDto>(mechanicHandover);
                if (review != null)
                {
                    operators.Add(new OperatorReviewDto
                    {
                        DriverId = reviewDto.DriverId,
                        DriverName = mechanicHandoverDto.DriverName,
                        OperatorName = reviewDto.OperatorName,
                        CarId = carDto?.Id ?? reviewDto.CarId,
                        CarModel = carDto?.Model ?? reviewDto.CarModel,
                        CarNumber = carDto?.Number ?? reviewDto.CarNumber,
                        CarOilCapacity = car?.FuelTankCapacity.ToString() ?? reviewDto.CarOilCapacity,
                        CarOilRemainig = car?.RemainingFuel.ToString() ?? reviewDto.CarOilRemainig,
                        OilAmount = reviewDto.OilAmount,
                        OilMarks = reviewDto.OilMarks,
                        IsGiven = reviewDto.IsGiven,
                        Comments = reviewDto.Comments,
                        Date = reviewDto.Date,
                        Status = reviewDto.Status
                    });
                }
                else
                {
                    operators.Add(new OperatorReviewDto
                    {
                        DriverId = mechanicHandoverDto.DriverId,
                        DriverName = mechanicHandoverDto.DriverName,
                        OperatorName = null,
                        CarId = carDto?.Id ?? 0,
                        CarModel = carDto?.Model ?? string.Empty,
                        CarNumber = carDto?.Number ?? string.Empty,
                        CarOilCapacity = carDto?.FuelTankCapacity.ToString() ?? string.Empty,
                        CarOilRemainig = carDto?.RemainingFuel.ToString() ?? string.Empty,
                        OilAmount = null,
                        OilMarks = null,
                        IsGiven = null,
                        Comments = null,
                        Date = null,
                        Status = StatusForDto.Unassigned
                    });
                }
            }

            var filteredReviews = ApplyFilters(resourceParameters, operators);
            var paginatedResult = PaginateReviews(filteredReviews, resourceParameters.PageSize, resourceParameters.PageNumber);

            return paginatedResult.ToResponse();
        }

        private List<OperatorReviewDto> ApplyFilters(OperatorReviewResourceParameters parameters, List<OperatorReviewDto> reviews)
        {
            var query = reviews.AsQueryable();

            if (!string.IsNullOrWhiteSpace(parameters.SearchString))
            {
                var searchString = parameters.SearchString.ToLowerInvariant();
                query = query.Where(x =>
                    (!string.IsNullOrEmpty(x.DriverName) && x.DriverName.ToLowerInvariant().Contains(searchString)) ||
                    (!string.IsNullOrEmpty(x.OperatorName) && x.OperatorName.ToLowerInvariant().Contains(searchString)) ||
                    (!string.IsNullOrEmpty(x.CarModel) && x.CarModel.ToLowerInvariant().Contains(searchString)) ||
                    (!string.IsNullOrEmpty(x.Comments) && x.Comments.ToLowerInvariant().Contains(searchString)));
            }

            if (parameters.Date is not null)
                query = query.Where(x => x.Date.Value.Date == parameters.Date.Value.Date);

            if (parameters.Status is not null)
                query = query.Where(x => x.Status == (StatusForDto)parameters.Status);

            if (parameters.OilAmount is not null)
                query = query.Where(x => x.OilAmount == parameters.OilAmount);

            if (parameters.OilAmountLessThan is not null)
                query = query.Where(x => x.OilAmount < parameters.OilAmountLessThan);

            if (parameters.OilAmountGreaterThan is not null)
                query = query.Where(x => x.OilAmount > parameters.OilAmountGreaterThan);

            if (parameters.IsGiven is not null)
                query = query.Where(x => x.IsGiven == parameters.IsGiven);

            if (parameters.DriverId is not null)
                query = query.Where(x => x.DriverId == parameters.DriverId);

            if (parameters.CarId is not null)
                query = query.Where(x => x.CarId == parameters.CarId);

            if (!string.IsNullOrEmpty(parameters.OrderBy))
                query = parameters.OrderBy.ToLowerInvariant() switch
                {
                    "date" => query.OrderBy(x => x.Date),
                    "datedesc" => query.OrderByDescending(x => x.Date),
                    _ => query.OrderBy(x => x.DriverId),
                };

            return query.ToList();
        }

        private PaginatedList<OperatorReviewDto> PaginateReviews(List<OperatorReviewDto> reviews, int pageSize, int pageNumber)
        {
            var totalCount = reviews.Count;
            var items = reviews.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<OperatorReviewDto>(items, totalCount, pageNumber, pageSize);
        }

        public async Task<IEnumerable<OperatorReviewDto>> GetOpearatorHistories(int? Id)
        {
            var _operator = await _context.Operators
                .Where(x => x.AccountId == Id)
                .FirstOrDefaultAsync();

            var operatorHistories = _context.OperatorReviews
                .AsNoTracking()
                .Include(o => o.Operator)
                .ThenInclude(a => a.Account)
                .Include(d => d.Driver)
                .ThenInclude(a => a.Account)
                .Include(c => c.Car)
                .Where(x => x.OperatorId == _operator.Id)
                .OrderByDescending(x => x.Date)
                .AsQueryable();

            var operatorReviewDto = _mapper.Map<IEnumerable<OperatorReviewDto>>(operatorHistories);

            return operatorReviewDto;
        }
    }
}
