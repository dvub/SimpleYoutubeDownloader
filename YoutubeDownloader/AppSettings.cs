using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace YoutubeDownloader
{
    public class AppSettings
    {
        public string path { get; set; }

        [JsonIgnore]
        public static string FILE_NAME = AppContext.BaseDirectory + "appsettings.json";

        [JsonIgnore]
        public static bool IsCreated
          => (File.Exists(FILE_NAME));

        /// <summary>
        /// make new JSON file.
        /// </summary>
        /// <returns>new JSON config file</returns>
        public static AppSettings Create()
        {

            if (IsCreated)
            {
                return Load();

            }
            if (!Directory.Exists(AppContext.BaseDirectory + @"\save"))
                Directory.CreateDirectory(AppContext.BaseDirectory + @"\save");
            var config = new AppSettings()
            {

                path = ""
            };
            JsonSerializerOptions options = new()
            {
                WriteIndented = true
            };

            File.WriteAllBytes(FILE_NAME, JsonSerializer.SerializeToUtf8Bytes(config, options));
            return config;

        }
        /// <summary>
        /// Loads existing JSON file.
        /// </summary>
        /// <returns>JSON config file.</returns>
        public static AppSettings Load()
        {
            var readBytes = File.ReadAllBytes(FILE_NAME);
            var config = JsonSerializer.Deserialize<AppSettings>(readBytes);
            return config;
        }
    }
}
