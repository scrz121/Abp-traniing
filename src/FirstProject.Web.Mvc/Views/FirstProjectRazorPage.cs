using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace FirstProject.Web.Views
{
    public abstract class FirstProjectRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected FirstProjectRazorPage()
        {
            LocalizationSourceName = FirstProjectConsts.LocalizationSourceName;
        }
    }
}
