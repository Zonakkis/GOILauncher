using FluentAvalonia.UI.Controls;
using GOILauncher.Helpers;
using GOILauncher.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace GOILauncher.ViewModels
{
    internal class MapManageViewModel : ViewModelBase
    {
        public MapManageViewModel()
        {
            var isDownloadValid = this.WhenAnyValue(x => x.SelectedCount,
                                                    x => x > 0);
            DeleteCommand = ReactiveCommand.Create(DeleteMaps, this.WhenAnyValue(x => x.SelectedCount, x => x > 0));
        }

        public override void Init()
        {

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
        private void GetMaps()
        {
            Maps = new ObservableCollection<Map>();
            SelectedCount = 0;
            if (Directory.Exists(Setting.Instance.LevelPath))
            {
                string[] s = Directory.GetFiles(Setting.Instance.LevelPath, "*.scene", SearchOption.AllDirectories);
                foreach (string file in Directory.GetFiles(Setting.Instance.LevelPath, "*.scene", SearchOption.AllDirectories))
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
                            map.Size = StorageUnitConvertHelper.ByteTo(fileInfo.Length);
                            map.OnMapSeletcedChangedEvent += OnMapSelectedChanged;
                            Maps.Add(map);
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        var map = new Map(mapName);
                        var fileInfo = new FileInfo(file);
                        map.Size = StorageUnitConvertHelper.ByteTo(fileInfo.Length);
                        map.OnMapSeletcedChangedEvent += OnMapSelectedChanged;
                        Maps.Add(map);
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

        [Reactive]
        public int TotalCount { get; set; }
        [Reactive]
        public int SelectedCount { get; set; }
        public ReactiveCommand<Unit, Unit> DeleteCommand { get; set; }

        public class Map
        {
            public Map(string name)
            {
                Name = name;
            }
            public string Name { get; set; }
            public string? Author { get; set; }
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
                foreach(var path in Directory.EnumerateDirectories($"{Setting.Instance.LevelPath}/").Where(directory => Path.GetFileName(directory).StartsWith(Name, StringComparison.InvariantCultureIgnoreCase)))
                {
                    Directory.Delete(path,true);
                }
                foreach (var path in Directory.GetFiles($"{Setting.Instance.LevelPath}/").Where(filename => Path.GetFileName(filename).StartsWith(Name,StringComparison.InvariantCultureIgnoreCase)))
                {
                    File.Delete(path);
                }
            }

            public delegate void OnMapSelectedChanged(bool selected);

            public event OnMapSelectedChanged OnMapSeletcedChangedEvent;
        }
    }
}
