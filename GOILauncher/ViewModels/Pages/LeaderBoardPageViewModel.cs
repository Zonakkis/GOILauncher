using Avalonia.Threading;
using GOILauncher.Models;
using GOILauncher.Services.LeanCloud;
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
            var query = new LeanCloudQuery<Speedrun>(nameof(Speedrun))
                            .OrderByAscending("TotalTime")
                            .Where("Fastest", true)
                            .Where("Category", "Glitchless")
                            .Select(nameof(Speedrun.Player), nameof(Speedrun.UID), nameof(Speedrun.VID),
                                    nameof(Speedrun.Platform), nameof(Speedrun.CountryCode),
                                    nameof(Speedrun.Time), nameof(Speedrun.Country), 
                                    nameof(Speedrun.VideoPlatform));
            var speedruns = await leanCloudService.Find(query);
            for (var i = 0; i < speedruns.Count; i++)
            {
                speedruns[i].Rank = i + 1;
                Speedruns.Add(speedruns[i]);
            }
        }

        public ObservableCollection<Speedrun> Speedruns { get; } = [];
    }
}
