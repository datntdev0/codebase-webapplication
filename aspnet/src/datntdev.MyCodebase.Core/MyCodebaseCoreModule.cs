using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero;
using Abp.Zero.Configuration;
using datntdev.MyCodebase.Authorization.Roles;
using datntdev.MyCodebase.Authorization.Users;
using datntdev.MyCodebase.Localization;
using datntdev.MyCodebase.MultiTenancy;

namespace datntdev.MyCodebase;

[DependsOn(typeof(AbpZeroCoreModule)
    , typeof(MyCodebaseCoreSharedModule)
)]
public class MyCodebaseCoreModule : AbpModule
{
    public override void PreInitialize()
    {
        // Declare entity types
        Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
        Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
        Configuration.Modules.Zero().EntityTypes.User = typeof(User);
        Configuration.Modules.Zero().RoleManagement.Configure();

        // Configure localization sources
        Configuration.Localization.Configure();
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(MyCodebaseCoreModule).GetAssembly());
    }
}
