using Abp.Domain.Entities.Auditing;
using FirstProject.BookCategories;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstProject.Categories
{
    [Table("Categories")]
    public class Category : AuditedEntity<int>
    {
        [Required]
        public string CategoryName { get; set; }

        [NotMapped]
        public ICollection<BookCategory> BookCategories { get; set; }
    }
}
