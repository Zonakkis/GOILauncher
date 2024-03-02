using GOILauncher.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace GOILauncher.ViewModels
{
    internal class MapManageViewModel : ViewModelBase
    {
        public MapManageViewModel()
        {

        }

        public override void OnSelectedViewChanged()
        {
            if (!SelectedGamePathNoteHide && Setting.Instance.gamePath != "未选择")
            {
                SelectedGamePathNoteHide = true;
                GetMaps();

            }
        }
        public string ConvertStorageUnit(long bytes)
        {
            if (bytes < 1024)
            {
                return $"{bytes.ToString("0.00")} B";
            }
            else if (bytes < 1048576)
            {
                return $"{(bytes / 1024D).ToString("0.00")} KB";
            }
            else
            {
                return $"{(bytes / 1048576D).ToString("0.00")} MB";
            }
        }
        private void GetMaps()
        {
            if (Directory.Exists(Setting.Instance.levelPath))
            {
                foreach (string file in Directory.GetFiles(Setting.Instance.levelPath))
                {
                    if (file.EndsWith(".scene"))
                    {
                        string mapName = Path.GetFileNameWithoutExtension(file);
                        string empty = string.Empty;
                        if (File.Exists(Path.ChangeExtension(file, "txt")) || File.Exists(Path.ChangeExtension(file, "mdata")))
                        {
                            try
                            {
                                Dictionary<string, string> settings = (from l in File.ReadAllLines(Path.ChangeExtension(file, File.Exists(Path.ChangeExtension(file, "txt")) ? "txt" : "mdata"))
                                              select l.Split(new char[] { '=' })).ToDictionary((string[] s) => s[0].Trim(), (string[] s) => s[1].Trim());
                                var map = new Map(mapName);
                                if (settings.ContainsKey("credit"))
                                {
                                    map.Author = settings["credit"];
                                }
                                map.Size = ConvertStorageUnit(new FileInfo(file).Length);
                                Maps.Add(map);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                this.RaisePropertyChanged("Maps");
            }
        }
        public ObservableCollection<Map> Maps { get; } = new ObservableCollection<Map>();

        private bool selectedGamePathNoteHide;
        public bool SelectedGamePathNoteHide
        {
            get => selectedGamePathNoteHide; set
            {
                this.RaiseAndSetIfChanged(ref selectedGamePathNoteHide, value, "SelectedGamePathNoteHide");
            }
        }


        public class Map
        {
            public Map(string name)
            {
                Name = name;
            }
            public string Name { get; set; }
            public string Author { get; set; }
            public string Size { get; set; }
            public bool IsSelected { get; set; }

        }
    }
}
