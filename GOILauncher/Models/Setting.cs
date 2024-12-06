using Avalonia;
using GOILauncher.Helpers;
using System;
using System.IO;

namespace GOILauncher.Models
{
    public class Setting
    {
        private bool nightMode;
        private bool saveMapZip;
        private string downloadPath;
        private string steamPath;
        private string levelPath;
        private string gamePath;
        private int previewQuality;
        public event Action? GamePathChanged;
        public event Action? LevelPathChanged;

        static Setting()
        {
            if (File.Exists($"{AppDomain.CurrentDomain.BaseDirectory}Settings.json"))
            {
                Instance = StorageHelper.LoadJson<Setting>(AppDomain.CurrentDomain.BaseDirectory, "Settings.json");
                if (Instance.NightMode)
                {
                    Application.Current!.RequestedThemeVariant = Avalonia.Styling.ThemeVariant.Dark;
                }
            }
            else Instance = new()
            {
                GamePath = GetDefaultValue<string>(nameof(GamePath)),
                LevelPath = GetDefaultValue<string>(nameof(LevelPath)),
                SteamPath = GetDefaultValue<string>(nameof(SteamPath)),
                DownloadPath = GetDefaultValue<string>(nameof(DownloadPath)),
                SaveMapZip = GetDefaultValue<bool>(nameof(SaveMapZip)),
                PreviewQuality = GetDefaultValue<int>(nameof(PreviewQuality)),
            };
        }
        public Setting()
        {
            gamePath = GetDefaultValue<string>(nameof(GamePath));
            levelPath = GetDefaultValue<string>(nameof(LevelPath));
            steamPath = GetDefaultValue<string>(nameof(SteamPath));
            downloadPath = GetDefaultValue<string>(nameof(DownloadPath));
            saveMapZip = GetDefaultValue<bool>(nameof(SaveMapZip));
            previewQuality = GetDefaultValue<int>(nameof(PreviewQuality));
        }

        public bool IsDefault(string propertyName)
        {
            var property = typeof(Setting).GetProperty(propertyName) ?? throw new ArgumentException($"Property '{propertyName}' not found.");
            var propertyValue = property.GetValue(this);
            var defaultValue = typeof(Setting).GetMethod(nameof(GetDefaultValue))!
                .MakeGenericMethod(property.PropertyType)
                .Invoke(null, [propertyName]);
            return propertyValue?.Equals(defaultValue) ?? defaultValue == null;
        }
        public static T GetDefaultValue<T>(string propertyName)
        {
            if (typeof(T) == typeof(string))
            {
                switch (propertyName)
                {
                    case nameof(GamePath):
                        return (T)(object)"未选择";
                    case nameof(LevelPath):
                        return (T)(object)"未选择（选择游戏路径后自动选择，也可手动更改）";
                    case nameof(SteamPath):
                        return (T)(object)"未选择（需要通过Steam启动游戏时才选择，否则可不选）";
                    case nameof(DownloadPath):
                        return (T)(object)$"{AppDomain.CurrentDomain.BaseDirectory}Download";
                }
            }
            else if (typeof(T) == typeof(bool))
            {
                if (propertyName == nameof(SaveMapZip))
                {
                    return (T)(object)false;
                }
            }
            else if (typeof(T) == typeof(int))
            {
                if (propertyName == nameof(PreviewQuality))
                {
                    return (T)(object)40;
                }
            }
            return default!;
        }
        public string GamePath
        {
            get => gamePath; 
            set
            {
                if(gamePath != value)
                {
                    gamePath = value;
                    Save();
                    GamePathChanged?.Invoke();
                }

            }
        }
        public string LevelPath
        {
            get => levelPath; 
            set
            {
                if(levelPath != value)
                {
                    levelPath = value;
                    Save();
                    LevelPathChanged?.Invoke();
                }
            }
        }
        public string SteamPath
        {
            get => steamPath; 
            set
            {
                steamPath = value;
                Save();
            }
        }
        public string DownloadPath
        {
            get => downloadPath;
            set
            {
                downloadPath = value;
                Save();
            }
        }
        public bool SaveMapZip
        {
            get => saveMapZip; 
            set
            {
                saveMapZip = value;
                Save();
            }
        }
        public bool NightMode
        {
            get => nightMode;
            set
            {
                nightMode = value;
                Save();
                Application.Current!.RequestedThemeVariant = value ? Avalonia.Styling.ThemeVariant.Dark : Avalonia.Styling.ThemeVariant.Light;
            }
        }
        public int PreviewQuality
        {
            get => previewQuality; set
            {
                previewQuality = value; 
                Save();
            }
        }
        public static Setting Instance { get; private set; }
        public void Save()
        {
            StorageHelper.SaveJson(AppDomain.CurrentDomain.BaseDirectory, "Settings.json", this, true);
        }
    }
}
