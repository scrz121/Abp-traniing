using System.Collections.Generic;
using FirstProject.Roles.Dto;

namespace FirstProject.Web.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }
    }
}