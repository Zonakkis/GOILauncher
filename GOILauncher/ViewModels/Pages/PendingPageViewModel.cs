using Avalonia.Threading;
using GOILauncher.Infrastructures.Interfaces;
using GOILauncher.Infrastructures.LeanCloud;
using GOILauncher.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GOILauncher.ViewModels.Pages
{
    public class PendingPageViewModel(ILeanCloud LeanCloud) : PageViewModelBase
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
            foreach (var pendingRun in await LeanCloud.Find(query))
            {
                PendingRuns.Add(pendingRun);
            }
        }

        public ObservableCollection<PendingRun> PendingRuns { get; } = [];
    }
}
