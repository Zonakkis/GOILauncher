using FluentAvalonia.UI.Controls;
using System.Threading;
using System.Threading.Tasks;

namespace GOILauncher.Helpers
{
    internal class NotificationHelper
    {
        public static async Task ShowContentDialog(string title, string content)
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
        public static async Task ShowNotification(string title, string message, InfoBarSeverity severity)
        {
            await CancellationTokenSource.CancelAsync();
            CancellationTokenSource = new CancellationTokenSource();
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
