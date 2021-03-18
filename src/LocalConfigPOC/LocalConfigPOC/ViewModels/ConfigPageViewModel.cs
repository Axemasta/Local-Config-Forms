using LocalConfigPOC.Managers;
using LocalConfigPOC.Models;
using MvvmHelpers;

namespace LocalConfigPOC.ViewModels
{
    public class ConfigViewModel : BaseViewModel
    {
        private readonly IConfigManager _configManager;

        private IAppConfig _config;
        public IAppConfig Config
        {
            get => _config;
            set => SetProperty(ref _config, value);
        }
        
        public ConfigViewModel(IConfigManager configManager)
        {
            _configManager = configManager;

            Title = "Here Is My Config!";

            _config = _configManager.GetConfig();
        }
    }
}