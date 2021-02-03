using Abp.AspNetCore.Mvc.ViewComponents;

namespace FirstProject.Web.Views
{
    public abstract class FirstProjectViewComponent : AbpViewComponent
    {
        protected FirstProjectViewComponent()
        {
            LocalizationSourceName = FirstProjectConsts.LocalizationSourceName;
        }
    }
}
