using Abp.MultiTenancy;
using Abp.Zero.Configuration;

namespace datntdev.MyCodebase.Authorization.Roles;

public static class AppRoleConfigurer
{
    public static void Configure(this IRoleManagementConfig roleManagementConfig)
    {
        // Static host roles

        roleManagementConfig.StaticRoles.Add(
            new StaticRoleDefinition(
                StaticRoleNames.Host.Admin,
                MultiTenancySides.Host
            )
        );

        // Static tenant roles

        roleManagementConfig.StaticRoles.Add(
            new StaticRoleDefinition(
                StaticRoleNames.Tenants.Admin,
                MultiTenancySides.Tenant
            )
        );
    }
}
