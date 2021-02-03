using System.Collections.Generic;
using FirstProject.Roles.Dto;

namespace FirstProject.Web.Models.Users
{
    public class UserListViewModel
    {
        public IReadOnlyList<RoleDto> Roles { get; set; }
    }
}
