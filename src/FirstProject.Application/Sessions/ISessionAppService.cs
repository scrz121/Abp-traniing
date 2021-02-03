using System.Threading.Tasks;
using Abp.Application.Services;
using FirstProject.Sessions.Dto;

namespace FirstProject.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
