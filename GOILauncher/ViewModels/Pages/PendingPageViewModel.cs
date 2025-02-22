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
            var pendingRuns = await leanCloudService.GetPendingRuns();
            foreach (var pendingRun in pendingRuns)
            {
                if (pendingRun.VideoPlatform == "哔哩哔哩")
                {
                    pendingRun.VideoURL = $"https://www.bilibili.com/video/{pendingRun.VID}";
                    pendingRun.PlayerURL = $"https://space.bilibili.com/{pendingRun.UID}";
                }
                PendingRuns.Add(pendingRun);
            }
        }

        public ObservableCollection<PendingRun> PendingRuns { get; } = [];
    }
}
