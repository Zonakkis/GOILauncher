using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using GOILauncher.Models;
using LeanCloud.Storage;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOILauncher.ViewModels
{
    internal class LeaderBoardViewModel : ViewModelBase
    {
        public LeaderBoardViewModel()
        {

        }
        public override void Init()
        {
            LCObject.RegisterSubclass("Speedrun", () => new Speedrun());
            Task.Run(GetSpeedruns);
        }
        public async Task GetSpeedruns()
        {
            Speedruns = [];
            LCQuery<Speedrun> query = new("Speedrun");
            query.OrderByAscending("TotalTime");
            query.WhereEqualTo("Fastest", true);
            query.WhereEqualTo("Category", "Glitchless");
            ReadOnlyCollection<Speedrun> speedruns = await query.Find();
            for (int i = 0; i < speedruns.Count; i++)
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

        public ObservableCollection<Speedrun> Speedruns { get; set; }
    }
}
