using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Select.Helpers
{
    public class Settings<T> where T : class
    {
        private readonly string _filePath;

        public Settings(string fileName)
        {
            _filePath = GetLocalFilePath(fileName);
        }

        private string GetLocalFilePath(string fileName)
        {
            var appData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Select");
            if (!Directory.Exists(appData))
            {
                Directory.CreateDirectory(appData);
            }
            return Path.Combine(appData, fileName);
        }

        public T LoadSettings() =>
            File.Exists(_filePath) ?
                JsonSerializer.Deserialize<T>(File.ReadAllText(_filePath)) :
                null;

        public void SaveSettings(T settings)
        {
            var json = JsonSerializer.Serialize(settings);
            File.WriteAllText(_filePath, json);
        }
    }
}