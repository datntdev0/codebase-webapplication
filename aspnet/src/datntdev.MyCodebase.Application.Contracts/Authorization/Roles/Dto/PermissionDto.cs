using Abp.Authorization;
using Abp.AutoMapper;

namespace datntdev.MyCodebase.Authorization.Roles.Dto;

[AutoMapFrom(typeof(Permission))]
public class PermissionDto
{
    public string Name { get; set; }

    public string DisplayName { get; set; }

    public string Description { get; set; }
}
