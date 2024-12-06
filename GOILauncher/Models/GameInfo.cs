using GOILauncher.Helpers;
using System;
using System.IO;
using System.Reflection;

namespace GOILauncher.Models
{
    public class GameInfo
    {
        public event Action? Refreshed;
        public void Refresh(string gamePath)
        {
            GetGameVersion(new FileInfo($"{gamePath}/GettingOverIt.exe").Length);
            GetModpackandLevelLoaderVersion(gamePath);
            GetBepinExVersion(gamePath);
            Refreshed?.Invoke();
        }
        public static Assembly LoadAssembly(string path)
        {
            byte[]? assemblyData;
            using (var fileStream = File.OpenRead(path))
            {
                using BinaryReader binaryReader = new(fileStream);
                assemblyData = binaryReader.ReadBytes((int)fileStream.Length);
            }
            return Assembly.Load(assemblyData);
        }
        public void GetGameVersion(long exeSize)
        {
            switch (exeSize)
            {
                case 653824:
                    GameVersion = "1.6/1.7";
                    break;
                case 650752:
                    GameVersion = "1.599";
                    break;
                case 639488:
                    GameVersion = "1.59/1.5861/1.584";
                    break;
                case 18687488:
                    GameVersion = "1.5762/1.54";
                    break;
                 case 18678272:
                    GameVersion = "1.53";
                    break;
                default:
                    Console.WriteLine(exeSize);
                    GameVersion = "未知版本";
                    break;
            }
        }
        public void GetModpackandLevelLoaderVersion(string gamePath)
        {
            try
            {
                var assembly = LoadAssembly($"{gamePath}/GettingOverIt_Data/Managed/Assembly-CSharp.dll");
                var modPackType = assembly.GetType("JsonConvertEx");
                var levelLoaderType = assembly.GetType("GOILevelImporter.ModInfo");
                ModpackVersion = modPackType != null ? "已安装" : "未安装";
                if (levelLoaderType != null)
                {
                    LevelLoaderVersion = (string)levelLoaderType.GetProperty("FULLVERSION")!.GetValue(typeof(string), null)!;
                }
                else
                {
                    LevelLoaderVersion = "未安装";
                }
            }
            catch(Exception ex)
            {
                _ = NotificationHelper.ShowNotification("获取Modpack版本失败", ex.Message, FluentAvalonia.UI.Controls.InfoBarSeverity.Error);
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
        public void GetBepinExVersion(string gamePath)
        {
            if (!Directory.Exists($"{gamePath}/BepInEx/core"))
            {
                BepInExVersion = "未安装";
            }
            else if (File.Exists($"{gamePath}/BepInEx/core/BepInEx.Preloader.Unity.dll"))
            {
                var assembly = LoadAssembly($"{gamePath}/BepInEx/core/BepInEx.Preloader.Unity.dll");
                var assemblyInformationalVersionAttribute = (AssemblyInformationalVersionAttribute)assembly.GetCustomAttribute(typeof(AssemblyInformationalVersionAttribute))!;
                BepInExVersion = assemblyInformationalVersionAttribute.InformationalVersion;
            }
            else
            {
                var assembly = LoadAssembly($"{gamePath}/BepInEx/core/BepInEx.Preloader.dll");
                var assemblyInformationalVersionAttribute = ((AssemblyFileVersionAttribute)assembly.GetCustomAttribute(typeof(AssemblyFileVersionAttribute))!);
                BepInExVersion =  assemblyInformationalVersionAttribute.Version;
            }
        }
        public string GameVersion { get; private set; } = string.Empty;
        public string ModpackVersion { get; private set; } = string.Empty;
        public string LevelLoaderVersion { get; private set; } = string.Empty;
        public string BepInExVersion { get; private set; } = string.Empty;

        public static readonly GameInfo Instance = new();
    }
}
