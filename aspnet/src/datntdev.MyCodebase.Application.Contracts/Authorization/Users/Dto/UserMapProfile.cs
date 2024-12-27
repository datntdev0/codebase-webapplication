using AutoMapper;

namespace datntdev.MyCodebase.Authorization.Users.Dto;

public class UserMapProfile : Profile
{
    public UserMapProfile()
    {
        CreateMap<UserDto, User>();
        CreateMap<UserDto, User>()
            .ForMember(x => x.Roles, opt => opt.Ignore())
            .ForMember(x => x.CreationTime, opt => opt.Ignore());

        CreateMap<CreateRequestDto, User>();
        CreateMap<CreateRequestDto, User>().ForMember(x => x.Roles, opt => opt.Ignore());
    }
}
