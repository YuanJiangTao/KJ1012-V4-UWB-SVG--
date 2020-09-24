using System.IO;
using KJ1012.AppSetting.IServices;
using KJ1012.Core.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KJ1012.AppSetting.Services
{
    public class AppSettingService : IAppSettingService
    {
        private readonly IOptionsMonitor<DataSettings> _dataSetting;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public AppSettingService(IOptionsMonitor<DataSettings> dataSetting,
            IWebHostEnvironment hostingEnvironment)
        {
            _dataSetting = dataSetting;
            _hostingEnvironment = hostingEnvironment;
        }

        public void SaveDataSetting(DataSettings dataSettings)
        {
            string appSettingName = "appSettings.json";
            if (!_hostingEnvironment.IsProduction())
            {
                appSettingName = $"appSettings.{_hostingEnvironment.EnvironmentName}.json";
            }
            var appSettingsJson = JObject.Parse(File.ReadAllText(appSettingName));
            appSettingsJson["DataSettings"] = JToken.FromObject(dataSettings);
            File.WriteAllText(appSettingName, JsonWriter(JsonConvert.SerializeObject(appSettingsJson)));
        }

        public DataSettings GetDataSetting()
        {
            return _dataSetting?.CurrentValue ?? new DataSettings();
        }
        private string JsonWriter(string json)
        {
            JsonSerializer serializer = new JsonSerializer();
            TextReader tr = new StringReader(json);
            JsonTextReader jtr = new JsonTextReader(tr);
            object obj = serializer.Deserialize(jtr);
            if (obj != null)
            {
                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 2,
                    IndentChar = ' '
                };
                serializer.Serialize(jsonWriter, obj);
                return textWriter.ToString();
            }

            return string.Empty;
        }
    }
}