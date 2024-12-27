using Abp.Modules;
using datntdev.MyCodebase.Authorization;

namespace datntdev.MyCodebase;

[DependsOn(typeof(MyCodebaseCoreModule)
    , typeof(MyCodebaseApplicationContractsModule)
)]
public class MyCodebaseApplicationModule : AbpModule
{
    public override void PreInitialize()
    {
        Configuration.Authorization.Providers.Add<MyCodebaseAuthorizationProvider>();
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(
            typeof(MyCodebaseApplicationModule).Assembly
        );
    }
}
