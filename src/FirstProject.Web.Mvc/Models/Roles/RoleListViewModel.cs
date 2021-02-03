using System.Collections.Generic;
using FirstProject.Roles.Dto;

namespace FirstProject.Web.Models.Roles
{
    public class RoleListViewModel
    {
        public IReadOnlyList<PermissionDto> Permissions { get; set; }
    }
}
