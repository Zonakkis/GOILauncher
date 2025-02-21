using GOILauncher.Models;
using System;
using System.IO;
using Version = GOILauncher.Models.Version;

namespace GOILauncher.Services;

public class AppService
{
    private static Setting LoadSetting()
    {
        if (File.Exists($"{AppDomain.CurrentDomain.BaseDirectory}Settings.json"))
        {
            return FileService.LoadFromJson<Setting>(AppDomain.CurrentDomain.BaseDirectory, "Settings.json");
        }
        return new Setting
        {
            DownloadPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Download"),
            PreviewQuality = 40
        };
    }

    public void SaveSetting()
    {
        FileService.SaveAsJson(AppDomain.CurrentDomain.BaseDirectory, "Settings.json", Setting);
    }
    public Setting Setting { get; } = LoadSetting();
    public Version Version { get; } = new("0.2.3");
}