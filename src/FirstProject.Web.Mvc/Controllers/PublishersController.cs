using FirstProject.Books;
using FirstProject.Categories;
using FirstProject.Controllers;
using FirstProject.Publishers;
using FirstProject.Web.Models.Publishers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstProject.Web.Controllers
{
    public class PublishersController : FirstProjectControllerBase
    {
        private readonly IBookAppService _bookAppService;
        private readonly ICategoryAppService _categoryAppService;
        private readonly IPublisherAppService _publisherAppService;

        public PublishersController(IBookAppService bookAppService, ICategoryAppService categoryAppService,IPublisherAppService publisherAppService)
        {
            _bookAppService = bookAppService;
            _categoryAppService = categoryAppService;
            _publisherAppService = publisherAppService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(int id)
        {
            var publisher = await _publisherAppService.GetById(id);
            var categories = await _categoryAppService.GetAll(null);
            var books = await _bookAppService.GetAll(null);
            
            var model = new IndexViewModel(categories,books);
            ViewBag.Publisher = publisher;
            return View(model);
        }
    }
}
