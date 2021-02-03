using Abp.Application.Services.Dto;
using System;

namespace FirstProject.Books.Dto
{
    //custom PagedResultRequestDto
    public class PagedBookResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}
