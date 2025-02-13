using FluentAvalonia.UI.Controls;
using GOILauncher.Helpers;
using GOILauncher.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Xml.Linq;

namespace GOILauncher.ViewModels
{
    public class MapManageViewModel : ViewModelBase
    {
        public MapManageViewModel(SettingViewModel settingViewModel)
        {
            _settingViewModel = settingViewModel;
            DeleteCommand = ReactiveCommand.Create(DeleteSelectedMaps, this.WhenAnyValue(x => x.SelectedCount, x => x > 0));
        }

        public override void Init()
        {

        }
        public override void OnSelectedViewChanged()
        {
            GetMaps();
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
            Maps.Clear();
            SelectedCount = 0;
            if (Directory.Exists(LevelPath))
            {
                foreach (var file in Directory.GetFiles(LevelPath, "*.scene", SearchOption.AllDirectories))
                {
                    var mapName = Path.GetFileNameWithoutExtension(file);
                    if (File.Exists(Path.ChangeExtension(file, "txt")) || File.Exists(Path.ChangeExtension(file, "mdata")))
                    {
                        try
                        {
                            var settings = (from l in File.ReadAllLines(Path.ChangeExtension(file, File.Exists(Path.ChangeExtension(file, "txt")) ? "txt" : "mdata"))
                                                                   select l.Split(['='])).ToDictionary(s => s[0].Trim(), s => s[1].Trim());
                            var map = new MapInfo(mapName);
                            if (settings.TryGetValue("credit", out var value))
                            {
                                map.Author = value;
                            }
                            var fileInfo = new FileInfo(file);
                            map.Size = StorageUnitConvertHelper.ByteTo(fileInfo.Length);
                            map.OnMapSeletcedChangedEvent += OnMapSelectedChanged;
                            Maps.Add(map);
                        }
                        catch(Exception ex)
                        {
                            _ = NotificationHelper.ShowNotification("错误",$"扫描地图时出错：{ex.Message}",InfoBarSeverity.Error);
                        }
                    }
                    else
                    {
                        var map = new MapInfo(mapName);
                        var fileInfo = new FileInfo(file);
                        map.Size = StorageUnitConvertHelper.ByteTo(fileInfo.Length);
                        map.OnMapSeletcedChangedEvent += OnMapSelectedChanged;
                        Maps.Add(map);
                    }
                }
                TotalCount = Maps.Count;
            }
        }
        public void Delete(MapInfo map)
        {
            foreach (var path in Directory.EnumerateDirectories($"{LevelPath}/")
                                .Where(directory => Path.GetFileName(directory)
                                .StartsWith(map.Name, StringComparison.InvariantCultureIgnoreCase)))
            {
                Directory.Delete(path, true);
            }
            foreach (var path in Directory.GetFiles($"{LevelPath}/")
                .Where(filename => Path.GetFileName(filename)
                .StartsWith(map.Name, StringComparison.InvariantCultureIgnoreCase)))
            {
                File.Delete(path);
            }
        }
        public async void DeleteSelectedMaps()
        {
            var contentDialog = new ContentDialog
            {
                Title = "确定要删除吗？",
                Content = $"将删除{SelectedCount}个地图",
                PrimaryButtonText = "确定",
                CloseButtonText = "取消",
                PrimaryButtonCommand = ReactiveCommand.Create(() =>
                {
                    foreach (var map in Maps.Where(map => map.IsSelected))
                    {
                        Delete(map);
                    }
                    GetMaps();
                })
            };
            await contentDialog.ShowAsync();
            
        }
        private readonly SettingViewModel _settingViewModel;
        private string? LevelPath => _settingViewModel.LevelPath;
        public ObservableCollection<MapInfo> Maps { get; set; } = [];

        [Reactive]
        public int TotalCount { get; set; }
        [Reactive]
        public int SelectedCount { get; set; }
        public ReactiveCommand<Unit, Unit> DeleteCommand { get; set; }
    }
    public class MapInfo(string name)
    {
        public string Name { get; set; } = name;
        public string Author { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;

        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                OnMapSeletcedChangedEvent?.Invoke(isSelected);
            }

        }

        public delegate void OnMapSelectedChanged(bool selected);

        public event OnMapSelectedChanged? OnMapSeletcedChangedEvent;
    }
}
