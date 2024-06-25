using AutoMapper;
using CheckDrive.Domain.Entities;
using CheckDrive.ApiContracts.DispatcherReview;

namespace CheckDrive.Domain.Mappings
{
    public class DispatcherReviewMappings : Profile
    {
        public DispatcherReviewMappings()
        {
            CreateMap<DispatcherReviewDto, DispatcherReview>();
            CreateMap<DispatcherReview, DispatcherReviewDto>()
                .ForMember(d => d.CarName, f => f.MapFrom(e => e.Car.Model))
                .ForMember(d => d.DriverName, f => f.MapFrom(e => $"{e.Driver.Account.FirstName} {e.Driver.Account.LastName}"))
                .ForMember(d => d.DispatcherName, f => f.MapFrom(e => $"{e.Dispatcher.Account.FirstName} {e.Dispatcher.Account.LastName}"))
                .ForMember(d => d.MechanicName, f => f.MapFrom(e => $"{e.Mechanic.Account.FirstName} {e.Mechanic.Account.LastName}"))
                .ForMember(d => d.OperatorName, f => f.MapFrom(e => $"{e.Operator.Account.FirstName} {e.Operator.Account.LastName}"))
                .ForMember(d => d.InitialDistance, f => f.MapFrom(e => e.MechanicHandover.Distance))
                .ForMember(d => d.FinalDistance, f => f.MapFrom(e => e.MechanicAcceptance.Distance))
                .ForMember(d => d.PouredFuel, f => f.MapFrom(e => e.OperatorReview.OilAmount))
                .ForMember(d => d.CarMeduimFuelConsumption, f => f.MapFrom(e => e.Car.MeduimFuelConsumption));
                
            CreateMap<DispatcherReviewForCreateDto, DispatcherReview>();
            CreateMap<DispatcherReviewForUpdateDto, DispatcherReview>();
        }
    }
}
