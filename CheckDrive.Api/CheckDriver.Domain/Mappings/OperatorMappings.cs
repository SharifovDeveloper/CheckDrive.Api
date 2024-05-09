using AutoMapper;
using CheckDrive.Domain.DTOs.Operator;
using CheckDriver.Domain.Entities;

namespace CheckDrive.Domain.Mappings
{
    public class OperatorMappings : Profile
    {
        public OperatorMappings() 
        {
            CreateMap<OperatorDto, Operator>();
            CreateMap<Operator, OperatorDto>();
            CreateMap<OperatorForCreateDto, Operator>();
            CreateMap<OperatorForCreateDto, Operator>();
        }
    }
}
