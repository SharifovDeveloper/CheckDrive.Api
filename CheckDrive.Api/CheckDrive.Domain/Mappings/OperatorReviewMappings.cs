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
            CreateMap<OperatorReview, OperatorReviewDto>();
            CreateMap<OperatorReviewForCreateDto, OperatorReview>();
            CreateMap<OperatorReviewForUpdateDto, OperatorReview>();
        }
    }
}
