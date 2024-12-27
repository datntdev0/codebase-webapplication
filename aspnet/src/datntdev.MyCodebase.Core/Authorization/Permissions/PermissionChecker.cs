using Abp.Authorization;
using datntdev.MyCodebase.Authorization.Roles;
using datntdev.MyCodebase.Authorization.Users;

namespace datntdev.MyCodebase.Authorization.Permissions;

public class PermissionChecker(UserManager userManager)
    : PermissionChecker<Role, User>(userManager)
{
}
