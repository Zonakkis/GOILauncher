using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GOILauncher.UI;

public class NotificationManager : ReactiveObject
{
    public static void ShowContentDialog(string title, string content)
    {
        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            var contentDialog = new ContentDialog()
            {
                FontSize = 20,
                Title = title,
                Content = content,
                CloseButtonText = "好的"
            };
            await contentDialog.ShowAsync();
        });
    }
    public static void ShowContentDialog(string title, string content,ICommand confirmCommand)
    {
        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            var contentDialog = new ContentDialog()
            {
                FontSize = 20,
                Title = title,
                Content = content,
                PrimaryButtonText = "确定",
                PrimaryButtonCommand = confirmCommand,
                CloseButtonText = "取消"
            };
            await contentDialog.ShowAsync();
        });
    }

    public void AddNotification(string title, string message,InfoBarSeverity severity,
        int displayTime = 5000)
    {
        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            var notification = new Notification
            {
                Title = title,
                Message = message,
                Severity = severity
            };
            Notifications.Add(notification);
            await Task.Delay(displayTime);
            Notifications.Remove(notification);
        });
    }

    public ObservableCollection<Notification> Notifications { get; set; } = [];
}