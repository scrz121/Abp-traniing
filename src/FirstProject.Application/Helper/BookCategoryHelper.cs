using Abp.Domain.Repositories;
using System.Threading.Tasks;
using System;
using FirstProject.BookCategories.Dto;
using FirstProject.BookCategories;
using Abp.Dependency;
using System.Collections.Generic;
using FirstProject.Books.Dto;
using System.Linq;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.Helper
{
    public class BookCategoryHelper: FirstProjectAppServiceBase
    {
        private readonly IRepository<BookCategory> _bookCategoryRepository;

        public BookCategoryHelper(IRepository<BookCategory> bookCategoryRepository)
        {
            _bookCategoryRepository = bookCategoryRepository;
        }

        public async Task Create(BookCategoryDto input)
        {
            try
            {
                var entity = ObjectMapper.Map<BookCategory>(input);
                await _bookCategoryRepository.InsertAsync(entity);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task Delete(BookCategoryDto dto)
        {
            try
            {
                await _bookCategoryRepository.DeleteAsync(x => x.BookId == dto.BookId && x.CategoryId == dto.CategoryId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task DeleteByBookId(int bookId)
        {
            try
            {
                await _bookCategoryRepository.DeleteAsync(x => x.BookId == bookId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<List<BookCategoryDto>> GetByBookId(int bookId)
        {
            try
            {
                var dtos = await _bookCategoryRepository.GetAllListAsync(x => x.BookId == bookId);
                return ObjectMapper.Map<List<BookCategoryDto>>(dtos);
            }
            catch (Exception e)
            {
                throw e;
            }
        } 

        public async Task<List<BookDto>> GetBookByCategoryId(int categoryId)
        {
            var allBooks = await _bookCategoryRepository
                .GetAll().Where(x=>x.CategoryId == categoryId).Include(x=>x.Book)
                .ToListAsync();
            if(allBooks == null || allBooks.Count <= 0)
            {
                return null;
            }
            List<BookDto> bookDtos = new List<BookDto>();
            foreach(var b in allBooks)
            {
                bookDtos.Add(ObjectMapper.Map<BookDto>(b.Book));
            }
            return bookDtos;
        }
    }
}
