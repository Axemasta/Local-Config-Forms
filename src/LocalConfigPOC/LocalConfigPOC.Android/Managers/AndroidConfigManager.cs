using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Android.Content;
using Java.IO;
using LocalConfigPOC.Android.Managers;
using LocalConfigPOC.Managers;
using LocalConfigPOC.Models;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidConfigManager))]
namespace LocalConfigPOC.Android.Managers
{
    public class AndroidConfigManager : INativeConfigManager
    {
        private Context _context;

        public AndroidConfigManager()
        {
            _context = Xamarin.Essentials.Platform.AppContext;
        }

        public IAppConfig ReadConfig()
        {
            //return LoadConfigFromResources();

            return LoadConfigFromAssets();
        }

        #region Load JSON

        /// <summary>
        /// Load Json File From Assets Folder (This is not compiled)
        /// </summary>
        /// <returns></returns>
        private IAppConfig LoadConfigFromAssets()
        {
            var resources = _context.Resources;
            var assetManager = resources?.Assets;
            const string configPath = "config.json";
            string content;

            using var configStream = assetManager?.Open(configPath);
            if (configStream == null) return null;
            using (var reader = new StreamReader(configStream))
            {
                content = reader.ReadToEnd();
            }

            if (string.IsNullOrEmpty(content))
                return null;

            return ParseConfigString(content);
        }

        private IAppConfig ParseConfigString(string configString)
        {
            return JsonConvert.DeserializeObject<AppConfig>(configString);
        }

        #endregion Load JSON

        #region Load Resource XML

        /// <summary>
        /// Load Config From Resources, This file gets compiled so can't be altered by unpacking the apk
        /// </summary>
        /// <returns></returns>
        private IAppConfig LoadConfigFromResources()
        {
            var resources = _context.Resources;

            var configDict = new Dictionary<string, int>()
            {
                { nameof(IAppConfig.ApiUrl), Resource.String.ApiUrl },
                { nameof(IAppConfig.SecretKey), Resource.String.Secret },
                { nameof(IAppConfig.DeploymentTarget), Resource.Integer.DeploymentTarget },
                { nameof(IAppConfig.Environment), Resource.Integer.Environment }
            };

            var config = new AppConfig();

            var props = config.GetType().GetTypeInfo().DeclaredProperties.ToList();

            foreach (var configVal in configDict)
            {
                PropertyInfo prop = props.FirstOrDefault(pi =>
                    pi.Name == configVal.Key ||
                    pi.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName == configVal.Key);

                var propValue = resources.GetString(configVal.Value);

                if (prop.PropertyType == typeof(string))
                {
                    prop.SetValue(config, propValue);
                }
                else
                {
                    var number = Convert.ToInt32(propValue);

                    prop.SetValue(config, number);
                }
            }

            return config;
        }

        #endregion Load Resource XML
    }
}
