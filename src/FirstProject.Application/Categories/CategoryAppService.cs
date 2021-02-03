using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.UI;
using FirstProject.Authorization;
using FirstProject.Categories.Dto;
using FirstProject.Helper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstProject.Categories
{
    public class CategoryFilter
    {
        public int SkipCount { get; set; }
        public int MaxResultCount { get; set; }
        public string Keyword { get; set; }
    }
    [AbpAuthorize]
    public class CategoryAppService : FirstProjectAppServiceBase, ICategoryAppService
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly BookCategoryHelper _bookCategoryHelper;

        public CategoryAppService(IRepository<Category> categoryRepository, BookCategoryHelper bookCategoryHelper)
        {
            _categoryRepository = categoryRepository;
            _bookCategoryHelper = bookCategoryHelper;
        }
        [AbpAuthorize(PermissionNames.Pages_Categories)]
        public async Task Create(CreateCategoryDto input)
        {
            try
            {
                var find = _categoryRepository.GetAll().ToList()
                            .WhereIf(!string.IsNullOrEmpty(input.CategoryName), x => x.CategoryName.Trim().ToLower() == input.CategoryName.Trim().ToLower()).ToList();
                if (find.Count != 0)
                {
                    throw new Exception("Category Name exist");
                }
                var entity = ObjectMapper.Map<Category>(input);
                await _categoryRepository.InsertAsync(entity);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Categories)]
        public async Task Delete(DeleteCategoryDto dto)
        {
            int id = dto.CategoryId;
            try
            {
                var books = await _bookCategoryHelper.GetBookByCategoryId(id);
                if(books != null || books.Count > 0)
                {
                    throw new Exception("Tồn tại sách thuộc loại danh mục này!");
                }
                await _categoryRepository.DeleteAsync(id);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Books_Read)]
        public async Task<ListResultDto<CategoryDto>> GetAll(CategoryFilter filter = null)
        {
            try
            {
                var allPublishers = await _categoryRepository
                   .GetAll().OrderBy(x => x.CategoryName)
                   .ToListAsync();
                var books = allPublishers;
                int totalCount = books.Count;
                if (filter != null)
                {
                    string keyword = filter.Keyword;
                    books = allPublishers.WhereIf(!String.IsNullOrEmpty(keyword), x => x.CategoryName.ToLower().Contains(filter.Keyword.ToLower())).ToList();

                    totalCount = books.Count;

                    if (totalCount > filter.SkipCount)
                    {
                        books = books.Skip(filter.SkipCount).Take(filter.MaxResultCount).ToList();
                    }
                }
                var result = new PagedResultDto<CategoryDto>(totalCount, ObjectMapper.Map<List<CategoryDto>>(books));

                return result;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        [AbpAuthorize(PermissionNames.Pages_Categories)]
        public async Task Update(CategoryDto categoryDto)
        {
            try
            {
                var find = _categoryRepository.GetAll().ToList()
                                           .WhereIf(!string.IsNullOrEmpty(categoryDto.CategoryName), x => x.CategoryName.Trim().ToLower() == categoryDto.CategoryName.Trim().ToLower()).ToList();
                if (find.Count != 0)
                {
                    throw new Exception("Category Name exist");
                }
                var categoryCheck = await _categoryRepository.FirstOrDefaultAsync(categoryDto.Id);
                if (categoryCheck != null)
                {
                    categoryDto.CreatorUserId = categoryCheck.CreatorUserId;
                    ObjectMapper.Map(categoryDto, categoryCheck);
                }
                else
                {
                    throw new Exception("Not exist");
                }

            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
    }
}
