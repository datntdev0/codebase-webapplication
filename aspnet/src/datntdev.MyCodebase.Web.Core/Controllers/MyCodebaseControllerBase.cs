using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace datntdev.MyCodebase.Controllers
{
    public abstract class MyCodebaseControllerBase : AbpController
    {
        protected MyCodebaseControllerBase()
        {
            LocalizationSourceName = MyCodebaseConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
