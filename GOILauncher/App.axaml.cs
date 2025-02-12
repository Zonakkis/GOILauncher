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
        public IServiceProvider ServiceProvider { get; private set; }
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

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<MainWindowViewModel>();
        }
    }
}