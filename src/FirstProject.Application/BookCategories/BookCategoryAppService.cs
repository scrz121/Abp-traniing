using Abp.Domain.Repositories;
using System.Threading.Tasks;
using System;
using Abp.UI;
using Microsoft.AspNetCore.Hosting;
using FirstProject.BookCategories.Dto;
using Abp.Application.Services.Dto;
using System.Linq;
using System.Collections.Generic;
using Abp.Authorization;
using FirstProject.Authorization;

namespace FirstProject.BookCategories
{
    public class Filter
    {
        public int BookId { get; set; }
        public int CategoryId { get; set; }
    }
    [AbpAuthorize]
    public class BookCategoryAppService : FirstProjectAppServiceBase, IBookCategoryAppService
    {
        private readonly IRepository<BookCategory> _bookCategoryRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public BookCategoryAppService(IRepository<BookCategory> bookCategoryRepository, IWebHostEnvironment webHostEnvironment)
        {
            _bookCategoryRepository = bookCategoryRepository;
            _hostingEnvironment = webHostEnvironment;
        }

        [AbpAuthorize(PermissionNames.Pages_BookCategory)]
        public async Task Create(BookCategoryDto input)
        {
            try
            {
                var entity = ObjectMapper.Map<BookCategory>(input);
                await _bookCategoryRepository.InsertAsync(entity);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        [AbpAuthorize(PermissionNames.Pages_BookCategory_Read)]
        public ListResultDto<BookCategoryDto> GetAllByBookId(int bookId)
        {
            var allBooks =  _bookCategoryRepository.GetAll().Where(x => x.BookId == bookId).ToList();

            int totalCount = allBooks.Count;

            var result = new PagedResultDto<BookCategoryDto>(totalCount, ObjectMapper.Map<List<BookCategoryDto>>(allBooks)
            );

            return result;
        }

        [AbpAuthorize(PermissionNames.Pages_BookCategory_Read)]
        public ListResultDto<BookCategoryDto> GetAll()
        {
            var allBooks = _bookCategoryRepository.GetAll().ToList();

            int totalCount = allBooks.Count;

            var result = new PagedResultDto<BookCategoryDto>(totalCount, ObjectMapper.Map<List<BookCategoryDto>>(allBooks)
            );

            return result;
        }
    }
}
