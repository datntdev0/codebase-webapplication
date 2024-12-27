using Abp.AutoMapper;
using Abp.Modules;

namespace datntdev.MyCodebase
{
    [DependsOn(typeof(AbpAutoMapperModule))]
    public class MyCodebaseApplicationContractsModule : AbpModule
    {
        public override void Initialize()
        {
            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(typeof(MyCodebaseApplicationContractsModule).Assembly)
            );
        }
    }
}
