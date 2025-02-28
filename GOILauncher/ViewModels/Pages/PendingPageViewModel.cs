using Avalonia.Threading;
using GOILauncher.Models;
using GOILauncher.Services.LeanCloud;
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
            var query = new LeanCloudQuery<PendingRun>()
                            .OrderByAscending("TotalTime")
                            .Select(nameof(PendingRun.Player), nameof(PendingRun.Category),
                                nameof(Speedrun.Platform), nameof(PendingRun.Time), 
                                nameof(Speedrun.VID),nameof(Speedrun.VideoPlatform));
            foreach (var pendingRun in await leanCloudService.Find(query))
            {
                PendingRuns.Add(pendingRun);
            }
        }

        public ObservableCollection<PendingRun> PendingRuns { get; } = [];
    }
}
