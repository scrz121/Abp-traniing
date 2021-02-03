using Abp.AutoMapper;
using FirstProject.Books;
using System;
using System.Collections.Generic;
using System.Text;

namespace FirstProject.Categories.Dto
{
    [AutoMap(typeof(Category))]
    public class CreateCategoryDto
    {
        public string CategoryName { get; set; }
    }
}
