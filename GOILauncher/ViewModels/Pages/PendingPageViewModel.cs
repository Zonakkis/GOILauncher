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
                            .OrderByAscending("total_time")
                            .Select("player", "category", "platform", "time", "video_id", "video_platform");
            foreach (var pendingRun in await leanCloudService.Find(query))
            {
                PendingRuns.Add(pendingRun);
            }
        }

        public ObservableCollection<PendingRun> PendingRuns { get; } = [];
    }
}
