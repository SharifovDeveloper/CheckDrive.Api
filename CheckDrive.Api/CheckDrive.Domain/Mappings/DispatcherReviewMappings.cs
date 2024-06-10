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
                .ForMember(d => d.OperatorName, f => f.MapFrom(e => $"{e.Operator.Account.FirstName} {e.Operator.Account.LastName}"));
            CreateMap<DispatcherReviewForCreateDto, DispatcherReview>();
            CreateMap<DispatcherReviewForUpdateDto, DispatcherReview>();
        }
    }
}
