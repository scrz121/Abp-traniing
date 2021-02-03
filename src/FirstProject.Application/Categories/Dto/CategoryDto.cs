using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace FirstProject.Categories.Dto
{
    [AutoMap(typeof(Category))]
    public class CategoryDto : AuditedEntity<int>
    {
        public string CategoryName { get; set; }
    }
}
