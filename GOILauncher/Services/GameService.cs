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
using System.Collections.ObjectModel;
using System.Linq;
using GOILauncher.ViewModels.Pages;
using System.Threading;
using System.Collections.Generic;

namespace GOILauncher.Services;

public class GameService(NotificationManager notificationManager)
{
    public void SetGamePath(string gamePath)
    {
        _gamePath = gamePath;
        GetGameInfo();
    }
    public void SetLevelPath(string levelPath)
    {
        _levelPath = levelPath;
        GetLocalMaps();
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

    private void GetLocalMaps()
    {
        LocalMaps.Clear();
        _localMapsDictionary.Clear();
        foreach (var directory in Directory.GetDirectories(_levelPath, "* by *"))
        { 
            var directoryName = Path.GetFileName(directory).Replace("By","by");
            var mapName = directoryName.Split(" by ")[0];
            var author = directoryName.Split(" by ")[1];
            var size = Directory.GetFiles(directory, "*.scene")
                                .Aggregate<string, long>(0, (size, scene) => size + new FileInfo(scene).Length);
            AddMap(new Map
            {
                Name = mapName,
                Author = author,
                Size = StorageUnitConvertHelper.ByteTo(size)
            });
        }
        foreach (var scene in Directory.GetFiles(_levelPath, "*.scene"))
        {
            var mapName = Path.GetFileNameWithoutExtension(scene);
            string? configFile = null;
            if (File.Exists(Path.ChangeExtension(scene, "txt")))
            {
                configFile = Path.ChangeExtension(scene, "txt");
            }
            else if (File.Exists(Path.ChangeExtension(scene, "mdata")))
            {
                configFile = Path.ChangeExtension(scene, "mdata");
            }
            string? credit = null;
            if (!string.IsNullOrEmpty(configFile))
            {
                var lines = File.ReadAllLines(configFile);
                var configs = lines.Select(line => line.Split('='))
                                   .ToDictionary(key => key[0].Trim(), val => val[1].Trim());
                credit = configs.GetValueOrDefault("credit");
            }
            var map = new Map
            {
                Name = mapName,
                Size = StorageUnitConvertHelper.ByteTo(new FileInfo(scene).Length),
                Author = credit
            };
            AddMap(map);
        }
    }
    public bool CheckWhetherMapExists(Map map)
    {
        return _localMapsDictionary.ContainsKey(map.Name);
    }
    public void AddMap(Map map)
    {
        if(_localMapsDictionary.TryGetValue(map.Name,out var localmap))
        {
            LocalMaps.Remove(localmap);
            _localMapsDictionary.Remove(map.Name);
        }
        LocalMaps.Add(map);
        _localMapsDictionary.Add(map.Name, map);
    }
    public void RemoveMap(Map map)
    {
        LocalMaps.Remove(map);
        _localMapsDictionary.Remove(map.Name);
    }
    public void DeleteMap(Map map)
    {
        if(Directory.GetDirectories(_levelPath, "* by *")
           .FirstOrDefault(directory => Path.GetFileName(directory).Contains(map.Name)) is { } direct)
        {
            Directory.Delete(direct, true);
        }
        foreach (var scene in Directory.GetFiles(_levelPath)
                 .Where(filename => Path.GetFileName(filename)
                                        .StartsWith(map.Name, StringComparison.InvariantCultureIgnoreCase)))
        {
            File.Delete(scene);
        }
        RemoveMap(map);
        map.Downloaded = false;
        map.Downloadable = true;
    }
    private string _gamePath;
    private string _levelPath;
    public GameInfo GameInfo { get; } = new();
    public ObservableCollection<Map> LocalMaps { get; } = [];
    private readonly Dictionary<string,Map> _localMapsDictionary = [];

}