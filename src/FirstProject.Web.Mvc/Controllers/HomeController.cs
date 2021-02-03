using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using FirstProject.Controllers;

namespace FirstProject.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : FirstProjectControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
