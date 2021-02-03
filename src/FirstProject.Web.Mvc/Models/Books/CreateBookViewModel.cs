using Abp.AutoMapper;
using Abp.Localization;
using FirstProject.Books;
using FirstProject.Books.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FirstProject.Web.Models.Books
{
    [AutoMapTo(typeof(CreateBookDto))]
    public class CreateBookViewModel
    {
        public const int MaxTitleLength = 256;
        public const int MaxDescriptionLength = 64 * 1024;

        [Required]
        [StringLength(MaxTitleLength)]
        public string Title { get; set; }

        [StringLength(MaxDescriptionLength)]
        public string Description { get; set; }

        [Required]
        public BookState State { get; set; }

        [Required]
        public int InventoryNumber { get; set; }
    }
}
