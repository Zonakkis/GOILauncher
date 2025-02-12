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

namespace GOILauncher
{
    public partial class App : Application
    {
        public static TopLevel? TopLevel { get; private set; }
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
                desktop.MainWindow = new MainWindow
                {
                    DataContext = ServiceProvider.GetRequiredService<MainWindowViewModel>(),
                };
                TopLevel = TopLevel.GetTopLevel(desktop.MainWindow);
            }
            base.OnFrameworkInitializationCompleted();
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddTransient<GameView>();
            services.AddSingleton<ModView>();
            services.AddSingleton<MapView>();
            services.AddTransient<MapManageView>();
            services.AddTransient<LeaderBoardView>();
            services.AddSingleton<SubmitSpeedrunView>();
            services.AddTransient<PendingView>();
            services.AddTransient<AboutView>();
            services.AddTransient<SettingView>();
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<GameViewModel>();
            services.AddTransient<ModViewModel>();
            services.AddTransient<MapViewModel>();
            services.AddTransient<MapManageViewModel>();
            services.AddTransient<LeaderBoardViewModel>();
            services.AddTransient<SubmitSpeedrunViewModel>();
            services.AddTransient<PendingViewModel>();
            services.AddTransient<AboutViewModel>();
            services.AddTransient<SettingViewModel>();
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
