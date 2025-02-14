using Avalonia.Threading;
using GOILauncher.UI;
using LeanCloud;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using FluentAvalonia.UI.Controls;
using GOILauncher.ViewModels.Pages;

namespace GOILauncher.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel(
            GamePageViewModel gamePageViewModel, ModPageViewModel modPageViewModel,MapPageViewModel mapPageViewModel,
            MapManagePageViewModel mapManagePageViewModel,LeaderBoardPageViewModel leaderBoardPageViewModel,
            SubmitSpeedrunPageViewModel submitSpeedrunPageViewModel,PendingPageViewModel pendingPageViewModel,
            SettingPageViewModel settingPageViewModel,AboutPageViewModel aboutPageViewModel,
            NotificationManager notificationManager)
        { 
            NotificationManager = notificationManager;
            Views = [
                new Page("游戏", gamePageViewModel),
                new Page("Mod", modPageViewModel),
                //new Page("Mod",new ObservableCollection<Page>()
                //{
                //    new Page("Modpack配置"),
                //}),
                new Page("地图", mapPageViewModel, [
                    new Page("管理地图", mapManagePageViewModel)
                ]),
                new Page("排行榜", leaderBoardPageViewModel, [
                    new Page("提交速通", submitSpeedrunPageViewModel),
                    new Page("待审核", pendingPageViewModel),
                ])
            ];
            FooterViews = [
                new Page("设置", settingPageViewModel),
                new Page("关于", aboutPageViewModel)
            ];
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
        public NotificationManager NotificationManager {  get; }
        public static HttpClient HttpClient { get; } = new(new HttpClientHandler() { AllowAutoRedirect = false });
        [Reactive]
        public PageViewModelBase CurrentView { get; set; }
        public Page SelectedPage
        {
            get => selectedView!;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedView, value);
                OnSelectedPageChanged(value);
            }
        }

        public ObservableCollection<Page> Views { get; }

        private Page? selectedView;
        public ObservableCollection<Page> FooterViews { get; }
    }
}


