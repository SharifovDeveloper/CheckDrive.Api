using AutoMapper;
using CheckDrive.ApiContracts.OperatorReview;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Domain.Mappings
{
    public class OperatorReviewMappings : Profile
    {
        public OperatorReviewMappings()
        {
            CreateMap<OperatorReviewDto, OperatorReview>();
            CreateMap<OperatorReview, OperatorReviewDto>()
                .ForMember(x => x.DriverName, f => f.MapFrom(e => $"{e.Driver.Account.FirstName} {e.Driver.Account.LastName}"))
                .ForMember(x => x.OperatorName, f => f.MapFrom(e => $"{e.Operator.Account.FirstName} {e.Operator.Account.LastName}"))
                .ForMember(x => x.CarModel, f => f.MapFrom(e => e.Car.Model))
                .ForMember(x => x.CarNumber, f => f.MapFrom(e => e.Car.Number))
                .ForMember(x => x.CarOilCapacity, f => f.MapFrom(e => e.Car.FuelTankCapacity))
                .ForMember(x => x.CarOilRemainig, f => f.MapFrom(e => e.Car.RemainingFuel))
                .ForMember(x => x.AccountDriverId, f => f.MapFrom(e => e.Driver.Account.Id));
            CreateMap<OperatorReviewForCreateDto, OperatorReview>();
            CreateMap<OperatorReviewForUpdateDto, OperatorReview>();
        }
    }
}
