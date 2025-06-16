using Avalonia.Threading;
using GOILauncher.Infrastructures.Interfaces;
using GOILauncher.Infrastructures.LeanCloud;
using GOILauncher.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GOILauncher.ViewModels.Pages
{
    public class LeaderBoardPageViewModel(ILeanCloud LeanCloud) : PageViewModelBase
    {
        public override void Init()
        {
            Dispatcher.UIThread.InvokeAsync(GetSpeedruns);
        }

        private async Task GetSpeedruns()
        {
            var query = new LeanCloudQuery<Speedrun>()
                            .OrderByAscending("TotalTime")
                            .Where("fastest", true)
                            .Where("category", "Glitchless")
                            .Select("player", "user_id", "video_id", "platform",
                            "country_code", "time", "country", "video_platform");
            var speedruns = await LeanCloud.Find(query);
            for (var i = 0; i < speedruns.Count; i++)
            {
                speedruns[i].Rank = i + 1;
                Speedruns.Add(speedruns[i]);
            }
        }

        public ObservableCollection<Speedrun> Speedruns { get; } = [];
    }
}
