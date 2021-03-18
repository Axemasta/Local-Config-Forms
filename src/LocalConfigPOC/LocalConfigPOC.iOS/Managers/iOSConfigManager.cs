using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Foundation;
using LocalConfigPOC.iOS.Managers;
using LocalConfigPOC.Managers;
using LocalConfigPOC.Models;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly: Dependency(typeof(iOSConfigManager))]
namespace LocalConfigPOC.iOS.Managers
{
    public class iOSConfigManager : INativeConfigManager
    {
        public IAppConfig ReadConfig()
        {
            //var plist = LoadPlistFromBundle("config");

            //if (plist == null)
            //    return null; // log / throw here

            //return ConvertPlistToConfig(plist);

            var jsonPath = NSBundle.MainBundle.PathForResource("config", "json");

            var fileContents = LoadFileString(jsonPath);

            if (string.IsNullOrEmpty(fileContents))
                return null; // log / throw here

            return ConvertConfigJson(fileContents);
        }

        #region Load JSON

        private string LoadFileString(string path)
        {
            using (var file = File.Open(path, FileMode.Open))
            using (var reader = new StreamReader(file))
            {
                return reader.ReadToEnd();
            }
        }

        private IAppConfig ConvertConfigJson(string json)
        {
            return JsonConvert.DeserializeObject<AppConfig>(json);
        }

        #endregion Load JSON

        #region Load Plist

        /// <summary>
        /// Convert NS Object To .NET Object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private object ToDotNet(object obj)
        {
            var type = obj.GetType();

            if (type == typeof(NSString) || type == typeof(NSMutableString))
            {
                return obj.ToString();
            }
            else if (type == typeof(NSNumber))
            {
                var num = obj as NSNumber;

                return num.Int32Value;
            }

            throw new NotSupportedException($"Can not convert object to dotnet type: {type.Name}");
        }

        /// <summary>
        /// Load Plist Fro,m Bundle
        /// </summary>
        /// <param name="plistName"></param>
        /// <returns></returns>
        private NSDictionary LoadPlistFromBundle(string plistName)
        {
            var path = NSBundle.MainBundle.PathForResource(plistName, "plist");

            return NSDictionary.FromFile(path);
        }

        private IAppConfig ConvertPlistToConfig(NSDictionary configDict)
        {
            var config = new AppConfig();

            var props = config.GetType().GetTypeInfo().DeclaredProperties.ToList();

            foreach (var entry in configDict)
            {
                string propName = entry.Key.ToString();

                PropertyInfo prop = props.FirstOrDefault(pi =>
                    pi.Name == propName ||
                    pi.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName == propName);

                if (prop == null)
                    continue;

                object propValue = ToDotNet(entry.Value);

                prop.SetValue(config, propValue);
            }

            return config;
        }

        #endregion Load Plist
    }
}