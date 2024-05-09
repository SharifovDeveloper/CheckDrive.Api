using AutoMapper;
using CheckDrive.Domain.DTOs.OperatorReview;
using CheckDriver.Domain.Entities;

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
