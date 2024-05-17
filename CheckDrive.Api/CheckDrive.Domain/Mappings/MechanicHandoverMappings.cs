using AutoMapper;
using CheckDrive.Domain.Entities;
using CheckDrive.ApiContracts.MechanicHandover;


namespace CheckDrive.Domain.Mappings
{
    public class MechanicHandoverMappings : Profile
    {
        public MechanicHandoverMappings()
        {
            CreateMap<MechanicHandoverDto, MechanicHandover>();
            CreateMap<MechanicHandover, MechanicHandoverDto>();
            CreateMap<MechanicHandoverForCreateDto, MechanicHandover>();
            CreateMap<MechanicHandoverForUpdateDto, MechanicHandover>();
        }
    }
}