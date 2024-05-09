using AutoMapper;
using CheckDrive.Domain.DTOs.DispatcherReview;
using CheckDriver.Domain.Entities;

namespace CheckDrive.Domain.Mappings
{
    public class DispatcherReviewMappings : Profile
    {
        public DispatcherReviewMappings() 
        {
            CreateMap<DispatcherReviewDto, DispatcherReview>();
            CreateMap<DispatcherReview, DispatcherReviewDto>();
            CreateMap<DispatcherReviewForCreateDto, DispatcherReview>();
            CreateMap<DispatcherReviewForUpdateDto, DispatcherReview>();
        }
    }
}
