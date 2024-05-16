using AutoMapper;
using CheckDrive.Domain.DTOs.Account;
using CheckDrive.Domain.DTOs.Doctor;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Domain.Mappings
{
    public class DoctorMappings : Profile
    {
        public DoctorMappings()
        {
            CreateMap<Doctor, DoctorDto>()
                .ForMember(x => x.FirstName, e => e.MapFrom(f => f.Account.FirstName))
                .ForMember(x => x.LastName, e => e.MapFrom(f => f.Account.LastName))
                .ForMember(x => x.Login, e => e.MapFrom(f => f.Account.Login))
                .ForMember(x => x.Password, e => e.MapFrom(f => f.Account.Password))
                .ForMember(x => x.Bithdate, e => e.MapFrom(f => f.Account.Bithdate))
                .ForMember(x => x.PhoneNumber, e => e.MapFrom(f => f.Account.PhoneNumber));
            CreateMap<DoctorForCreateDto, Account>()
                .ForMember(x => x.RoleId, e => e.MapFrom(RoleId => 3));
            CreateMap<Account, DoctorDto>();
            CreateMap<AccountForUpdateDto, Doctor>();
        }
    }
}
