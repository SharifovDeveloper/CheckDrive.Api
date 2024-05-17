using AutoMapper;
using CheckDrive.Domain.Entities;
using CheckDrive.ApiContracts.DoctorReview;


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
