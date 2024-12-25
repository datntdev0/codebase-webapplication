using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using datntdev.MyCodebase.Authorization.Users;
using datntdev.MyCodebase.MultiTenancy;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System;

namespace datntdev.MyCodebase.Controllers
{
    public abstract class MyCodebaseControllerBase : AbpController
    {
        public TenantManager TenantManager { get; set; }

        public UserManager UserManager { get; set; }

        protected MyCodebaseControllerBase()
        {
            LocalizationSourceName = MyCodebaseConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        protected virtual async Task<User> GetCurrentUserAsync()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            return user;
        }

        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        }
    }
}
