using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Dependency;
using datntdev.MyCodebase.Configuration;
using datntdev.MyCodebase.EntityFrameworkCore;
using datntdev.MyCodebase.Identity;
using Castle.MicroKernel.Registration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using datntdev.MyCodebase.Web.Host.Startup;

namespace datntdev.MyCodebase.Web.Tests;

public class Startup
{
    private readonly IConfigurationRoot _appConfiguration;

    public Startup(IWebHostEnvironment env)
    {
        _appConfiguration = env.GetAppConfiguration();
    }

    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
        services.AddEntityFrameworkInMemoryDatabase();

        services.AddMvc(options =>
        {
            options.Conventions.Add(new MyCodebaseServiceConvention(services));
        });

        IdentityRegistrar.Register(services);
        AuthConfigurer.Configure(services, _appConfiguration);
        
        // Configure Abp and Dependency Injection
        return services.AddAbp<MyCodebaseWebTestModule>(options => options.SetupTest());
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        UseInMemoryDb(app.ApplicationServices);

        app.UseAbp(); //Initializes ABP framework.

        app.UseExceptionHandler("/Error");

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
        });
    }

    private void UseInMemoryDb(IServiceProvider serviceProvider)
    {
        var builder = new DbContextOptionsBuilder<MyCodebaseDbContext>();
        builder.UseInMemoryDatabase(Guid.NewGuid().ToString()).UseInternalServiceProvider(serviceProvider);
        var options = builder.Options;

        var iocManager = serviceProvider.GetRequiredService<IIocManager>();
        iocManager.IocContainer
            .Register(
                Component.For<DbContextOptions<MyCodebaseDbContext>>()
                    .Instance(options)
                    .LifestyleSingleton()
            );
    }
}