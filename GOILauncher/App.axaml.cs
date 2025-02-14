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
                var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
                mainWindow.DataContext = ServiceProvider.GetRequiredService<MainWindowViewModel>();
                desktop.MainWindow = mainWindow;
            }
            base.OnFrameworkInitializationCompleted();
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddTransient<GameView>();
            services.AddSingleton<ModView>();
            services.AddSingleton<MapView>();
            services.AddTransient<MapManageView>();
            services.AddTransient<LeaderBoardView>();
            services.AddSingleton<SubmitSpeedrunView>();
            services.AddTransient<PendingView>();
            services.AddTransient<AboutView>();
            services.AddTransient<SettingView>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<GameViewModel>();
            services.AddSingleton<ModViewModel>();
            services.AddSingleton<MapViewModel>();
            services.AddSingleton<MapManageViewModel>();
            services.AddSingleton<LeaderBoardViewModel>();
            services.AddSingleton<SubmitSpeedrunViewModel>();
            services.AddSingleton<PendingViewModel>();
            services.AddSingleton<AboutViewModel>();
            services.AddSingleton<SettingViewModel>();
            services.AddSingleton<NotificationManager>();
            services.AddSingleton<FileService>(serviceProvider => new FileService(TopLevel.GetTopLevel(serviceProvider.GetRequiredService<MainWindow>())!));
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
