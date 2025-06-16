using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using GOILauncher.ViewModels;
using GOILauncher.UI.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using Downloader;
using Avalonia.Controls.Notifications;
using GOILauncher.UI;
using GOILauncher.Services;
using GOILauncher.UI.Views.Pages;
using GOILauncher.ViewModels.Pages;
using System.Net.Http;
using GOILauncher.Infrastructures.LeanCloud;
using GOILauncher.Infrastructures.Interfaces;
using GOILauncher.Constants;

namespace GOILauncher
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
        public override void OnFrameworkInitializationCompleted()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            }
            base.OnFrameworkInitializationCompleted();
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<ILeanCloud, LeanCloud>(serviceProvider =>
            {
                return new LeanCloud(LeanCloudConnection.Url,
                    LeanCloudConnection.AppId, LeanCloudConnection.AppKey,
                    serviceProvider.GetRequiredService<HttpClient>());
            });
            services.AddSingleton<MainWindow>(serviceProvider => new MainWindow
            {
                DataContext = serviceProvider.GetRequiredService<MainWindowViewModel>()
            });
            services.AddTransient<GamePage>();
            services.AddSingleton<ModPage>();
            services.AddSingleton<MapPage>();
            services.AddTransient<MapManagePage>();
            services.AddTransient<LeaderBoardPage>();
            services.AddSingleton<SubmitSpeedrunPage>();
            services.AddTransient<PendingPage>();
            services.AddTransient<AboutPage>();
            services.AddTransient<SettingPage>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<GamePageViewModel>();
            services.AddSingleton<ModPageViewModel>();
            services.AddSingleton<MapPageViewModel>();
            services.AddSingleton<MapManagePageViewModel>();
            services.AddSingleton<LeaderBoardPageViewModel>();
            services.AddSingleton<SubmitSpeedrunPageViewModel>();
            services.AddSingleton<PendingPageViewModel>();
            services.AddSingleton<AboutPageViewModel>();
            services.AddSingleton<SettingPageViewModel>();
            services.AddSingleton<NotificationManager>();
            services.AddSingleton<GameService>();
            services.AddSingleton<AppService>();
            services.AddSingleton<FileService>(serviceProvider => new FileService(new Lazy<TopLevel>(serviceProvider.GetRequiredService<MainWindow>)));
            services.AddSingleton<HttpClient>(_ => new HttpClient());
            services.AddSingleton<DownloadConfiguration>(new DownloadConfiguration()
            {
                ChunkCount = 16,
                ParallelDownload = true,
                MaxTryAgainOnFailover = int.MaxValue,
                Timeout = 60000,
                //MaximumMemoryBufferBytes = 1024 * 1024 * 50,
                RequestConfiguration =
                {
                    KeepAlive = true,
                    UserAgent="Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:92.0) Gecko/20100101 Firefox/92.0",
                    ProtocolVersion = HttpVersion.Version11,
                }
            });
            services.AddTransient<DownloadService>(serviceProvider => new DownloadService(serviceProvider.GetRequiredService<DownloadConfiguration>()));
        }
    }
}
