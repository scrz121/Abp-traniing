using Abp.AutoMapper;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FirstProject.Books.Dto
{
    [AutoMap(typeof(Book))]
    public class CreateBookDto
    {
        [Required]
        public int PublisherId { get; set; }

        [Required]
        public DateTime PublishDate { get; set; }

        [Required]
        public string Title { get; set; }

        public BookState State { get; set; }

        public string Description { get; set; }

        [Required]
        public int InventoryNumber { get; set; }

        [Ignore]
        public IFormFile CoverImageFile { get; set; }

        [Ignore]
        public List<int> Category { get; set; }

        public string CoverImage { get; set; } 
    }
}
