using FluentAvalonia.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public async static Task ShowNotification(string title, string message,InfoBarSeverity severity)
        {
            NotificationBar!.Title = title;
            NotificationBar.Message = message;
            NotificationBar.Severity = severity;
            NotificationBar.IsOpen = true;
            await Task.Delay(5000);
            NotificationBar.IsOpen = false;
        }

        internal static void ShowNotification(string v1, string v2, object infoBarSeverity)
        {
            throw new NotImplementedException();
        }

        public static InfoBar? NotificationBar { get; set; }
    }
}
