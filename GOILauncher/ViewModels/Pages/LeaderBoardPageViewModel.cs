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
            _ = GetSpeedruns();
        }
        public async Task GetSpeedruns()
        {
            Speedruns.Clear();
            var speedruns = await leanCloudService.GetSpeedruns();
            for (var i = 0; i < speedruns.Count; i++)
            {
                switch (speedruns[i].VideoPlatform)
                {
                    case "哔哩哔哩":
                        speedruns[i].VideoURL = $"https://www.bilibili.com/video/{speedruns[i].VID}";
                        speedruns[i].PlayerURL = $"https://space.bilibili.com/{speedruns[i].UID}";
                        break;
                    case "YouTube":
                        speedruns[i].VideoURL = $"https://www.youtube.com/watch?v={speedruns[i].VID}";
                        speedruns[i].PlayerURL = $"https://www.youtube.com/channel/{speedruns[i].UID}";
                        break;
                }
                speedruns[i].Rank = i + 1;
                Speedruns.Add(speedruns[i]);
            }
        }

        public ObservableCollection<Speedrun> Speedruns { get; } = [];
    }
}
