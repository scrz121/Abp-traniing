using Abp.Domain.Repositories;
using System.Threading.Tasks;
using System;
using FirstProject.Books;
using FirstProject.Books.Dto;
using Abp.Dependency;
using System.Collections.Generic;
using FirstProject.Books.Dto;
using System.Linq;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.Helper
{
    public class BookHelper: FirstProjectAppServiceBase
    {
        private readonly IRepository<Book> _bookRepository;

        public BookHelper(IRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<List<BookDto>> GetByPublisherId(int id)
        {
            try
            {
                var dtos = await _bookRepository.GetAllListAsync(x => x.PublisherId == id);
                return ObjectMapper.Map<List<BookDto>>(dtos);
            }
            catch (Exception e)
            {
                throw e;
            }
        } 
    }
}
