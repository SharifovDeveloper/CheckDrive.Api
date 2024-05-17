using AutoMapper;
using CheckDrive.Domain.Entities;
using CheckDrive.ApiContracts.Role;


namespace CheckDrive.Domain.Mappings
{
    public class RoleMappings : Profile
    {
        public RoleMappings()
        {
            CreateMap<RoleDto, Role>();
            CreateMap<Role, RoleDto>();
            CreateMap<RoleForCreateDto, Role>();
            CreateMap<RoleForUpdateDto, Role>();
        }
    }
}
