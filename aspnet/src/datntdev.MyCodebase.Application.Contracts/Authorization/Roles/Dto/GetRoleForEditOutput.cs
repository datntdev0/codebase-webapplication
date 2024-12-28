﻿using System.Collections.Generic;

namespace datntdev.MyCodebase.Authorization.Roles.Dto;

public class GetRoleForEditOutput
{
    public RoleDto Role { get; set; }

    public List<FlatPermissionDto> Permissions { get; set; }

    public List<string> GrantedPermissionNames { get; set; }
}