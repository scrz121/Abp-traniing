using System.Threading.Tasks;
using project.Models.TokenAuth;
using project.Web.Controllers;
using Shouldly;
using Xunit;

namespace project.Web.Tests.Controllers
{
    public class HomeController_Tests: projectWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}