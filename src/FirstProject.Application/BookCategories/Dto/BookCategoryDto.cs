using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using FirstProject.Books;
using FirstProject.Categories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using FirstProject.BookCategories;
using System.ComponentModel.DataAnnotations;

namespace FirstProject.BookCategories.Dto
{
    [AutoMap(typeof(BookCategory))]
    public class BookCategoryDto : AuditedEntity<int>
    {
        [Required]
        public int BookId { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
