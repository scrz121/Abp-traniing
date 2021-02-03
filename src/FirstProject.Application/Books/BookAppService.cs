using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using FirstProject.Books.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using Abp.Linq.Extensions;
using Abp.Collections.Extensions;
using System.Linq;
using Abp.UI;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using FirstProject.BookCategories;
using FirstProject.BookCategories.Dto;
using FirstProject.Helper;
using Abp.Domain.Uow;
using Abp.Authorization;
using FirstProject.Authorization;
using System.ComponentModel.DataAnnotations;

namespace FirstProject.Books
{
    public class BookFilter
    {
        public int SkipCount { get; set; }
        public int MaxResultCount { get; set; }
        public string Keyword { get; set; }
        public BookState? State { get; set; }
        public int? Category { get; set; }
        public int? Year { get; set; }
        public int? Publisher { get; set; }
    }   
    public class BookPublisherFilter
    {
        public int? Year { get; set; }
        public int? CategoryId { get; set; }
        [Required]
        public int PublisherId { get; set; }
    }

    [AbpAuthorize]
    public class BookAppService : FirstProjectAppServiceBase, IBookAppService
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly BookCategoryHelper _bookCategoryHelper;
        private readonly IBookCategoryAppService _bookCategoryAppService;

        public BookAppService(IBookCategoryAppService bookCategoryAppService, IRepository<Book> bookRepository, BookCategoryHelper bookCategoryHelper, IWebHostEnvironment webHostEnvironment)
        {
            _bookCategoryAppService = bookCategoryAppService;
            _bookRepository = bookRepository;
            _bookCategoryHelper = bookCategoryHelper;
            _hostingEnvironment = webHostEnvironment;
        }

        [AbpAuthorize(PermissionNames.Pages_Books_Create)]
        [UnitOfWork]
        public async Task Create([FromForm]CreateBookDto input)
        {
            string path = "";
            try
            {
                if(input.Category == null || input.Category.Count <= 0)
                {
                    throw new Exception("Category is null");
                }

                var find = _bookRepository.GetAll().ToList().WhereIf(!string.IsNullOrEmpty(input.Title), x => x.Title.Trim().ToLower() == input.Title.Trim().ToLower()).ToList();

                if (find.Count > 0) throw new Exception("Title exist");

                var temp = input.CoverImageFile.FileName.Split('/');
                var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + string.Join("",temp);
                path = Path.Combine(Directory.GetCurrentDirectory(),
                    _hostingEnvironment.WebRootPath, "img", "uploads", "books", fileName
                    );

                input.CoverImage= $"/img/uploads/books/{fileName}";
                var book = await _bookRepository.InsertAsync(ObjectMapper.Map<Book>(input));
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await input.CoverImageFile.CopyToAsync(stream);
                }
                await UnitOfWorkManager.Current.SaveChangesAsync();

                List<Task> TaskList = new List<Task>();

                foreach (var item in input.Category)
                {
                    BookCategoryDto dto = new BookCategoryDto { BookId = book.Id, CategoryId = item };
                    Task task = new Task(() => _bookCategoryHelper.Create(dto));
                    task.Start();
                    TaskList.Add(task);
                }
                await Task.WhenAll(TaskList.ToArray());
            }
            catch (Exception e)
            {
                if (!string.IsNullOrEmpty(path) && File.Exists(path)){
                    File.Delete(path);
                }
                throw new UserFriendlyException(e.Message);
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Books)]
        public async Task Delete(DeleteBookDto bookDto)
        {
            string oldPath = "";
            try
            {
                int id = bookDto.BookId;
                var bookCheck = await _bookRepository.GetAll().AsNoTracking().Where(x=>x.Id == bookDto.BookId).ToListAsync();
                if (bookCheck.Count == 0)
                {
                    throw new Exception("Not exist");
                }
                var book = bookCheck[0];
                if (!string.IsNullOrEmpty(book.CoverImage))
                {
                    var oldFileName = book.CoverImage.Split('/').Last();
                    oldPath = Path.Combine(Directory.GetCurrentDirectory(),
                    _hostingEnvironment.WebRootPath, "img", "uploads", "books", oldFileName);
                }
                await _bookRepository.DeleteAsync(id);
                await _bookCategoryHelper.DeleteByBookId(id);
                if (File.Exists(oldPath))
                {
                    File.Delete(oldPath);
                }
            }
            catch(Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Books)]
        [UnitOfWork]
        public async Task Update([FromForm]BookDto bookDto)
        {
            string path = "";
            string oldPath = "";
            try
            {
                if (bookDto.Category == null || bookDto.Category.Count <= 0)
                {
                    throw new Exception("Category is null");
                }
                var bookCheck = await _bookRepository.FirstOrDefaultAsync(bookDto.Id);
                if (bookCheck == null)
                {
                    throw new Exception("Not exist");
                }
                var find = _bookRepository.GetAll().ToList().WhereIf(!string.IsNullOrEmpty(bookDto.Title), x => x.Title.Trim().ToLower() == bookDto.Title.Trim().ToLower()).ToList();

                if (bookDto.Title != bookCheck.Title)
                {
                    if (find.Count > 0) throw new Exception("Title exist");
                }
                else
                {
                     if(find.Count > 1) throw new Exception("Title exist");
                }

                bookDto.CreatorUserId = bookCheck.CreatorUserId;

                if (bookDto.CoverImageFile != null)
                {
                    if (!string.IsNullOrEmpty(bookDto.CoverImage))
                    {
                        var oldFileName = bookDto.CoverImage.Split('/').Last();
                        oldPath = Path.Combine(Directory.GetCurrentDirectory(),
                        _hostingEnvironment.WebRootPath, "img", "uploads", "books", oldFileName);
                    }
                    var temp = bookDto.CoverImageFile.FileName.Split('/');
                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + string.Join("", temp);
                    path = Path.Combine(Directory.GetCurrentDirectory(),
                        _hostingEnvironment.WebRootPath, "img", "uploads", "books", fileName
                        );
                    bookDto.CoverImage = $"/img/uploads/books/{fileName}";
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await bookDto.CoverImageFile.CopyToAsync(stream);
                    }
                }

                ObjectMapper.Map(bookDto, bookCheck);

                var lstCategory = await _bookCategoryHelper.GetByBookId(bookCheck.Id);

                List<Task> TaskList = new List<Task>();

                foreach (var item in bookDto.Category)
                {
                    int pos = lstCategory.FindIndex(x => x.CategoryId == item);
                    if (pos >= 0)
                    {
                        lstCategory.RemoveAt(pos);
                    }
                    else {
                        BookCategoryDto dto = new BookCategoryDto { BookId = bookCheck.Id, CategoryId = item };
                        Task task = new Task(() => _bookCategoryHelper.Create(dto));
                        task.Start();
                        TaskList.Add(task);
                    };
                }

                foreach(var c in lstCategory)
                {
                    BookCategoryDto dto = new BookCategoryDto { BookId = bookCheck.Id, CategoryId = c.CategoryId };
                    Task task = new Task(() => _bookCategoryHelper.Delete(dto));
                    task.Start();
                    TaskList.Add(task);
                }

                await Task.WhenAll(TaskList.ToArray());

                if (File.Exists(oldPath))
                {
                    File.Delete(oldPath);
                }
            }
            catch(Exception e)
            {
                if(!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    File.Delete(path);
                }
                throw new UserFriendlyException(e.Message);
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Books_Read)]
        public async Task<BookDto> GetAsync(int bookId)
        {
            try
            {
                var book = await _bookRepository.GetAsync(bookId);
                var result = ObjectMapper.Map<BookDto>(book);
                return result;
            }catch(Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Books_Read)]
        public async Task<ListResultDto<BookDto>> GetAll(BookFilter filter = null)
        {
            List<Book> allBooks = new List<Book>();
            if(filter != null)
            {
                if (filter.Category.HasValue)
                {
                    int id = filter.Category ?? default(int);
                    var temp = await _bookCategoryHelper.GetBookByCategoryId(id);
                    ObjectMapper.Map(temp, allBooks);
                }
                else
                {
                    allBooks = await _bookRepository
                        .GetAll().OrderByDescending(x => x.CreationTime)
                        .ToListAsync();
                }
                string keyword = filter.Keyword;
                BookState state = filter.State.GetValueOrDefault();
                var books = allBooks;

                int publisherId = filter.Publisher ?? default(int);
                int year = filter.Year ?? default(int);

                books = books.WhereIf(filter.Publisher.HasValue, x => x.PublisherId == publisherId).ToList();
                books = books.WhereIf(filter.Year.HasValue, x => x.PublishDate.Year == year).ToList();

                books = books.WhereIf(!String.IsNullOrEmpty(keyword), x => x.Title.ToLower().Contains(filter.Keyword.ToLower())).ToList();
                books = books.WhereIf(Enum.IsDefined(typeof(BookState), state), x => x.State == state).ToList();

                int totalCount = books.Count;

                if (totalCount > filter.SkipCount)
                {
                    books = books.Skip(filter.SkipCount).Take(filter.MaxResultCount).ToList();
                }
                var result = new PagedResultDto<BookDto>(totalCount, ObjectMapper.Map<List<BookDto>>(books)
                );
                return result;
            }
            else
            {
                allBooks = await _bookRepository
                    .GetAll().OrderByDescending(x => x.CreationTime)
                    .ToListAsync();
                var result = new PagedResultDto<BookDto>(allBooks.Count, ObjectMapper.Map<List<BookDto>>(allBooks));
                return result;
            }
           
        }

        [AbpAuthorize(PermissionNames.Pages_Books_Read)]
        public async Task<int> GetBookByPublisher(BookPublisherFilter filter)
        {
            var books = new List<BookDto>();
            if (filter.CategoryId.HasValue)
            {
                int categoryId = filter.CategoryId ?? default(int);
                var temp = await _bookCategoryHelper.GetBookByCategoryId(categoryId);
                books = temp.Where(x => x.PublisherId == filter.PublisherId).ToList();
            }
            else
            {
                var temp =  _bookRepository.GetAll().ToList().Where(x=>x.PublisherId == filter.PublisherId).ToList();
                ObjectMapper.Map(temp,books);
            }
            if (filter.Year.HasValue)
            {
                int year = filter.Year ?? default(int);
                books = books.Where(x => x.PublishDate.Year == year).ToList();
            }
            return books.Count;
        }
    }
}
