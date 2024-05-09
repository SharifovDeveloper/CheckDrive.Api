using AutoMapper;
using CheckDrive.Domain.DTOs.Role;
using CheckDriver.Domain.Entities;

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
