using GOILauncher.Helpers;
using GOILauncher.Models;
using GOILauncher.Services.LeanCloud;
using ReactiveUI;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI.Fody.Helpers;
using System;

namespace GOILauncher.ViewModels.Pages
{
    public class SubmitSpeedrunPageViewModel : PageViewModelBase
    {
        public SubmitSpeedrunPageViewModel(LeanCloudService leanCloudService)
        {
            _leanCloudService = leanCloudService;
            PendingRun = new PendingRun
            {
                Level = Levels[0],
                Category = Categories[0],
                Platform = Platforms[0],
                VideoPlatform = VideoPlatforms[0]
            };
            SubmitCommand = ReactiveCommand.CreateFromTask(Submit,
                this.WhenAnyValue(x => x.PendingRun.VID, x => x.PendingRun.Player,
                (vid, player) => !string.IsNullOrEmpty(vid) && !string.IsNullOrEmpty(player)));
            this.WhenAnyValue(x => x.PendingRun.Level)
                .Subscribe(x => FullGame = x == "完整游戏");
        }

        private async Task Submit()
        {
            await _leanCloudService.Create(PendingRun);
            await NotificationHelper.ShowContentDialog("提示", "提交成功！");
        }
        private readonly LeanCloudService _leanCloudService;

        public PendingRun PendingRun { get; }
        public string[] Levels { get; } = ["完整游戏", "Tutorial","Devil's Chimney","Slide Skip",
                                           "Furniture Land","Orange Hell","Anvil","Bucket",
                                           "Ice Mountain","Radio Tower","Space"];
        [Reactive]
        public bool FullGame { get; set; }
        public string[] Categories { get; } = ["Glitchless", "Snake"];
        public string[] Platforms { get; } = ["PC", "Android", "iOS"];
        public string[] VideoPlatforms { get; } = ["哔哩哔哩"];
        public ReactiveCommand<Unit,Unit> SubmitCommand { get; }
    }
}
