using Avalonia.Interactivity;
using FluentAvalonia.UI.Controls;
using GOILauncher.Helpers;
using GOILauncher.Models;
using LeanCloud;
using LeanCloud.Storage;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GOILauncher.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            if (File.Exists($"{System.AppDomain.CurrentDomain.BaseDirectory}Settings.json"))
            {
                Setting.Instance = StorageHelper.LoadJSON<Setting>(System.AppDomain.CurrentDomain.BaseDirectory, "Settings.json");
            }
            LCApplication.Initialize("3Dec7Zyj4zLNDU0XukGcAYEk-gzGzoHsz", "uHF3AdKD4i3RqZB7w1APiFRF", "https://3dec7zyj.lc-cn-n1-shared.com", null);
            //LCLogger.LogDelegate = (LCLogLevel level, string info) =>
            //{
            //    switch (level)
            //    {
            //        case LCLogLevel.Debug:
            //            Trace.WriteLine($"[DEBUG] {DateTime.Now} {info}\n");
            //            break;
            //        case LCLogLevel.Warn:
            //            Trace.WriteLine($"[WARNING] {DateTime.Now} {info}\n");
            //            break;
            //        case LCLogLevel.Error:
            //            Trace.WriteLine($"[ERROR] {DateTime.Now} {info}\n");
            //            break;
            //        default:
            //            Trace.WriteLine(info);
            //            break;
            //    }
            //};
            Views = new ObservableCollection<ViewTemplate>
            {
              new ViewTemplate(typeof(GameViewModel), "游戏"),
             new ViewTemplate(typeof(ModViewModel), "Mod"),
            new ViewTemplate(typeof(MapViewModel), "地图")
            };
            FooterViews = new ObservableCollection<ViewTemplate>
        {
            new ViewTemplate(typeof(AboutViewModel), "关于"),
            new ViewTemplate(typeof(SettingViewModel), "设置"),
        };
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

        public ObservableCollection<ViewTemplate> Views { get; } 

        private ViewModelBase currentView;

        private ViewTemplate? selectedView;

        public ObservableCollection<ViewTemplate> FooterViews { get; }
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


