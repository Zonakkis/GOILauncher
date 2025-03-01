using System.IO;
using GOILauncher.Models;
using Version = GOILauncher.Models.Version;

namespace GOILauncher.Services;

public class AppService
{
    private static Setting LoadSetting()
    {
        if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(),"Settings.json")))
        {
            return FileService.LoadFromJson<Setting>(Directory.GetCurrentDirectory(), "Settings.json");
        }
        return new Setting
        {
            DownloadPath = Path.Combine(Directory.GetCurrentDirectory(), "Download"),
            PreviewQuality = 40
        };
    }

    public void SaveSetting()
    {
        FileService.SaveAsJson(Directory.GetCurrentDirectory(), "Settings.json", Setting);
    }
    public Setting Setting { get; } = LoadSetting();
    public Version Version { get; } = new("0.2.4");
}