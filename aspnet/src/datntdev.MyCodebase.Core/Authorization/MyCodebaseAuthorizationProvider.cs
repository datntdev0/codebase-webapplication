using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;
using datntdev.MyCodebase.Authorization.Permissions;

namespace datntdev.MyCodebase.Authorization;

public class MyCodebaseAuthorizationProvider : AuthorizationProvider
{
    public override void SetPermissions(IPermissionDefinitionContext context)
    {
        context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
        context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
        context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
    }

    private static LocalizableString L(string name)
    {
        return new(name, MyCodebaseConsts.LocalizationSourceName);
    }
}
