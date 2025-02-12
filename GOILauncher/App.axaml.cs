using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using GOILauncher.ViewModels;
using GOILauncher.UI.Views;
using Microsoft.Extensions.DependencyInjection;
using System;

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
        }
    }
}