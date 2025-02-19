using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using GOILauncher.Helpers;
using GOILauncher.Models;
using GOILauncher.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using System.Xml.Linq;
using GOILauncher.UI;

namespace GOILauncher.ViewModels.Pages
{
    public class MapManagePageViewModel : PageViewModelBase
    {
        public MapManagePageViewModel(GameService gameService)
        {
            _gameService = gameService;
            Maps = gameService.LocalMaps;
            DeleteCommand = ReactiveCommand.Create<IList>(DeleteSelectedMaps);
        }
        public override void Init()
        {
        }
        public override void OnSelectedViewChanged()
        {
        }

        private void DeleteSelectedMaps(IList maps)
        {
            NotificationManager.ShowContentDialog("提示", $"将删除{maps.Count}个地图",
                ReactiveCommand.Create(() =>
            {
                List<Map> mapsToDelete = [];
                mapsToDelete.AddRange(maps.Cast<Map>());
                foreach (var map in mapsToDelete)
                {
                    _gameService.DeleteMap(map);
                }
            }));
        }
        private readonly GameService _gameService;
        public ObservableCollection<Map> Maps { get; set; }
        public ReactiveCommand<IList, Unit> DeleteCommand { get; set; }
    }
}
