using GOILauncher.Helpers;
using LeanCloud.Storage;
using ReactiveUI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GOILauncher.ViewModels.Pages
{
    public class AboutPageViewModel : PageViewModelBase
    {
        public override void Init()
        {
            Task.Run(async () =>
            {
                await GetCredits();
                this.RaisePropertyChanged(nameof(Thanks));
            });
        }
        public async Task GetCredits()
        {
            LCQuery<LCObject> query = new("Credits");
            query.AddAscendingOrder("Player");
            query.Select("Player");
            var credits = await query.Find();
            foreach (var credit in credits)
            {
                Players.Add(credit["Player"] as string);
            }
        }
        private List<string?> Players { get; } = [];
        public string Thanks => Players!.Concatenate(",");

        private string GOILauncherversion = Models.Version.Instance.ToString();

        public string GOILauncherVersion
        {
            get => $"GOILauncher v{GOILauncherversion}";
            set => this.RaiseAndSetIfChanged(ref GOILauncherversion, value);
        }
    }
}
