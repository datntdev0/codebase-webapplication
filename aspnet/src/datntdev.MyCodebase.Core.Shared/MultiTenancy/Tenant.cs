using Abp.MultiTenancy;
using datntdev.MyCodebase.Authorization.Users;

namespace datntdev.MyCodebase.MultiTenancy;

public class Tenant : AbpTenant<User>
{
    public Tenant()
    {
    }

    public Tenant(string tenancyName, string name) 
        : base(tenancyName, name)
    {
    }
}
