using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using datntdev.MyCodebase.Configuration;
using datntdev.MyCodebase.EntityFrameworkCore;
using datntdev.MyCodebase.Migrator.DependencyInjection;
using Castle.MicroKernel.Registration;
using Microsoft.Extensions.Configuration;

namespace datntdev.MyCodebase.Migrator;

[DependsOn(typeof(MyCodebaseEntityFrameworkModule))]
public class MyCodebaseMigratorModule : AbpModule
{
    private readonly IConfigurationRoot _appConfiguration;

    public MyCodebaseMigratorModule(MyCodebaseEntityFrameworkModule abpProjectNameEntityFrameworkModule)
    {
        abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

        _appConfiguration = AppConfigurations.Get(
            typeof(MyCodebaseMigratorModule).GetAssembly().GetDirectoryPathOrNull()
        );
    }

    public override void PreInitialize()
    {
        Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
            MyCodebaseConsts.ConnectionStringName
        );

        Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        Configuration.ReplaceService(
            typeof(IEventBus),
            () => IocManager.IocContainer.Register(
                Component.For<IEventBus>().Instance(NullEventBus.Instance)
            )
        );
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(MyCodebaseMigratorModule).GetAssembly());
        ServiceCollectionRegistrar.Register(IocManager);
    }
}
