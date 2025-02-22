using GOILauncher.UI;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Net.Http;
using GOILauncher.ViewModels.Pages;
using GOILauncher.Services.LeanCloud;
using System.Diagnostics;
using System;
using System.Threading.Tasks;
using GOILauncher.Models;
using Version = GOILauncher.Models.Version;
using GOILauncher.Services;

namespace GOILauncher.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel(
            GamePageViewModel gamePageViewModel, ModPageViewModel modPageViewModel,MapPageViewModel mapPageViewModel,
            MapManagePageViewModel mapManagePageViewModel,LeaderBoardPageViewModel leaderBoardPageViewModel,
            SubmitSpeedrunPageViewModel submitSpeedrunPageViewModel,PendingPageViewModel pendingPageViewModel,
            SettingPageViewModel settingPageViewModel,AboutPageViewModel aboutPageViewModel,
            NotificationManager notificationManager,LeanCloudService leanCloudService,AppService appService)
        {
            NotificationManager = notificationManager;
            _leanCloudService = leanCloudService;
            _version = appService.Version;
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
            CurrentView = Views[0].View;
            SelectedPage = Views[0];
            Task.Run(CheckUpdate);
        }

        private async Task CheckUpdate()
        {
            var update = await _leanCloudService.Get<Update>("67b8c0b2d2c78e5c0084c98e");
            var newVersion = new Version(update.Version);
            if (newVersion > _version)
            {
                NotificationManager.ShowContentDialog("有新版本！！",
                    $"{_version} -> {newVersion}\r\n{update.Changelog}",
                    "手动更新", "自动更新",
                    ReactiveCommand.Create(() =>
                    {
                        Process.Start("explorer.exe", "https://github.com/Zonakkis/GOILauncher/releases");
                        Environment.Exit(0);
                    }),
                    ReactiveCommand.Create(() =>
                    {
                        Process.Start("GOILUpdater.exe", update.Url);
                        Environment.Exit(0);
                    }));
            }
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
        public NotificationManager NotificationManager { get; }
        private readonly LeanCloudService _leanCloudService;
        private readonly Version _version;
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