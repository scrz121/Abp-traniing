using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using FirstProject.Configuration.Dto;

namespace FirstProject.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : FirstProjectAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
