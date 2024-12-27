using Abp.Authorization;
using Abp.Authorization.Roles;
using AutoMapper;
using System.Linq;

namespace datntdev.MyCodebase.Authorization.Roles.Dto;

public class RoleMapProfile : Profile
{
    public RoleMapProfile()
    {
        // Role and permission
        CreateMap<Permission, string>().ConvertUsing(r => r.Name);
        CreateMap<RolePermissionSetting, string>().ConvertUsing(r => r.Name);

        CreateMap<CreateRequestDto, Role>();

        CreateMap<RoleDto, Role>();

        CreateMap<Role, RoleDto>().ForMember(x => x.GrantedPermissions,
            opt => opt.MapFrom(x => x.Permissions.Where(p => p.IsGranted)));

        CreateMap<Role, RoleListDto>();
        CreateMap<Role, RoleEditDto>();
        CreateMap<Permission, FlatPermissionDto>();
    }
}