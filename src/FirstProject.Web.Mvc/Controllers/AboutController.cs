using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using FirstProject.Controllers;

namespace FirstProject.Web.Controllers
{
    [AbpMvcAuthorize]
    public class AboutController : FirstProjectControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}
