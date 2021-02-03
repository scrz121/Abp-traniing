using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Localization;
using FirstProject.Books;
using FirstProject.Books.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using FirstProject.Categories.Dto;
using FirstProject.Publishers.Dto;
using Abp.Application.Services.Dto;

namespace FirstProject.Web.Models.Books
{
    public class IndexViewModel
    {

        public ListResultDto<PublisherDto> publisherDtos { get; set; }
        public ListResultDto<CategoryDto> categoryDtos { get; set; }
        public ListResultDto<BookDto> bookDtos { get; set; }

        public IndexViewModel(ListResultDto<PublisherDto> _publisherDtos, ListResultDto<CategoryDto> _categoryDtos,ListResultDto<BookDto> _bookDtos)
        {
            publisherDtos = _publisherDtos;
            categoryDtos = _categoryDtos;
            bookDtos = _bookDtos;
        }

        public string GetBookLabel(BookDto book)
        {
            switch (book.State)
            {
                case BookState.New:
                    return "label-success";
                default:
                    return "label-default";
            }
        }

        public List<SelectListItem> GetBooksStateSelectListItems(ILocalizationManager localizationManager)
        {
            var list = new List<SelectListItem>();

            list.AddRange(Enum.GetValues(typeof(BookState))
                    .Cast<BookState>()
                    .Select(state =>
                        new SelectListItem
                        {
                            Text = localizationManager.GetString(FirstProjectConsts.LocalizationSourceName, $"BookState_{state}"),
                            Value = state.ToString()
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
                            Value = item.Id.ToString()
                        })
            );

            return list;
        }       
        public List<SelectListItem> GetCategorySelectListItems()
        {
            var list = new List<SelectListItem>();

            list.AddRange(categoryDtos.Items
                    .Cast<CategoryDto>()
                    .Select(item =>
                        new SelectListItem
                        {
                            Text = item.CategoryName,
                            Value = item.Id.ToString()
                        })
            );

            return list;
        }
        public List<SelectListItem> GetYearSelectListItems()
        {
            var list = new List<SelectListItem>();

            list.AddRange(bookDtos.Items
                    .Cast<BookDto>()
                    .Select(item =>
                        new SelectListItem
                        {
                            Text = item.PublishDate.ToString("yyyy"),
                            Value = item.PublishDate.ToString("yyyy")
                        })
            );
            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    if (list[i].Value == list[j].Value)
                    {
                        list.RemoveAt(j);
                        j--;
                    }
                }
            }
            return list;
        }
    }
}