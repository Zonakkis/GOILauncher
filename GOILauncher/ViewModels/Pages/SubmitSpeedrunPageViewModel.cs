using GOILauncher.Helpers;
using GOILauncher.Models;
using GOILauncher.Services.LeanCloud;
using LeanCloud.Storage;
using ReactiveUI.Fody.Helpers;
using System.Threading.Tasks;

namespace GOILauncher.ViewModels.Pages
{
    public class SubmitSpeedrunPageViewModel : PageViewModelBase
    {
        public SubmitSpeedrunPageViewModel(LeanCloudService leanCloudService)
        {
            _leanCloudService = leanCloudService;
            SelectSpeedrunType = true;
            Category = Categories[0];
            Platform = Platforms[0];
            VideoPlatform = VideoPlatforms[0];
        }
        public void ToggleView(int para)
        {
            switch (para)
            {
                case 1:
                    SubmitSpeedrun = true;
                    SelectSpeedrunType = false;
                    break;
                case 2:
                    SubmitLevel = true;
                    SelectSpeedrunType = false;
                    break;
                case 3:
                    SubmitSpeedrun = false;
                    SubmitLevel = false;
                    SelectSpeedrunType = true;
                    break;
            }
        }

        public async Task Submit(int para)
        {
            switch (para)
            {
                case 1:
                    if (VID == string.Empty)
                    {
                        await NotificationHelper.ShowContentDialog("提示", "BV号居然是空的？");
                        return;
                    }
                    if (Player == string.Empty)
                    {
                        await NotificationHelper.ShowContentDialog("提示", "这个BV号好像无效呢...");
                        return;
                    }
                    if (await CheckWhetherExisted(Player, Category, Platform))
                    {
                        await NotificationHelper.ShowContentDialog("提示", "已存在相同玩家相同平台的同一模式速通，也许可以先等待通过审核？");
                        return;
                    }
                    var run = new PendingRun
                    {
                        Category = Category,
                        Platform = Platform,
                        Player = Player,
                        UID = (await BilibiliHelper.GetResultFromBVID(VID)).UID,
                        VideoPlatform = VideoPlatform,
                        VID = VID,
                        Time = $"{Minute}分{Second:00}.{MillionSecond:000}秒",
                        Minute = Minute,
                        Second = Second,
                        MillionSecond = MillionSecond,
                    };
                    await _leanCloudService.Create(run);
                    await NotificationHelper.ShowContentDialog("提示", "提交成功！");
                    break;
                case 2:
                    break;

            }
        }

        private async Task<bool> CheckWhetherExisted(string player, string category, string platform)
        {
            var query = new LeanCloudQuery<PendingRun>(nameof(PendingRun))
                            .Where(nameof(PendingRun.Player), player)
                            .Where(nameof(PendingRun.Category), category)
                            .Where(nameof(PendingRun.Platform), platform);
            return await _leanCloudService.Count(query) > 0;
        }
        private readonly LeanCloudService _leanCloudService;
        [Reactive]
        public bool SelectSpeedrunType { get; set; }
        [Reactive]
        public bool SubmitSpeedrun { get; set; }
        [Reactive]
        public bool SubmitLevel { get; set; }
        public string[] Categories { get; } = ["Glitchless", "Snake"];
        [Reactive]
        public string Category { get; set; }
        public string[] Platforms { get; } = ["PC", "Android", "iOS"];
        [Reactive]
        public string Platform { get; set; }
        [Reactive]
        public int Minute { get; set; }
        [Reactive]
        public int Second { get; set; }
        [Reactive]
        public int MillionSecond { get; set; }
        public string[] VideoPlatforms { get; } = ["哔哩哔哩"];
        [Reactive]
        public string VideoPlatform { get; set; }
        [Reactive]
        public string VID { get; set; } = string.Empty;
        [Reactive]
        public string Player { get; set; } = string.Empty;
    }
}
