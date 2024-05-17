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
            CreateMap<DispatcherReview, DispatcherReviewDto>();
            CreateMap<DispatcherReviewForCreateDto, DispatcherReview>();
            CreateMap<DispatcherReviewForUpdateDto, DispatcherReview>();
        }
    }
}
