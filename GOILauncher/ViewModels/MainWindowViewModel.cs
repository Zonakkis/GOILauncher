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
                Setting.Load(System.AppDomain.CurrentDomain.BaseDirectory);
            }
            LCApplication.Initialize("3Dec7Zyj4zLNDU0XukGcAYEk-gzGzoHsz", "uHF3AdKD4i3RqZB7w1APiFRF", "https://3dec7zyj.lc-cn-n1-shared.com", null);
            LCLogger.LogDelegate = (LCLogLevel level, string info) =>
            {
                switch (level)
                {
                    case LCLogLevel.Debug:
                        Trace.WriteLine($"[DEBUG] {DateTime.Now} {info}\n");
                        break;
                    case LCLogLevel.Warn:
                        Trace.WriteLine($"[WARNING] {DateTime.Now} {info}\n");
                        break;
                    case LCLogLevel.Error:
                        Trace.WriteLine($"[ERROR] {DateTime.Now} {info}\n");
                        break;
                    default:
                        Trace.WriteLine(info);
                        break;
                }
            };
            Views = [
                new(typeof(GameViewModel), "游戏"),
                new(typeof(ModViewModel), "Mod"),
                //new Page(typeof(ModViewModel), "Mod",new ObservableCollection<Page>()
                //{
                //    new Page(typeof(ModpackManageViewModel), "Modpack配置"),
                //}),
                //new Page(typeof(MapViewModel), "地图"),
                new(typeof(MapViewModel), "地图",[

                    new(typeof(MapManageViewModel), "管理地图"),
                ]),
                new(typeof(LeaderBoardViewModel),"排行榜",[
                    new(typeof(SubmitSpeedrunViewModel), "提交速通"),
                    new(typeof(PendingViewModel), "待审核"),
                ])
            ];
            FooterViews = [
                new Page(typeof(AboutViewModel), "关于"),
                new Page(typeof(SettingViewModel), "设置"),
            ];
            SelectedPage = Views[0];
        }


        public void OnSelectedPageChanged(Page value)
        {
            CurrentView = value.View;
            if(!CurrentView.Initialized)
            {
                CurrentView.Init();
                CurrentView.Initialized = true;
            }
            value.View.OnSelectedViewChanged();
        }

        public ViewModelBase CurrentView
        {
            get
            {
                return currentView;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref currentView, value, nameof(CurrentView));
            }
        }

        public Page SelectedPage
        {
            get
            {
                return selectedView!;
            }
            set
            {
                OnSelectedPageChanged(value);
                this.RaiseAndSetIfChanged(ref selectedView, value, "SelectedView");
            }
        }

        public ObservableCollection<Page> Views { get; }

        private ViewModelBase currentView;

        private Page? selectedView;
        public ObservableCollection<Page> FooterViews { get; }
    }
        public class Page
    {
        public Page(Type type, string name,ObservableCollection<Page>? subPages = null)
        {
            ModelType = type;
            Label = name;
            View = (ViewModelBase)Activator.CreateInstance(ModelType)!;
            SubPages = subPages;
        }

        public string Label { get; }
        public Type ModelType { get; }
        public ViewModelBase View { get; }
        public ObservableCollection<Page>? SubPages { get; }
    }


}


