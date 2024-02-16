using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GOILauncher.Models
{
    public class GameInfo: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

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
        public void GetGameVersion(long exeSize)
        {
            switch (exeSize)
            {
                case 653824:
                    GameVersion = "1.7";
                    break;
                default:
                    GameVersion = "未知版本";
                    break;
            }
        }
        public void GetModpackandLevelLoaderVersion(string gamepath)
        {
            Assembly assembly = LoadAssembly($"{gamepath}/GettingOverIt_Data/Managed/Assembly-CSharp.dll");
            Type modPackType = assembly.GetType("JsonConvertEx");
            Type levelLoaderType = assembly.GetType("GOILevelImporter.ModInfo");
            if (modPackType != null)
            {
                ModpackVersion = "已安装";
            }
            else
            {
                ModpackVersion = "未安装";
            }
            if (levelLoaderType != null)
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
        public void GetBepinExVersion(string gamepath)
        {
            if (!Directory.Exists($"{gamepath}/BepInEx/core"))
            {
                BepInExVersion = "未安装";
            }
            else if (File.Exists($"{gamepath}/BepInEx/core/BepInEx.Preloader.Unity.dll"))
            {
                Assembly assembly = LoadAssembly($"{gamepath}/BepInEx/core/BepInEx.Preloader.Unity.dll");
                AssemblyInformationalVersionAttribute assemblyInformationalVersionAttribute = ((AssemblyInformationalVersionAttribute)assembly.GetCustomAttribute(typeof(AssemblyInformationalVersionAttribute)));
                BepInExVersion = assemblyInformationalVersionAttribute!.InformationalVersion;

            }
            else
            {
                Assembly assembly = LoadAssembly($"{gamepath}/BepInEx/core/BepInEx.Preloader.dll");
                AssemblyInformationalVersionAttribute assemblyInformationalVersionAttribute = ((AssemblyInformationalVersionAttribute)assembly.GetCustomAttribute(typeof(AssemblyInformationalVersionAttribute)));
                BepInExVersion =  assemblyInformationalVersionAttribute!.InformationalVersion;
            }
        }

        private string gameVersion;
        public string GameVersion
        {
            get => gameVersion;
            set
            {
                gameVersion = value;
                NotifyPropertyChanged("GameVersion");
            }
        }

        private string modpackVersion;
        public string ModpackVersion
        {
            get => modpackVersion; 
            set
            {
                modpackVersion = value;
                NotifyPropertyChanged("ModpackVersion");

            }
        }

        private string levelLoaderVersion;
        public string LevelLoaderVersion
        {
            get => levelLoaderVersion; 
            set
            {
                levelLoaderVersion = value;
                NotifyPropertyChanged("LevelLoaderVersion");
            }
        }

        private string bepInExVersion;
        public string BepInExVersion
        {
            get => bepInExVersion; 
            set
            {
                bepInExVersion = value;
                NotifyPropertyChanged("BepInExVersion");
            }
        }

        public static GameInfo Instance = new GameInfo();
    }
}
