using AutoMapper;
using DbApi;


namespace DbApi.Mapper
{
    public class MapperProfile: Profile
    {
        public MapperProfile()
        {

            CreateMap<UserModel, User>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(y => y.Id))
                .ForMember(dest => dest.Name, opts => opts.MapFrom(y => y.Name))
                .ForMember(dest => dest.Email, opts => opts.MapFrom(y => y.Email))
                .ForMember(dest => dest.Password, opts => opts.MapFrom(y => y.Password))
                .ForMember(dest => dest.Salt, opts => opts.Ignore())
                .ForMember(dest => dest.RoleId, opts => opts.MapFrom(y => y.Role))
                .ReverseMap();

            CreateMap<MessageModel, Message>()
                .ForMember(dest => dest.MessageId, opts => opts.MapFrom(y => y.MessageId))
                .ForPath(dest => dest.FromUser.Email, opts => opts.MapFrom(y => y.FromUser))
                .ForPath(dest => dest.ToUser.Email, opts => opts.MapFrom(y => y.ToUser))
                .ForMember(dest => dest.IsRead, opts => opts.MapFrom(y => y.IsRead))
                .ForMember(dest => dest.Date, opts => opts.MapFrom(y => y.Date))
                .ForMember(dest => dest.Text, opts => opts.MapFrom(y => y.Text)) 
                .ReverseMap();

        }
    }
}
