using GOILauncher.Helpers;
using GOILauncher.Models;
using LeanCloud.Storage;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOILauncher.ViewModels
{
    internal class AboutViewModel : ViewModelBase
    {

        public AboutViewModel()
        {

        }

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
            LCQuery<LCObject> query = new LCQuery<LCObject>("Credits");
            query.AddAscendingOrder("Player");
            query.Select("Player");
            ReadOnlyCollection<LCObject> credits = await query.Find();
            foreach (LCObject credit in credits)
            {
                Players.Add(credit["Player"] as string);
            }
        }
        private List<string> Players { get; } = new List<string>();

        public string Thanks
        {
            get
            {
                return Players.Concatenate(","); ;
            }
        }

        private string GOILverison = Models.Version.Instance.ToString();


        public string GOILVerison { get => $"GOILauncher v{GOILverison}";
            set
            {
                this.RaiseAndSetIfChanged(ref GOILverison, value, "GOILVerison");
            }
        }
}
}
