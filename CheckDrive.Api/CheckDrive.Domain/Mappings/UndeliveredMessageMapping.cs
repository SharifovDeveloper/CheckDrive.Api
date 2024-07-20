using AutoMapper;
using CheckDrive.ApiContracts;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Domain.Mappings
{
    public class UndeliveredMessageMapping : Profile
    {
        public UndeliveredMessageMapping()
        {
            CreateMap<UndeliveredMessageForDto, UndeliveredMessage>()
                .ForMember(dest => dest.SendingMessageStatus, opt => opt.MapFrom(src => (SendingMessageStatus)src.SendingMessageStatus));

            CreateMap<UndeliveredMessage, UndeliveredMessageForDto>()
            .ForMember(dest => dest.SendingMessageStatus, opt => opt.MapFrom(src => (SendingMessageStatusForDto)src.SendingMessageStatus));
        }
    }
}
