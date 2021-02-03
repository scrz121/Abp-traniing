using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace FirstProject.Controllers
{
    public abstract class FirstProjectControllerBase: AbpController
    {
        protected FirstProjectControllerBase()
        {
            LocalizationSourceName = FirstProjectConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
