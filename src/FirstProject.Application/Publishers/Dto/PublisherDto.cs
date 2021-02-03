using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace FirstProject.Publishers.Dto
{
    [AutoMap(typeof(Publisher))]
    public class PublisherDto : AuditedEntity<int>
    {
        public string PublisherName { get; set; }
    }
}
