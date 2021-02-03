using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using FirstProject.Configuration;

namespace FirstProject.Web.Host.Startup
{
    [DependsOn(
       typeof(FirstProjectWebCoreModule))]
    public class FirstProjectWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public FirstProjectWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(FirstProjectWebHostModule).GetAssembly());
        }
    }
}
