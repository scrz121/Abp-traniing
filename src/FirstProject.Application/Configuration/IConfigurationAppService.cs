using System.Threading.Tasks;
using FirstProject.Configuration.Dto;

namespace FirstProject.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
