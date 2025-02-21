using GOILauncher.Helpers;
using LeanCloud.Storage;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GOILauncher.Models;
using GOILauncher.Services;
using GOILauncher.Services.LeanCloud;
using DynamicData.Binding;
using ReactiveUI.Fody.Helpers;

namespace GOILauncher.ViewModels.Pages
{
    public class AboutPageViewModel : PageViewModelBase
    {
        public AboutPageViewModel(AppService appService,LeanCloudService leanCloudService)
        {
            Version = appService.Version;
            _leanCloudService = leanCloudService;
        }
        public override void Init()
        {
            _ = GetCredits();
        }

        private async Task GetCredits()
        {
            foreach (var credit in await _leanCloudService.GetCredits())
            {
                Players.Add(credit.Player);
            }
            Thanks = string.Join(',', Players);
        }
        private readonly LeanCloudService _leanCloudService;
        private ObservableCollection<string> Players { get; } = [];
        [Reactive]
        public string Thanks { get; set; }
        public Version Version { get; set; }
    }
}
