using GOILauncher.Models;
using LeanCloud.Storage;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GOILauncher.ViewModels
{
    public class LeaderBoardViewModel : ViewModelBase
    {
        public override void Init()
        {
            LCObject.RegisterSubclass(nameof(Speedrun), () => new Speedrun());
            _ = GetSpeedruns();
        }
        public async Task GetSpeedruns()
        {
            Speedruns.Clear();
            LCQuery<Speedrun> query = new(nameof(Speedrun));
            query.OrderByAscending("TotalTime");
            query.WhereEqualTo("Fastest", true);
            query.WhereEqualTo("Category", "Glitchless");
            var speedruns = await query.Find();
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
            this.RaisePropertyChanged(nameof(Speedruns));
        }

        public ObservableCollection<Speedrun> Speedruns { get; set; } = [];
    }
}
