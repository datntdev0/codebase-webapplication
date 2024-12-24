using Abp.Modules;
using Abp.Reflection.Extensions;
using datntdev.MyCodebase.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace datntdev.MyCodebase.Web.Host.Startup
{
    [DependsOn(
       typeof(MyCodebaseWebCoreModule))]
    public class MyCodebaseWebHostModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public MyCodebaseWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MyCodebaseWebHostModule).GetAssembly());
        }
    }
}
