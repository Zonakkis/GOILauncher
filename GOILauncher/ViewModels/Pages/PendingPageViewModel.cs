using Avalonia.Threading;
using GOILauncher.Models;
using GOILauncher.Services.LeanCloud;
using LeanCloud.Storage;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GOILauncher.ViewModels.Pages
{
    public class PendingPageViewModel(LeanCloudService leanCloudService) : PageViewModelBase
    {
        public override void Init()
        {
            Dispatcher.UIThread.InvokeAsync(GetPendingRuns);
        }

        private async Task GetPendingRuns()
        {
            PendingRuns.Clear();
            foreach (var pendingRun in await leanCloudService.GetPendingRuns())
            {
                PendingRuns.Add(pendingRun);
            }
        }

        public ObservableCollection<PendingRun> PendingRuns { get; } = [];
    }
}
