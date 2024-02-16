using Avalonia.Controls.Shapes;
using Avalonia.OpenGL;
using GOI地图管理器.Helpers;
using GOI地图管理器.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace GOI地图管理器.ViewModels
{
    internal class GameViewModel : ViewModelBase
    {
        public GameViewModel()
        {
            
        }
        public override void OnSelectedViewChanged()
        {
            GamePath = Setting.Instance.gamePath;
        }

        public Assembly LoadAssembly(string path)
        {
            byte[] assemblyData = null;
            using (FileStream fileStream = File.OpenRead(path))
            {
                using (BinaryReader binaryReader = new BinaryReader(fileStream))
                {
                    assemblyData = binaryReader.ReadBytes((int)fileStream.Length);
                }
            }
            return Assembly.Load(assemblyData);
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
        void GetModpackandLevelLoaderVersion(string gamepath)
        {
            Assembly assembly = LoadAssembly($"{gamepath}/GettingOverIt_Data/Managed/Assembly-CSharp.dll");
            Type modPackType = assembly.GetType("JsonConvertEx");
            Type levelLoaderType = assembly.GetType("GOILevelImporter.ModInfo");
            if(modPackType != null)
            {
                ModpackVersion = "已安装";
            }
            else
            {
                ModpackVersion = "未安装";
            }
            if(levelLoaderType != null)
            {
                LevelLoaderVersion = (string)levelLoaderType.GetProperty("FULLVERSION")!.GetValue(typeof(string), null)!;
            }
            else
            {
                LevelLoaderVersion = "未安装";
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
        string GetBepinExVersion(string gamepath)
        {
            if(!Directory.Exists($"{gamepath}/BepInEx/core"))
            {
                return "未安装";
            }
            else if(File.Exists($"{gamepath}/BepInEx/core/BepInEx.Preloader.Unity.dll"))
            {
                Assembly assembly = LoadAssembly($"{gamepath}/BepInEx/core/BepInEx.Preloader.Unity.dll");
                AssemblyInformationalVersionAttribute assemblyInformationalVersionAttribute = ((AssemblyInformationalVersionAttribute)assembly.GetCustomAttribute(typeof(AssemblyInformationalVersionAttribute)));
                return assemblyInformationalVersionAttribute.InformationalVersion;

            }
            else
            {
                Assembly assembly = LoadAssembly($"{gamepath}/BepInEx/core/BepInEx.Preloader.dll");
                AssemblyInformationalVersionAttribute assemblyInformationalVersionAttribute = ((AssemblyInformationalVersionAttribute)assembly.GetCustomAttribute(typeof(AssemblyInformationalVersionAttribute)));
                return assemblyInformationalVersionAttribute.InformationalVersion;
            }
        }
        public void LaunchGOI(int para)
        {
            switch(para)
            {
                case 1:
                    Process.Start($"{GamePath}/GettingOverIt.exe");
                    return;
                case 2:
                    if (SteamPath != "未选择（需要通过Steam启动游戏时才选择，否则可不选）")
                    {
                        Process.Start($"{SteamPath}/steam.exe","steam://rungameid/240720");
                    }
                    return;
            }
        }

        public string gamePath = "未选择";
        public string GamePath
        {
            get => gamePath;
            set
            {
                if (gamePath != value)
                {
                    this.RaiseAndSetIfChanged(ref gamePath, value, "GamePath");
                    SelectedGamePathNoteHide = true;
                    GameVersion = GetGameVersion(new FileInfo($"{GamePath}/GettingOverIt.exe").Length);
                    GetModpackandLevelLoaderVersion(GamePath);
                    BepInExVersion = GetBepinExVersion(GamePath);

                }
            }
        }
        public string SteamPath { get => Setting.Instance.steamPath; }

        private bool selectedGamePathNoteHide;
        public bool SelectedGamePathNoteHide
        {
            get => selectedGamePathNoteHide; set
            {
                this.RaiseAndSetIfChanged(ref selectedGamePathNoteHide, value, "UnSelectedNoteHide");
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

        string bepInExVersion;
        public string BepInExVersion
        {
            get => bepInExVersion; set
            {
                this.RaiseAndSetIfChanged(ref bepInExVersion, value, "BepInExVersion");
            }
        }
    }





}
