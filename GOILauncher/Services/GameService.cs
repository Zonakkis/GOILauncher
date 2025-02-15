using Avalonia.Controls.Notifications;
using GOILauncher.Helpers;
using GOILauncher.Models;
using GOILauncher.UI;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using FluentAvalonia.UI.Controls;
using System.Diagnostics.CodeAnalysis;

namespace GOILauncher.Services;

public class GameService(NotificationManager notificationManager)
{
    public void SetGamePath(string gamePath)
    {
        _gamePath = gamePath;
        GetGameInfo();
    }
    private void GetGameInfo()
    {
        if(string.IsNullOrEmpty(_gamePath))
        {
            return;
        }    
        GetGameVersion();
        GetModpackandLevelLoaderVersion();
        GetBepinExVersion();
    }
    private void GetGameVersion()
    {
        var fileVersionInfo = FileVersionInfo.GetVersionInfo(Path.Combine(_gamePath,"GettingOverIt.exe"));
        GameInfo.GameVersion = fileVersionInfo.FilePrivatePart switch
        {
            37248 => "1.7",
            35766 => "1.6",
            5126 => "1.599",
            _ => "未知版本"
        };
    }
    public void GetModpackandLevelLoaderVersion()
    {
        try
        {
            var assemblyLoadContext = new AssemblyLoadContext("Assembly-CSharp", true); 
            assemblyLoadContext.LoadFromAssemblyPath(Path.Combine(_gamePath, "GettingOverIt_Data/Managed/UnityEngine.CoreModule.dll"));
            var assembly = assemblyLoadContext.LoadFromAssemblyPath(Path.Combine(_gamePath, "GettingOverIt_Data/Managed/Assembly-CSharp.dll"));
            var levelLoaderInfo = assembly.GetType("GOILevelImporter.ModInfo");
            if (levelLoaderInfo is not null)
            {
                var levelLoaderFullVersionPropertyInfo = levelLoaderInfo.GetProperty("FULLVERSION");
                GameInfo.LevelLoaderVersion = (string)levelLoaderFullVersionPropertyInfo.GetValue(null);
            }
            else
            {
                GameInfo.LevelLoaderVersion = "未安装";
            }
            var settingsManager = assembly.GetType("SettingsManager", true);
            var modpackBuildFieldInfo = settingsManager.GetField("build", BindingFlags.Static | BindingFlags.NonPublic);
            if (modpackBuildFieldInfo is not null)
            {
                GameInfo.ModpackVersion = (string)modpackBuildFieldInfo.GetValue(null);
            }
            else
            {
                GameInfo.ModpackVersion = "未安装";
            }
            assemblyLoadContext.Unload();
        }
        catch (Exception ex)
        {
            GameInfo.ModpackVersion = "已安装";
            notificationManager.AddNotification("获取Modpack版本失败", ex.Message, InfoBarSeverity.Error);
        }
    }
    private void GetBepinExVersion()
    {
        if (!Directory.Exists($"{_gamePath}/BepInEx/core"))
        {
            GameInfo.BepInExVersion = "未安装";
        }
        var assemblyLoadContext = new AssemblyLoadContext("BepInEx", true);
        if (File.Exists($"{_gamePath}/BepInEx/core/BepInEx.Preloader.Unity.dll"))
        {
            var assembly = assemblyLoadContext.LoadFromAssemblyPath(Path.Combine(_gamePath, "BepInEx/core/BepInEx.Preloader.Unity.dll"));
            var assemblyInformationalVersionAttribute = (AssemblyInformationalVersionAttribute)assembly.GetCustomAttribute(typeof(AssemblyInformationalVersionAttribute));
            GameInfo.BepInExVersion = assemblyInformationalVersionAttribute.InformationalVersion;
        }
        else
        {
            var assembly = assemblyLoadContext.LoadFromAssemblyPath(Path.Combine(_gamePath, "BepInEx/core/BepInEx.Preloader.dll"));
            var assemblyInformationalVersionAttribute = (AssemblyFileVersionAttribute)assembly.GetCustomAttribute(typeof(AssemblyFileVersionAttribute));
            GameInfo.BepInExVersion = assemblyInformationalVersionAttribute.Version;
        }
        assemblyLoadContext.Unload();
    }
    public GameInfo GameInfo { get; } = new();

    private string _gamePath;
}