using GOI地图管理器.Models;
using LeanCloud;
using LeanCloud.Storage;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOI地图管理器.ViewModels
{
    internal class MapViewModel : ViewModelBase
    {
        
        public MapViewModel()
        {
            this.Maps = new ObservableCollection<Map>();
            LCApplication.Initialize("3Dec7Zyj4zLNDU0XukGcAYEk-gzGzoHsz", "uHF3AdKD4i3RqZB7w1APiFRF", "https://3dec7zyj.lc-cn-n1-shared.com", null);
            this.GetMaps();
        }
        public async void GetMaps()
        {
            LCQuery<LCObject> query = new LCQuery<LCObject>("Maps");
            query.OrderByAscending("Name");
            ReadOnlyCollection<LCObject> maps = await query.Find();
            foreach (LCObject map in maps)
            {
                this.Maps.Add(new Map(map));
            }
        }

        public void Download()
        {

        }

        public void OnSelectedListItemChanged(Map value)
        {
            if(!value.IsLoaded)
            {
                value.Load();
            }
            this.IsSelected = true;
            this.CurrentMap = value;
        }

        public Map CurrentMap
        {
            get
            {
                return this._currentMap;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref this._currentMap, value, "CurrentMap");
            }
        }

        private Map _currentMap;
        public bool IsSelected
        {
            get
            {
                return this._isSelected;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref this._isSelected, value, "IsSelected");
            }
        }

        private bool _isSelected;

        public ObservableCollection<Map> Maps { get; }
        public Map SelectedMap
        {
            get
            {
                return this._selectedMap;
            }
            set
            {
                this.OnSelectedListItemChanged(value);
                this.RaiseAndSetIfChanged(ref this._selectedMap, value, "SelectedMap");
            }
        }

        private Map _selectedMap;


    }
}
