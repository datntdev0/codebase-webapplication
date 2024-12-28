using Abp.Modules;
using Abp.Reflection.Extensions;

namespace datntdev.MyCodebase.Web.Host.Startup
{
    [DependsOn(typeof(MyCodebaseWebCoreModule))]
    public class MyCodebaseWebHostModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MyCodebaseWebHostModule).GetAssembly());
        }
    }
}
