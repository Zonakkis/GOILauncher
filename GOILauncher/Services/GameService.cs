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

namespace GOILauncher.Services;

public class GameService(NotificationManager notificationManager)
{
    public GameInfo GetGameInfo(string gamePath)
    {
        var game = GetGameVersion(gamePath);
        var (modpack, levelLoader) = GetModpackandLevelLoaderVersion(gamePath);
        var bepInEx = GetBepinExVersion(gamePath);
        return new GameInfo
        {
            GameVersion = game,
            ModpackVersion = modpack,
            LevelLoaderVersion = levelLoader,
            BepInExVersion = bepInEx,
        };
    }
    private static string GetGameVersion(string gamePath)
    {
        var fileVersionInfo = FileVersionInfo.GetVersionInfo(Path.Combine(gamePath,"GettingOverIt.exe"));
        return fileVersionInfo.FilePrivatePart switch
        {
            37248 => "1.7",
            35766 => "1.6",
            5126 => "1.599",
            _ => "未知版本"
        };
    }
    private (string modpack, string levelLoader) GetModpackandLevelLoaderVersion(string gamePath)
    {
        try
        {
            (string modpack, string levelLoader) result = ("未安装", "未安装");
            var assemblyLoadContext = new AssemblyLoadContext("Assembly-CSharp", true); 
            assemblyLoadContext.LoadFromAssemblyPath(Path.Combine(gamePath, "GettingOverIt_Data/Managed/UnityEngine.CoreModule.dll"));
            var assembly = assemblyLoadContext.LoadFromAssemblyPath(Path.Combine(gamePath, "GettingOverIt_Data/Managed/Assembly-CSharp.dll"));
            var settingsManager = assembly.GetType("SettingsManager", true);
            var modpackBuildFieldInfo = settingsManager.GetField("build", BindingFlags.Static | BindingFlags.NonPublic);
            if (modpackBuildFieldInfo is not null)
            {
                result.modpack = (string)modpackBuildFieldInfo.GetValue(null);
            }
            var levelLoaderInfo = assembly.GetType("GOILevelImporter.ModInfo");
            if (levelLoaderInfo is not null)
            {
                var levelLoaderFullVersionPropertyInfo = levelLoaderInfo.GetProperty("FULLVERSION");
                result.levelLoader = (string)levelLoaderFullVersionPropertyInfo.GetValue(null);
            }
            assemblyLoadContext.Unload();
            return result;
        }
        catch (Exception ex)
        {
            notificationManager.AddNotification("获取Modpack和LevelLoader版本失败", ex.Message, InfoBarSeverity.Error);
            return ("获取失败", "获取失败");
        }
    }
    private static string GetBepinExVersion(string gamePath)
    {
        if (!Directory.Exists($"{gamePath}/BepInEx/core"))
        {
            return "未安装";
        }
        var assemblyLoadContext = new AssemblyLoadContext("BepInEx", true);
        string version;
        if (File.Exists($"{gamePath}/BepInEx/core/BepInEx.Preloader.Unity.dll"))
        {
            var assembly = assemblyLoadContext.LoadFromAssemblyPath(Path.Combine(gamePath,"BepInEx/core/BepInEx.Preloader.Unity.dll"));
            var assemblyInformationalVersionAttribute = (AssemblyInformationalVersionAttribute)assembly.GetCustomAttribute(typeof(AssemblyInformationalVersionAttribute));
            version = assemblyInformationalVersionAttribute.InformationalVersion;
        }
        else
        {
            var assembly = assemblyLoadContext.LoadFromAssemblyPath(Path.Combine(gamePath, "BepInEx/core/BepInEx.Preloader.dll"));
            var assemblyInformationalVersionAttribute = (AssemblyFileVersionAttribute)assembly.GetCustomAttribute(typeof(AssemblyFileVersionAttribute));
            version = assemblyInformationalVersionAttribute.Version;
        }
        assemblyLoadContext.Unload();
        return version;
    }
}