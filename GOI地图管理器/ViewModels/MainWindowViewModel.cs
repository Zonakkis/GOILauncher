﻿using FluentAvalonia.UI.Controls;
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
            if (File.Exists($"{System.AppDomain.CurrentDomain.BaseDirectory}Settings.json"))
            {
                Setting.Instance = StorageHelper.LoadJSON<Setting>(System.AppDomain.CurrentDomain.BaseDirectory, "Settings.json");
            }
            SelectedView = Views[0];
        }
        public void OnSelectedViewChanged(ViewTemplate value)
        {
            value.View.OnSelectedViewChanged();
            CurrentView = value.View;

        }

        public ViewModelBase CurrentView
        {
            get
            {
                return currentView;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref currentView, value, "CurrentView");
            }
        }

        public ViewTemplate SelectedView
        {
            get
            {
                return selectedView!;
            }
            set
            {
                OnSelectedViewChanged(value);
                this.RaiseAndSetIfChanged(ref selectedView, value, "SelectedView");
            }
        }

        public ObservableCollection<ViewTemplate> Views { get; } = new ObservableCollection<ViewTemplate>
        {
            new ViewTemplate(typeof(GameViewModel), "游戏"),
            new ViewTemplate(typeof(ModViewModel), "Mod"),
            new ViewTemplate(typeof(MapViewModel), "地图")
        };

        private ViewModelBase currentView;

        private ViewTemplate? selectedView;

        public ObservableCollection<ViewTemplate> FooterViews { get; } = new ObservableCollection<ViewTemplate>
        {
            new ViewTemplate(typeof(AboutViewModel), "关于"),
            new ViewTemplate(typeof(SettingViewModel), "设置"),
        };
    }

    public class ViewTemplate
    {
        public ViewTemplate(Type type, string name)
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


