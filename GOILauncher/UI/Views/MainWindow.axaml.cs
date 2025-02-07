using System;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Interactivity;
using FluentAvalonia.UI.Controls;
using GOILauncher.Helpers;
using LeanCloud.Storage;
using ReactiveUI;

namespace GOILauncher.UI.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            NotificationHelper.NotificationBar  = NotificationBar;
        }

        public new async void Loaded(object? sender, RoutedEventArgs e)
        {
            var query = new LCQuery<LCObject>("Update");
            var update = await query.Get("65cf1c6c6599eb4f2882a8c5");
            Models.Version newVersion = new((update[nameof(Version)] as string)!);
            if (newVersion > Version)
            {
                var contentDialog = new ContentDialog
                {
                    Title = "有新版本！！",
                    Content = $"{Version} -> {newVersion}\r\n{update["Description"]}",
                    PrimaryButtonText = "手动更新",
                    CloseButtonText = "自动更新",
                    PrimaryButtonCommand = ReactiveCommand.Create(() =>
                    {
                        Process.Start("explorer.exe", "https://github.com/Zonakkis/GOILauncher/releases");
                        Environment.Exit(0);
                    }),
                    CloseButtonCommand = ReactiveCommand.Create(() =>
                    {
                        Process.Start("GOILUpdater.exe", (update["URL"] as string)!);
                        Environment.Exit(0);
                    })
                };
                await contentDialog.ShowAsync();
            }
        }
        private static Models.Version Version => Models.Version.Instance;
    }
}