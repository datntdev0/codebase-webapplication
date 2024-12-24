using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using datntdev.MyCodebase.EntityFrameworkCore;
using datntdev.MyCodebase.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace datntdev.MyCodebase.Web.Tests;

[DependsOn(
    typeof(MyCodebaseWebMvcModule),
    typeof(AbpAspNetCoreTestBaseModule)
)]
public class MyCodebaseWebTestModule : AbpModule
{
    public MyCodebaseWebTestModule(MyCodebaseEntityFrameworkModule abpProjectNameEntityFrameworkModule)
    {
        abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
    }

    public override void PreInitialize()
    {
        Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(MyCodebaseWebTestModule).GetAssembly());
    }

    public override void PostInitialize()
    {
        IocManager.Resolve<ApplicationPartManager>()
            .AddApplicationPartsIfNotAddedBefore(typeof(MyCodebaseWebMvcModule).Assembly);
    }
}