using System;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Interactivity;
using FluentAvalonia.UI.Controls;
using GOILauncher.Helpers;
using GOILauncher.ViewModels;
using LeanCloud.Storage;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

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