using Abp.Authorization.Users;
using Abp.Extensions;
using System;

namespace datntdev.MyCodebase.Authorization.Users;

public class User : AbpUser<User>
{
    public const string DefaultPassword = "123qwe";

    public static string CreateRandomPassword()
    {
        return Guid.NewGuid().ToString("N").Truncate(16);
    }

    public static User CreateTenantAdminUser(int tenantId, string emailAddress)
    {
        var user = new User
        {
            TenantId = tenantId,
            UserName = AdminUserName,
            Name = AdminUserName,
            Surname = AdminUserName,
            EmailAddress = emailAddress,
            Roles = [],
        };

        user.SetNormalizedNames();

        return user;
    }
}
