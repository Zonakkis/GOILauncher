using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace GOI地图管理器.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public void ChangePane()
        {
            this.IsPaneOpen = !this.IsPaneOpen;
            Trace.WriteLine(this.IsPaneOpen);
        }
        public void OnSelectedListItemChanged(ListItemTemplate value)
        {
            this.CurrentPage = value.View;
        }
        public string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref this._description, value, "Description");
            }
        }

        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000031 RID: 49 RVA: 0x00003537 File Offset: 0x00001737
        public string Greeting
        {
            get
            {
                return "Welcome to Avalonia!";
            }
        }

        // Token: 0x17000003 RID: 3
        // (get) Token: 0x06000032 RID: 50 RVA: 0x0000353E File Offset: 0x0000173E
        public bool IsLoading
        {
            get
            {
                return true;
            }
        }

        // Token: 0x17000004 RID: 4
        // (get) Token: 0x06000033 RID: 51 RVA: 0x0000353E File Offset: 0x0000173E
        public bool IsReady
        {
            get
            {
                return true;
            }
        }

        // Token: 0x17000005 RID: 5
        // (get) Token: 0x06000034 RID: 52 RVA: 0x00003541 File Offset: 0x00001741
        // (set) Token: 0x06000035 RID: 53 RVA: 0x00003549 File Offset: 0x00001749
        public bool IsPaneOpen
        {
            get
            {
                return this._isPaneOpen;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref this._isPaneOpen, value, "IsPaneOpen");
            }
        }

        // Token: 0x17000006 RID: 6
        // (get) Token: 0x06000036 RID: 54 RVA: 0x0000355E File Offset: 0x0000175E
        // (set) Token: 0x06000037 RID: 55 RVA: 0x00003566 File Offset: 0x00001766
        public ViewModelBase CurrentPage
        {
            get
            {
                return this._currentPage;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref this._currentPage, value, "CurrentPage");
            }
        }

        public ListItemTemplate SelectedListItem
        {
            get
            {
                return this._selectedListItem;
            }
            set
            {
                this.OnSelectedListItemChanged(value);
                this.RaiseAndSetIfChanged(ref this._selectedListItem, value, "SelectedListItem");
            }
        }

        public ObservableCollection<ListItemTemplate> Items { get; } = new ObservableCollection<ListItemTemplate>
        {
            new ListItemTemplate(typeof(GameViewModel), "游戏"),
            new ListItemTemplate(typeof(ModViewModel), "Mod"),
            new ListItemTemplate(typeof(MapViewModel), "地图"),
            new ListItemTemplate(typeof(SettingViewModel), "设置")
        };

        private string _description = string.Empty;

        private bool _isPaneOpen;

        private ViewModelBase _currentPage = new GameViewModel();

        private ListItemTemplate? _selectedListItem;
    }

    public class ListItemTemplate
    {
        public ListItemTemplate(Type type, string name)
        {
            this.ModelType = type;
            this.Label = name;
            this.View = (ViewModelBase)Activator.CreateInstance(this.ModelType);
        }

        public string Label { get; }

        public Type ModelType { get; }

        public ViewModelBase View { get; }
    }
}


