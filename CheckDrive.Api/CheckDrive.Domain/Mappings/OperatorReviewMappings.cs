using AutoMapper;
using CheckDrive.Domain.Entities;
using CheckDrive.ApiContracts.OperatorReview;


namespace CheckDrive.Domain.Mappings
{
    public class OperatorReviewMappings : Profile
    {
        public OperatorReviewMappings()
        {
            CreateMap<OperatorReviewDto, OperatorReview>();
            CreateMap<OperatorReview, OperatorReviewDto>()
                .ForMember(x => x.DriverName, f => f.MapFrom(e => $"{e.Driver.Account.FirstName} {e.Driver.Account.LastName}"))
                .ForMember(x => x.OperatorName, f => f.MapFrom(e => $"{e.Operator.Account.FirstName} {e.Operator.Account.LastName}"));
            CreateMap<OperatorReviewForCreateDto, OperatorReview>();
            CreateMap<OperatorReviewForUpdateDto, OperatorReview>();
        }
    }
}
