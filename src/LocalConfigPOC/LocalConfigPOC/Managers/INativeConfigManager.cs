using LocalConfigPOC.Models;

namespace LocalConfigPOC.Managers
{
    /// <summary>
    /// Native Config Manager
    /// </summary>
    public interface INativeConfigManager
    { 
        IAppConfig ReadConfig();
    }
}