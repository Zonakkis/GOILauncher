using FluentAvalonia.UI.Controls;
using GOILauncher.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace GOILauncher.ViewModels
{
    internal class MapManageViewModel : ViewModelBase
    {
        public MapManageViewModel()
        {
            var isDownloadValid = this.WhenAnyValue(x => x.SelectedCount,
                                                    x => x > 0);
            DeleteCommand = ReactiveCommand.Create(DeleteMaps, this.WhenAnyValue(x => x.SelectedCount, x => x > 0));
            //DeleteCommand = ReactiveCommand.Create(DeleteMaps);
        }
        public override void OnSelectedViewChanged()
        {
            Task.Run(GetMaps);
        }

        public void OnMapSelectedChanged(bool selected)
        {
            if (selected)
            {
                SelectedCount++;
            }
            else
            {
                SelectedCount--;
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
            Maps = new ObservableCollection<Map>();
            SelectedCount = 0;
            if (Directory.Exists(Setting.Instance.levelPath))
            {
                foreach (string file in Directory.GetFiles(Setting.Instance.levelPath))
                {
                    if (file.EndsWith(".scene"))
                    {
                        string mapName = Path.GetFileNameWithoutExtension(file);
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
                                var fileInfo = new FileInfo(file);
                                map.Size = ConvertStorageUnit(fileInfo.Length);
                                map.OnMapSeletcedChangedEvent += OnMapSelectedChanged;
                                Maps.Add(map);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                TotalCount = Maps.Count;
                this.RaisePropertyChanged("Maps");
            }
        }
        public async void DeleteMaps()
        {
            var contentDialog = new ContentDialog()
            {
                Title = "确定要删除吗？",
                Content = $"将删除{SelectedCount}个地图",
                PrimaryButtonText = "确定",
                CloseButtonText = "取消",
            };
            contentDialog.PrimaryButtonCommand = ReactiveCommand.Create(() =>
            {
                foreach (var map in Maps.Where(map => map.IsSelected == true))
                {
                    map.Delete();
                }
                GetMaps();
            });
            await contentDialog.ShowAsync();
            
        }
        public ObservableCollection<Map> Maps { get; set; } = new ObservableCollection<Map>();

        private int totalCount;
        public int TotalCount
        {
            get => totalCount;
            set
            {
                this.RaiseAndSetIfChanged(ref totalCount, value, "TotalCount");
            }
        }

        private int selectedCount;
        public int SelectedCount
        {
            get => selectedCount;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedCount, value, "SelectedCount");
            }
        }
        public ReactiveCommand<Unit, Unit> DeleteCommand { get; }

        public class Map
        {
            public Map(string name)
            {
                Name = name;
            }
            public string Name { get; set; }
            public string Author { get; set; }
            public string Size { get; set; }

            private bool isSelected;
            public bool IsSelected 
            { 
                get => isSelected; 
                set
                {
                    isSelected = value;
                    OnMapSeletcedChangedEvent(isSelected);
                }

            }

            public void Delete()
            {
                foreach(var path in Directory.GetFiles($"{Setting.Instance.levelPath}/").Where(filename => filename.Contains(Name)))
                {
                    File.Delete(path);
                }
            }

            public delegate void OnMapSelectedChanged(bool selected);

            public event OnMapSelectedChanged OnMapSeletcedChangedEvent;
        }
    }
}
