using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using GOILauncher.ViewModels;
using GOILauncher.Views;
using System.Diagnostics.CodeAnalysis;

namespace GOILauncher
{
    public partial class App : Application
    {
        public static TopLevel? TopLevel { get; private set; }
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
                TopLevel = TopLevel.GetTopLevel(desktop.MainWindow);
            }
            base.OnFrameworkInitializationCompleted();
        }
    }
}