using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace FirstProject.Authorization
{
    public class FirstProjectAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);

            #region Book Permission
            var bookManager = context.CreatePermission(PermissionNames.Pages_Books, L("Books"));
            bookManager.CreateChildPermission(PermissionNames.Pages_Books_Read,L(PermissionNames.Pages_Books_Read));
            bookManager.CreateChildPermission(PermissionNames.Pages_Books_Create, L(PermissionNames.Pages_Books_Create));
            //bookManager.CreateChildPermission(PermissionNames.Pages_Books_Update, L(PermissionNames.Pages_Books_Update));
            //bookManager.CreateChildPermission(PermissionNames.Pages_Books_Delete, L(PermissionNames.Pages_Books_Delete));
            #endregion

            #region Category Permission
            var categoryManagement = context.CreatePermission(PermissionNames.Pages_Categories, L("Categories"));
            categoryManagement.CreateChildPermission(PermissionNames.Pages_Categories_Read, L(PermissionNames.Pages_Categories_Read));
            //categoryManagement.CreateChildPermission(PermissionNames.Pages_Categories_Create, L(PermissionNames.Pages_Categories_Create));
            //categoryManagement.CreateChildPermission(PermissionNames.Pages_Categories_Update, L(PermissionNames.Pages_Categories_Update));
            //categoryManagement.CreateChildPermission(PermissionNames.Pages_Categories_Delete, L(PermissionNames.Pages_Categories_Delete));
            #endregion

            #region Publisher Permission
            var publisherManagement = context.CreatePermission(PermissionNames.Pages_Publishers, L("Publishers"));
            publisherManagement.CreateChildPermission(PermissionNames.Pages_Publishers_Read, L(PermissionNames.Pages_Publishers_Read));
            //publisherManagement.CreateChildPermission(PermissionNames.Pages_Publishers_Create, L(PermissionNames.Pages_Publishers_Create));
            //publisherManagement.CreateChildPermission(PermissionNames.Pages_Publishers_Update, L(PermissionNames.Pages_Publishers_Update));
            //publisherManagement.CreateChildPermission(PermissionNames.Pages_Publishers_Delete, L(PermissionNames.Pages_Publishers_Delete));
            #endregion

            #region BookCategory Permission
            var bookCategoryManagement = context.CreatePermission(PermissionNames.Pages_BookCategory, L("BookCategory"));
            bookCategoryManagement.CreateChildPermission(PermissionNames.Pages_BookCategory_Read, L(PermissionNames.Pages_BookCategory_Read));
            //bookCategoryManagement.CreateChildPermission(PermissionNames.Pages_Publishers_Create, L(PermissionNames.Pages_BookCategory_Create));
            //bookCategoryManagement.CreateChildPermission(PermissionNames.Pages_Publishers_Update, L(PermissionNames.Pages_BookCategory_Update));
            //bookCategoryManagement.CreateChildPermission(PermissionNames.Pages_Publishers_Delete, L(PermissionNames.Pages_BookCategory_Delete));
            #endregion
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, FirstProjectConsts.LocalizationSourceName);
        }
    }
}
