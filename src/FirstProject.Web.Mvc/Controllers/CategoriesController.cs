using FirstProject.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstProject.Web.Controllers
{
    public class CategoriesController : FirstProjectControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
