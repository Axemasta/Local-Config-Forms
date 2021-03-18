using LocalConfigPOC.Models;

namespace LocalConfigPOC.Managers
{
    /// <summary>
    /// Config Manager
    /// </summary>
    public interface IConfigManager
    {
        /// <summary>
        /// Get App Config
        /// </summary>
        /// <returns></returns>
        IAppConfig GetConfig();
    }
}