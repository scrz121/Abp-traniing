using Abp.Authorization;
using FirstProject.Authorization.Roles;
using FirstProject.Authorization.Users;

namespace FirstProject.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
