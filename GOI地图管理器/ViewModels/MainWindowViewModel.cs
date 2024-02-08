using GOI地图管理器.Helpers;
using GOI地图管理器.Models;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace GOI地图管理器.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {

        }
        public void ChangePane()
        {
            IsPaneOpen = !IsPaneOpen;
            Trace.WriteLine(IsPaneOpen);
        }
        public void OnSelectedListItemChanged(ListItemTemplate value)
        {
            CurrentPage = value.View;
            CurrentPage.OnSelectedViewModelChanged();

        }
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _description, value, "Description");
            }
        }

        public bool IsLoading
        {
            get
            {
                return true;
            }
        }

        public bool IsReady
        {
            get
            {
                return true;
            }
        }

        public bool IsPaneOpen
        {
            get
            {
                return _isPaneOpen;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _isPaneOpen, value, "IsPaneOpen");
            }
        }

        public ViewModelBase CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _currentPage, value, "CurrentPage");
            }
        }

        public ListItemTemplate SelectedListItem
        {
            get
            {
                return _selectedListItem!;
            }
            set
            {
                OnSelectedListItemChanged(value);
                this.RaiseAndSetIfChanged(ref _selectedListItem, value, "SelectedListItem");
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
            ModelType = type;
            Label = name;
            View = (ViewModelBase)Activator.CreateInstance(ModelType)!;
        }

        public string Label { get; }

        public Type ModelType { get; }

        public ViewModelBase View { get; }
    }
}


