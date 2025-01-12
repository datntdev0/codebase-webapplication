using Abp.Modules;

namespace datntdev.MyCodebase;

public class MyCodebaseCoreSharedModule : AbpModule
{
    public override void PreInitialize()
    {
        Configuration.Auditing.IsEnabledForAnonymousUsers = true;
        
        // Enable this line to create a multi-tenant application.
        Configuration.MultiTenancy.IsEnabled = MyCodebaseConsts.MultiTenancyEnabled;
        Configuration.MultiTenancy.TenantIdResolveKey = "Abp.TenantId";
    }
}
