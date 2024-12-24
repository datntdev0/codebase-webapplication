using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using datntdev.MyCodebase.Authorization;

namespace datntdev.MyCodebase;

[DependsOn(
    typeof(MyCodebaseCoreModule),
    typeof(AbpAutoMapperModule))]
public class MyCodebaseApplicationModule : AbpModule
{
    public override void PreInitialize()
    {
        Configuration.Authorization.Providers.Add<MyCodebaseAuthorizationProvider>();
    }

    public override void Initialize()
    {
        var thisAssembly = typeof(MyCodebaseApplicationModule).GetAssembly();

        IocManager.RegisterAssemblyByConvention(thisAssembly);

        Configuration.Modules.AbpAutoMapper().Configurators.Add(
            // Scan the assembly for classes which inherit from AutoMapper.Profile
            cfg => cfg.AddMaps(thisAssembly)
        );
    }
}
