using Abp.Application.Services;
using Abp.Application.Services.Dto;
using FirstProject.BookCategories.Dto;
using FirstProject.Books.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FirstProject.BookCategories
{
    public interface IBookCategoryAppService : IApplicationService
    {
        Task Create(BookCategoryDto input);

        public ListResultDto<BookCategoryDto> GetAllByBookId(int bookId);
    }
}
