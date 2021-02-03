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

namespace FirstProject.Web.Models.Publishers
{
    public class IndexViewModel
    {
        public ListResultDto<CategoryDto> categoryDtos { get; set; }
        public ListResultDto<BookDto> bookDtos{ get; set; }

        public IndexViewModel(ListResultDto<CategoryDto> _categoryDtos, ListResultDto<BookDto> _bookDtos)
        {
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
            for(int i = 0; i < list.Count-1; i++)
            {
                for(int j = i + 1; j < list.Count; j++)
                {
                    if(list[i].Value == list[j].Value)
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