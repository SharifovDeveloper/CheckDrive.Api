using AutoMapper;
using CheckDrive.Domain.DTOs.Doctor;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Domain.Mappings
{
    public class DoctorMappings : Profile
    {
        public DoctorMappings() 
        {
            CreateMap<DoctorDto, Doctor>();
            CreateMap<Doctor, DoctorDto>();
            CreateMap<DoctorForCreateDto, Doctor>();
            CreateMap<DoctorForUpdateDto, Doctor>();
        }
    }
}
