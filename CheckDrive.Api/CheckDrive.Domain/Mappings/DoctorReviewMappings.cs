using AutoMapper;
using CheckDrive.Domain.DTOs.DoctorReview;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Domain.Mappings
{
    public class DoctorReviewMappings : Profile
    {
        public DoctorReviewMappings() 
        {
            CreateMap<DoctorReviewDto, DoctorReview>();
            CreateMap<DoctorReview, DoctorReviewDto>();
            CreateMap<DoctorReviewForCreateDto, DoctorReview>();
            CreateMap<DoctorReviewForUpdateDto, DoctorReview>();
        }
    }
}
