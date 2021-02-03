using FirstProject.Controllers;
using FirstProject.Roles;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using FirstProject.Books;
using FirstProject.Categories;
using FirstProject.Publishers;
using FirstProject.BookCategories;
using System.Threading.Tasks;
using FirstProject.Books.Dto;
using FirstProject.Web.Models.Books;

namespace FirstProject.Web.Controllers
{
    public class BooksController : FirstProjectControllerBase
    {
        private readonly IBookAppService _bookAppService;
        private readonly IPublisherAppService _publisherAppService;
        private readonly ICategoryAppService _categoryAppService;
        private readonly IBookCategoryAppService _bookCategoryAppService;

        public BooksController(IBookAppService bookAppService, IPublisherAppService publisherAppService, ICategoryAppService categoryAppService, IBookCategoryAppService bookCategoryAppService)
        {
            _bookCategoryAppService = bookCategoryAppService;
            _bookAppService = bookAppService;
            _publisherAppService = publisherAppService;
            _categoryAppService = categoryAppService;
        }

        public async Task<ActionResult> Index()
        {
            var publishers = await _publisherAppService.GetAll(null);
            var categories = await _categoryAppService.GetAll(null);
            var books = await _bookAppService.GetAll(null);
            var model = new IndexViewModel(publishers,categories,books);

            return View(model);
        }

        public async  Task<ActionResult> EditModal(int bookId)
        {
            var book = await _bookAppService.GetAsync(bookId);
            var publishers = await _publisherAppService.GetAll(null);
            var allCategories = await _categoryAppService.GetAll(null);
            var bookCategories = _bookCategoryAppService.GetAllByBookId(bookId);
            List<CheckBoxCategory> lst = new List<CheckBoxCategory>();
            foreach(var c in allCategories.Items)
            {
                CheckBoxCategory checkBoxCategory = new CheckBoxCategory();
                checkBoxCategory.Id = c.Id;
                checkBoxCategory.Name = c.CategoryName;
                checkBoxCategory.Selected = false;
                foreach(var bc in bookCategories.Items)
                {
                    if (c.Id == bc.CategoryId) checkBoxCategory.Selected = true;
                }
                lst.Add(checkBoxCategory);
            }

            var model = new EditBookModalViewModel
            {
                Book = book,publisherDtos = publishers,checkBoxCategories = lst
            };
            return PartialView("_EditModal", model);
        }
    }
}
