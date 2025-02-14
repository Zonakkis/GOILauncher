using FluentAvalonia.UI.Controls;

namespace GOILauncher.UI;

public class Notification
{
    public required string Title { get; set; }
    public required string Message { get; set; }
    public InfoBarSeverity Severity { get; set; }


}