using LocalConfigPOC.Enums;
using Newtonsoft.Json;

namespace LocalConfigPOC.Models
{
    public class AppConfig : IAppConfig
    {
        [JsonProperty("Secret")]
        public string SecretKey { get; set; }
        
        [JsonProperty("ApiUrl")]
        public  string ApiUrl { get; set; }
        
        [JsonProperty("Environment")]
        public Environment Environment { get; set; }
        
        [JsonProperty("DeploymentTarget")]
        public DeploymentTarget DeploymentTarget { get; set; }
    }
}