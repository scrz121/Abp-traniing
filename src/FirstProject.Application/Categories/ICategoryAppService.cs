using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FirstProject.Categories.Dto;

namespace FirstProject.Categories
{
    public interface ICategoryAppService : IApplicationService
    {
        Task<ListResultDto<CategoryDto>> GetAll(CategoryFilter filter);
        Task Create(CreateCategoryDto input);
        Task Delete(DeleteCategoryDto dto);
        Task Update(CategoryDto dto);
    }
}
