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
    internal class PendingViewModel : ViewModelBase
    {
        public PendingViewModel()
        {

        }
        public override void Init()
        {
            LCObject.RegisterSubclass(nameof(PendingRun), () => new PendingRun());
            PendingRuns = new ObservableCollection<PendingRun>();
        }
        public override void OnSelectedViewChanged()
        {
            Task.Run(GetPendingRuns);
        }
        public async Task GetPendingRuns()
        {
            LCQuery<PendingRun> query = new LCQuery<PendingRun>(nameof(PendingRun));
            if(await query.Count() == PendingRuns.Count)
            {
                return;
            }
            PendingRuns = new ObservableCollection<PendingRun>();
            ReadOnlyCollection<PendingRun> pendingRuns = await query.Find();
            foreach (PendingRun pendingRun in pendingRuns)
            {
                if(pendingRun.VideoPlatform == "哔哩哔哩")
                {
                    pendingRun.VideoURL = $"https://www.bilibili.com/video/{pendingRun.VID}";
                    pendingRun.PlayerURL = $"https://space.bilibili.com/{pendingRun.UID}";
                }
                PendingRuns.Add(pendingRun);
            }
            this.RaisePropertyChanged("PendingRuns");
        }

        public ObservableCollection<PendingRun> PendingRuns { get; set; }
    }
}
