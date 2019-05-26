using System;
using System.IO;
using Cards;
using Microsoft.Extensions.Configuration;
using OxfordDictionaries;

namespace consoleApp
{
    class SettingsProvider : IOxfordDictionarySettingsProvider
    {
        private const string _appsettingsFile = "appsettings.json";
        private const string _cardsDatabaseSettingName = "CardsDatabase";
        private const string _oxfordDictionaryCacheDatabaseSettingName = "OxfordDictionaryCacheDatabase";

        IConfigurationRoot _configuration;

        public SettingsProvider()
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile(_appsettingsFile, optional: true, reloadOnChange: true)
              .AddUserSecrets<SettingsProvider>();

            _configuration = builder.Build();

            Environment.SetEnvironmentVariable(OxfordDictionariesConstants.OxfordCacheDbConnectionStringEnvironmentVariableName, _configuration.GetConnectionString(_oxfordDictionaryCacheDatabaseSettingName));
        }

        private T LoadSettings<T>(string sectionName)
        {
            var result = (T)Activator.CreateInstance(typeof(T));
            _configuration.GetSection(sectionName).Bind(result);
            return result;
        }

        public OxfordDictionarySettings GetOxfordDictionarySettings()
        {
            return LoadSettings<OxfordDictionarySettings>(nameof(OxfordDictionarySettings));
        }
    }
}
