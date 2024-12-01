﻿using FluentAvalonia.UI.Controls;
using LC.Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GOILauncher.Helpers
{
    internal class NotificationHelper
    {

        public async static Task ShowContentDialog(string title, string content)
        {
            var contentDialog = new ContentDialog()
            {
                FontSize = 18,
                Title = title,
                Content = content,
                CloseButtonText = "好的",
            };
            await contentDialog.ShowAsync();
        }
        public async static void ShowNotification(string title, string message, InfoBarSeverity severity)
        {
            CancellationTokenSource?.Cancel();
            CancellationTokenSource = new();
            NotificationBar!.Title = title;
            NotificationBar.Message = message;
            NotificationBar.Severity = severity;
            NotificationBar.IsOpen = true;
            try
            {
                await Task.Delay(5000, CancellationTokenSource.Token);
                NotificationBar.IsOpen = false;
            }
            catch (TaskCanceledException)
            {
                // 忽略任务取消异常
            }
        }

        private static CancellationTokenSource CancellationTokenSource { get; set; } = new();
        public static InfoBar? NotificationBar { get; set; }
    }
}
