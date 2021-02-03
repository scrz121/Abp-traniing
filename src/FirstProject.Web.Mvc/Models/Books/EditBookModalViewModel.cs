using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Application.Services.Dto;
using Abp.Localization;
using FirstProject.Books;
using FirstProject.Books.Dto;
using FirstProject.Categories.Dto;
using FirstProject.Publishers.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FirstProject.Web.Models.Books
{
    public class CheckBoxCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
    }

    public class EditBookModalViewModel
    {
        public ListResultDto<PublisherDto> publisherDtos { get; set; }

        public BookDto Book { get; set; }

        public ListResultDto<CategoryDto> categoryDtos { get; set; }

        public List<CheckBoxCategory> checkBoxCategories{ get; set; }

        public List<SelectListItem> GetBooksStateSelectListItems(ILocalizationManager localizationManager)
        {
            var list = new List<SelectListItem>();

            list.AddRange(Enum.GetValues(typeof(BookState))
                    .Cast<BookState>()
                    .Select(state =>
                        new SelectListItem
                        {
                            Text = localizationManager.GetString(FirstProjectConsts.LocalizationSourceName, $"BookState_{state}"),
                            Value = state.ToString(),
                            Selected = Book.State == state
                        })
            );

            return list;
        }

        public List<SelectListItem> GetPublisherSelectListItems()
        {
            var list = new List<SelectListItem>();

            list.AddRange(publisherDtos.Items
                    .Cast<PublisherDto>()
                    .Select(item =>
                        new SelectListItem
                        {
                            Text = item.PublisherName,
                            Value = item.Id.ToString(),
                            Selected = item.Id == Book.PublisherId
                        })
            );

            return list;
        }
    }
}
