using LocalConfigPOC.Enums;

namespace LocalConfigPOC.Models
{
    public interface IAppConfig
    {
        string SecretKey { get; }
        
        string ApiUrl { get; }
        
        Environment Environment { get; }
        
        DeploymentTarget DeploymentTarget { get; }
    }
}