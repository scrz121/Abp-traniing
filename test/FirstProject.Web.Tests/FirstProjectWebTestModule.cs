using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using FirstProject.EntityFrameworkCore;
using FirstProject.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace FirstProject.Web.Tests
{
    [DependsOn(
        typeof(FirstProjectWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class FirstProjectWebTestModule : AbpModule
    {
        public FirstProjectWebTestModule(FirstProjectEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(FirstProjectWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(FirstProjectWebMvcModule).Assembly);
        }
    }
}