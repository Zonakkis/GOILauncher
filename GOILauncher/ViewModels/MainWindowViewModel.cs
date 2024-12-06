using LeanCloud;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;

namespace GOILauncher.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            LCApplication.Initialize("3Dec7Zyj4zLNDU0XukGcAYEk-gzGzoHsz", "uHF3AdKD4i3RqZB7w1APiFRF", "https://3dec7zyj.lc-cn-n1-shared.com");
#if DEBUG
            EnableLeanCloudLog();
#endif
            CurrentView = Views[0].View;
            SelectedPage = Views[0];
        }
#if DEBUG
        private static void EnableLeanCloudLog()
        {
            LCLogger.LogDelegate = (level, info) =>
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
        }
#endif
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
        public static HttpClient HttpClient { get; } = new(new HttpClientHandler() { AllowAutoRedirect = false });
        [Reactive]
        public ViewModelBase CurrentView { get; set; }
        public Page SelectedPage
        {
            get => selectedView!;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedView, value);
                OnSelectedPageChanged(value);
            }
        }

        public ObservableCollection<Page> Views { get; } = [
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

        private Page? selectedView;
        public ObservableCollection<Page> FooterViews { get; } = [
                new(typeof(AboutViewModel), "关于"),
                new(typeof(SettingViewModel), "设置"),
            ];
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


