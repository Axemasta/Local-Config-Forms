using LocalConfigPOC.Models;

namespace LocalConfigPOC.Managers
{
    /// <summary>
    /// Config Manager
    /// </summary>
    public class ConfigManager : IConfigManager
    {
        private readonly INativeConfigManager _nativeConfigManager;
        
        public ConfigManager(INativeConfigManager nativeConfigManager)
        {
            _nativeConfigManager = nativeConfigManager;
        }

        /// <inheritdoc/>
        public IAppConfig GetConfig()
        {
            return _nativeConfigManager.ReadConfig();
        }
    }
}