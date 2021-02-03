using Abp.Domain.Entities.Auditing;
using FirstProject.Books;
using FirstProject.Categories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FirstProject.BookCategories
{
    public class BookCategory : AuditedEntity<int>
    {
        public int BookId { get; set; }
        public int CategoryId { get; set; }

        [ForeignKey(nameof(BookId))]
        public Book Book { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }
    }
}
