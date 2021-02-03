using Abp.Application.Services;
using Abp.Application.Services.Dto;
using FirstProject.Books.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FirstProject.Books
{
    public interface IBookAppService : IApplicationService
    {
        Task<ListResultDto<BookDto>> GetAll(BookFilter filter);
        Task Create(CreateBookDto input);
        Task<BookDto> GetAsync(int bookId);
        Task Update(BookDto bookDto);
    }
}
