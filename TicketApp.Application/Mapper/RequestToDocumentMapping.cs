using AutoMapper;
using TicketApp.Application.Models.Request;
using TicketApp.Core.Entities;

namespace TicketApp.Application.Mapper
{
    public class RequestToDocumentMapping : Profile
    {
        public RequestToDocumentMapping()
        {
            CreateMap<UserRequestModel, UserDocument>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.Ordinal));
        }
    }
}