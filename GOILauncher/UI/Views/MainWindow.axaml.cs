using Avalonia.Controls;
using GOILauncher.Helpers;
using GOILauncher.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace GOILauncher.UI.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
#if DEBUG
            if (Design.IsDesignMode)
            {
                Design.SetDataContext(this,App.ServiceProvider.GetRequiredService<MainWindowViewModel>());
            }
#endif
            InitializeComponent();
            NotificationHelper.NotificationBar = NotificationBar;
        }
    }
}