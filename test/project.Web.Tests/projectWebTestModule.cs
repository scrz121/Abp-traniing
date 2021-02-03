using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using project.EntityFrameworkCore;
using project.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace project.Web.Tests
{
    [DependsOn(
        typeof(projectWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class projectWebTestModule : AbpModule
    {
        public projectWebTestModule(projectEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(projectWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(projectWebMvcModule).Assembly);
        }
    }
}