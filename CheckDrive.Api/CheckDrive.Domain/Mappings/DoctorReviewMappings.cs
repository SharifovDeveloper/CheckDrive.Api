using AutoMapper;
using CheckDrive.ApiContracts.DoctorReview;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Domain.Mappings
{
    public class DoctorReviewMappings : Profile
    {
        public DoctorReviewMappings()
        {
            CreateMap<DoctorReviewDto, DoctorReview>();
            CreateMap<DoctorReview, DoctorReviewDto>()
                .ForMember(x => x.DriverName, f => f.MapFrom(e => $"{e.Driver.Account.FirstName} {e.Driver.Account.LastName}"))
                .ForMember(x => x.DoctorName, f => f.MapFrom(e => $"{e.Doctor.Account.FirstName} {e.Doctor.Account.LastName}"));
            CreateMap<DoctorReviewForCreateDto, DoctorReview>();
            CreateMap<DoctorReviewForUpdateDto, DoctorReview>();
        }
    }
}
