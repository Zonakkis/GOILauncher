using GOILauncher.Helpers;
using GOILauncher.Models;
using LeanCloud.Storage;
using ReactiveUI.Fody.Helpers;
using System.Threading.Tasks;

namespace GOILauncher.ViewModels
{
    internal class SubmitSpeedrunViewModel : ViewModelBase
    {
        public SubmitSpeedrunViewModel()
        {
            VID = string.Empty;
            Player = string.Empty;
            SelectSpeedrunType = true;
            Categories = ["Glitchless", "Snake"];
            Platforms = ["PC", "Android", "iOS"];
            VideoPlatforms = ["哔哩哔哩"];
            Category = Categories[0];
            Platform = Platforms[0];
            VideoPlatform = VideoPlatforms[0];
        }
        public override void Init()
        {
            LCObject.RegisterSubclass(nameof(PendingRun), () => new PendingRun());
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
                    if(VID == string.Empty)
                    {
                        await NotificationHelper.ShowContentDialog("提示", "BV号居然是空的？");
                        return;
                    }
                    if (Player == string.Empty)
                    {
                        await NotificationHelper.ShowContentDialog("提示", "这个BV号好像无效呢...");
                        return;
                    }
                    if(await CheckWhetherExisted(Player,Category,Platform))
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
                    await run.Save();
                    await NotificationHelper.ShowContentDialog("提示", "提交成功！");
                    break;
                case 2:
                    break;

            }
        }

        public async Task<bool> CheckWhetherExisted(string player, string category, string platform)
        {
            LCQuery<PendingRun> query = new(nameof(PendingRun));
            query.WhereEqualTo("Player", player);
            query.WhereEqualTo("Category", category);
            query.WhereEqualTo("Platform", platform);
            if (await query.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        [Reactive]
        public bool SelectSpeedrunType { get; set; }
        [Reactive]
        public bool SubmitSpeedrun { get; set; }
        [Reactive]
        public bool SubmitLevel { get; set; }
        [Reactive]
        public string[] Categories { get; set; }
        [Reactive]
        public string Category { get; set; }
        [Reactive]
        public string[] Platforms { get; set; }
        [Reactive]
        public string Platform { get; set; }
        [Reactive]
        public int Minute { get; set; }
        [Reactive]
        public int Second { get; set; }
        [Reactive]
        public int MillionSecond { get; set; }
        [Reactive]
        public string[] VideoPlatforms { get; set; }
        [Reactive]
        public string VideoPlatform { get; set; }
        [Reactive]
        public string VID { get; set; }
        [Reactive]
        public string Player { get; set; }
    }
}
