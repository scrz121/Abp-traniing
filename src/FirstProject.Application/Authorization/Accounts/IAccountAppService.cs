using System.Threading.Tasks;
using Abp.Application.Services;
using FirstProject.Authorization.Accounts.Dto;

namespace FirstProject.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
