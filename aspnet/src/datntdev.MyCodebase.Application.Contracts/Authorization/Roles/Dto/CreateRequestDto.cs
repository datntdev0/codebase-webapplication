using Abp.Authorization.Roles;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace datntdev.MyCodebase.Authorization.Roles.Dto;

public class CreateRequestDto
{
    [Required]
    [StringLength(AbpRoleBase.MaxNameLength)]
    public string Name { get; set; }

    [Required]
    [StringLength(AbpRoleBase.MaxDisplayNameLength)]
    public string DisplayName { get; set; }

    public string NormalizedName { get; set; }

    [StringLength(Role.MaxDescriptionLength)]
    public string Description { get; set; }

    public List<string> GrantedPermissions { get; set; }

    public CreateRequestDto()
    {
        GrantedPermissions = new List<string>();
    }
}