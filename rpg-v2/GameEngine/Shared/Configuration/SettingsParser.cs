using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace game.GameEngine.Shared.Configuration;

public static class SettingsParser
{
    private static readonly string SettingsPath = 
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SourceOfMagic", "Settings");

    private static readonly object Lock = new object();
    
    public delegate void OnSettingsChangeHandler();
    public static event OnSettingsChangeHandler? OnSettingsChange;

    public static T GetSettings<T>() where T: struct, ISettings
    {
        lock (Lock)
        {
            var path = Path.Combine(SettingsPath, $"{typeof(T).Name}.json");
            
            if (!File.Exists(path))
                SaveSettings(new T());
            
            using var stream = File.Open(path, FileMode.OpenOrCreate);
            var settings = JsonSerializer.Deserialize<T>(stream);
            return settings;
        }
        
    }

    public static void SaveSettings<T>(T settings) where T: struct, ISettings
    {
        lock (Lock)
        {
            var json = JsonSerializer.Serialize(settings);
            var path = Path.Combine(SettingsPath, $"{typeof(T).Name}.json");
            Directory.CreateDirectory(SettingsPath);
            File.WriteAllText(path, json);
            OnSettingsChange?.Invoke();
        }
        
    }
}