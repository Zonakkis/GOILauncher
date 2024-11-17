using Avalonia.Controls;
using Avalonia.Interactivity;
using FluentAvalonia.UI.Controls;
using GOILauncher.Helpers;
using GOILauncher.Models;
using LeanCloud.Storage;
using Microsoft.CodeAnalysis;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GOILauncher.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            NotificationHelper.NotificationBar  = NotificationBar;
        }

        private async void Loaded(object? sender, RoutedEventArgs e)
        {
            LCQuery<LCObject> query = new("Config");
            LCObject prefixURLObject = await query.Get("6693d2d7209bbd07d16cfde9");
            LanzouyunHelper.prefix = prefixURLObject["Value"] as string;
            query = new LCQuery<LCObject>("Update");
            LCObject update = await query.Get("65cf1c6c6599eb4f2882a8c5");
            Models.Version newVersion = new((update["Version"] as string)!);
            if (newVersion > Models.Version.Instance)
            {
                var contentDialog = new ContentDialog
                {
                    Title = "有新版本！！",
                    Content = $"{Models.Version.Instance} -> {newVersion}\r\n{update["Description"]}",
                    PrimaryButtonText = "手动更新",
                    CloseButtonText = "自动更新",
                    PrimaryButtonCommand = ReactiveCommand.Create(() =>
                    {
                        Process.Start("explorer.exe", "https://github.com/Zonakkis/GOILauncher/releases");
                        Environment.Exit(0);
                    }),
                    CloseButtonCommand = ReactiveCommand.Create(() =>
                    {
                        Process.Start("GOILUpdater.exe", (update["DownloadURL"] as string)!);
                        Environment.Exit(0);
                    })
                };
                await contentDialog.ShowAsync();
            }


        }
    }
}