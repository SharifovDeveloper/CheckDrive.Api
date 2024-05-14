using AutoMapper;
using CheckDrive.Domain.DTOs.Mechanic;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Domain.Mappings
{
    public class MechanicMappings : Profile
    {
        public MechanicMappings() 
        {
            CreateMap<MechanicDto, Mechanic>();
            CreateMap<Mechanic, MechanicDto>();
            CreateMap<MechanicForCreateDto, Mechanic>();
            CreateMap<MechanicForUpdateDto, Mechanic>();
        }
    }
}
