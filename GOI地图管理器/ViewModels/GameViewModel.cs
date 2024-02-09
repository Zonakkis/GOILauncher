﻿using GOI地图管理器.Helpers;
using GOI地图管理器.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GOI地图管理器.ViewModels
{
    internal class GameViewModel : ViewModelBase
    {
        public GameViewModel()
        {
            if (File.Exists($"{System.AppDomain.CurrentDomain.BaseDirectory}Settings.json"))
            {
                //Setting = StorageHelper.LoadJSON<Setting>(System.AppDomain.CurrentDomain.BaseDirectory, "Settings.json");
                Setting setting = StorageHelper.LoadJSON<Setting>(System.AppDomain.CurrentDomain.BaseDirectory, "Settings.json");
                GamePath = setting.gamePath;
                SteamPath = (setting.steamPath[0] != '未') ? setting.steamPath : "未选择";
                UnSelectedGamePathNoteHide = true;
                GameVersion = GetGameVersion(new FileInfo($"{GamePath}/GettingOverIt.exe").Length);
                ModpackVersion = GetModpackVersion(GamePath);
                LevelLoaderVersion = GetLevelLoaderVersion(GamePath);
                Trace.WriteLine(new FileInfo($"{GamePath}/GettingOverIt.exe").Length);
            }
            else
            {
                //Setting = new Setting();
                GamePath = "未选择";
                SteamPath = "未选择";
            }
            LaunchCommand = ReactiveCommand.Create<int>(LaunchGOI);
        }
        public override void OnSelectedViewModelChanged()
        {
            if (SteamPath == "未选择" && GamePath == "未选择" && File.Exists($"{System.AppDomain.CurrentDomain.BaseDirectory}Settings.json"))
            {
                Setting setting = StorageHelper.LoadJSON<Setting>(System.AppDomain.CurrentDomain.BaseDirectory, "Settings.json");
                GamePath = setting.gamePath;
                SteamPath = (setting.steamPath[0] != '未') ? setting.steamPath : "未选择";
                //Trace.WriteLine(new FileInfo($"{GamePath}/GettingOverIt.exe").Length);
                UnSelectedGamePathNoteHide = true;
                GameVersion = GetGameVersion(new FileInfo($"{GamePath}/GettingOverIt.exe").Length); 
                ModpackVersion = GetModpackVersion(GamePath);
                LevelLoaderVersion = GetLevelLoaderVersion(GamePath);
            }
        }

        string GetGameVersion(long exeSize)
        {
            switch(exeSize)
            {
                case 653824:
                    return "1.7";
                default:
                    return "未知版本";
            }
        }

        string GetModpackVersion(string gamepath)
        {
            Assembly assembly = Assembly.LoadFrom($"{gamepath}/GettingOverIt_Data/Managed/Assembly-CSharp.dll");
            if (assembly.GetType("JsonConvertEx") != null)
            {
                return "已安装";
            }
            else
            {
                return "未安装";
            }
            //尝试通过Modpack版本变化推断版本，但因为无法创建unity实例失败
            //Type type = assembly.GetType("ModPotCustomizer");
            //if (assembly.GetType("ModDioCustomizer") != null)
            //{
            //    return "2148";
            //}
            //else if((string)type.GetProperty("version").GetValue(Activator.CreateInstance(type)) == "1.1")
            //{
            //    return "2120";
            //}
            //else
            //{
            //    return "21??";
            //}

        }

        string GetLevelLoaderVersion(string gamepath)
        {
            Assembly assembly = Assembly.LoadFrom($"{gamepath}/GettingOverIt_Data/Managed/Assembly-CSharp.dll");
            Type type = assembly.GetType("GOILevelImporter.ModInfo");
            if (type != null)
            {
                return (string)type.GetProperty("FULLVERSION")!.GetValue(typeof(string), null)!;
            }
            else
            {
                return "未安装";
            }
        }

        public void LaunchGOI(int para)
        {
            switch(para)
            {
                case 1:
                    Process.Start($"{GamePath}/GettingOverIt.exe");
                    break;
                case 2:
                    if (SteamPath != "未选择")
                    {
                        Process.Start($"{SteamPath}/steam.exe","steam://rungameid/240720");
                    }
                    break;
            }
        }
        public string GamePath;
        public string SteamPath;

        private bool unSelectedGamePathNoteHide;

        public bool UnSelectedGamePathNoteHide
        {
            get => unSelectedGamePathNoteHide; set
            {
                this.RaiseAndSetIfChanged(ref unSelectedGamePathNoteHide, value, "UnSelectedNoteHide");
            }
        }

        string gameVersion;

        public string GameVersion
        {
            get => gameVersion; set
            {
                this.RaiseAndSetIfChanged(ref gameVersion, value, "GameVersion");
            }
        }

        string modpackVersion;

        public string ModpackVersion
        {
            get => modpackVersion; set
            {
                this.RaiseAndSetIfChanged(ref modpackVersion, value, "ModpackVersion");
            }
        }

        string levelLoaderVersion;

        public string LevelLoaderVersion
        {
            get => levelLoaderVersion; set
            {
                this.RaiseAndSetIfChanged(ref levelLoaderVersion, value, "LevelLoaderVersion");
            }
        }

        public ReactiveCommand<int, Unit> LaunchCommand { get; }
    }
}
