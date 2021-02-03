using Abp.Domain.Entities.Auditing;
using Abp.Timing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FirstProject.Publishers;
using System.Collections.Generic;
using FirstProject.BookCategories;

namespace FirstProject.Books
{
    public enum BookState : int
    {
        Old = 0,
        New = 1
    }

    [Table("Books")]
    public class Book : AuditedEntity<int>
    {
        public Book()
        {
            CreationTime = Clock.Now;
        }

        public Book(string title, string description = null)
            : this()
        {
            Title = title;
            Description = description;
        }

        public const int MaxTitleLength = 256;
        public const int MaxDescriptionLength = 64 * 1024;
        
        [Required]
        public int PublisherId { get; set; }

        [Required]
        public DateTime PublishDate { get; set; }

        [Required]
        [StringLength(MaxTitleLength)]
        public string Title { get; set; }

        public BookState State { get; set; }

        [StringLength(MaxDescriptionLength)]
        public string Description { get; set; }

        [Required]
        public int InventoryNumber { get; set; }

        [Required]
        public string CoverImage { get; set; }
        
        [ForeignKey("PublisherId")]
        public virtual Publisher Publisher{ get; set; }

        [NotMapped]
        public ICollection<BookCategory> BookCategories { get; set; }

    }
}
