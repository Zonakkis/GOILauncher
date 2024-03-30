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
            Speedruns = new ObservableCollection<Speedrun>();
            LCQuery<Speedrun> query = new LCQuery<Speedrun>("Speedrun");
            query.OrderByAscending("Rank");
            ReadOnlyCollection<Speedrun> speedruns = await query.Find();
            foreach (Speedrun speedrun in speedruns)
            {
                if(speedrun.VideoPlatform == "哔哩哔哩")
                {
                    speedrun.VideoURL = $"https://www.bilibili.com/video/{speedrun.VID}";
                    speedrun.PlayerURL = $"https://space.bilibili.com/{speedrun.UID}";
                }
                Speedruns.Add(speedrun);
            }
            this.RaisePropertyChanged("Speedruns");
        }

        public ObservableCollection<Speedrun> Speedruns { get; set; }
    }
}
