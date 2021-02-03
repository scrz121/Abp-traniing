using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using FirstProject.Roles.Dto;
using FirstProject.Users.Dto;

namespace FirstProject.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>
    {
        Task<ListResultDto<RoleDto>> GetRoles();

        Task ChangeLanguage(ChangeUserLanguageDto input);

        Task<bool> ChangePassword(ChangePasswordDto input);
    }
}
