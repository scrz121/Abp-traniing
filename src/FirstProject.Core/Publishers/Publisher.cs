using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FirstProject.Publishers

{
    [Table("Publishers")]
    public class Publisher : AuditedEntity<int>
    {
        public string PublisherName { get; set; }
    }
}
