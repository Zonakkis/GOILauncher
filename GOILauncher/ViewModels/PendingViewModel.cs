using GOILauncher.Models;
using LeanCloud.Storage;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GOILauncher.ViewModels
{
    public class PendingViewModel : ViewModelBase
    {
        public override void Init()
        {
            LCObject.RegisterSubclass(nameof(PendingRun), () => new PendingRun());
        }
        public override void OnSelectedViewChanged()
        {
            Task.Run(GetPendingRuns);
        }
        public async Task GetPendingRuns()
        {
            LCQuery<PendingRun> query = new(nameof(PendingRun));
            if(await query.Count() == PendingRuns.Count)
            {
                return;
            }
            PendingRuns = [];
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
            this.RaisePropertyChanged(nameof(PendingRuns));
        }

        public ObservableCollection<PendingRun> PendingRuns { get; private set; } = [];
    }
}
