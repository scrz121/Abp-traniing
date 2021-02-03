using Abp.AutoMapper;
using FirstProject.Books;
using System;
using System.Collections.Generic;
using System.Text;

namespace FirstProject.Publishers.Dto
{
    [AutoMap(typeof(Publisher))]
    public class CreatePublisherDto
    {
        public string PublisherName { get; set; }
    }
}
