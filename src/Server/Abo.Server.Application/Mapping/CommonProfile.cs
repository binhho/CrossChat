using Abo.Server.Application.DataTransferObjects;
using Abo.Server.Application.DataTransferObjects.Messages;
using Abo.Server.Domain.Entities;
using AutoMapper;

namespace Abo.Server.Application.Mapping
{
    public class CommonProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<User, UserDto>();
                //.ForMember(dto => dto.IsInChat, opt => opt.Ignore());

            Mapper.CreateMap<PublicMessage, PublicMessageDto>();
                //.ForMember(dto => dto.AuthorName, opt => opt.ResolveUsing(entity => entity.Author.Name))

            base.Configure();
        }
    }
}
