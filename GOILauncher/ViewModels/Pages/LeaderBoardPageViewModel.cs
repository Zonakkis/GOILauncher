using Avalonia.Threading;
using GOILauncher.Models;
using GOILauncher.Services.LeanCloud;
using LeanCloud.Storage;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GOILauncher.ViewModels.Pages
{
    public class LeaderBoardPageViewModel(LeanCloudService leanCloudService) : PageViewModelBase
    {
        public override void Init()
        {
            Dispatcher.UIThread.InvokeAsync(GetSpeedruns);
        }

        private async Task GetSpeedruns()
        {
            Speedruns.Clear();
            var speedruns = await leanCloudService.GetSpeedruns();
            for (var i = 0; i < speedruns.Count; i++)
            {
                speedruns[i].Rank = i + 1;
                Speedruns.Add(speedruns[i]);
            }
        }

        public ObservableCollection<Speedrun> Speedruns { get; } = [];
    }
}
