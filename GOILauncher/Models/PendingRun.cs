using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Text.Json.Serialization;
using System.Reactive.Linq;

namespace GOILauncher.Models
{
    public class PendingRun : ReactiveObject
    {
        public PendingRun()
        {
            this.WhenAnyValue(x => x.VID, x => x.VideoPlatform)
                .Subscribe(_ =>
                {
                    VideoURL = VideoPlatform switch
                    {
                        "哔哩哔哩" => $"https://www.bilibili.com/video/{VID}",
                        "YouTube" => $"https://www.youtube.com/watch?v={VID}",
                        "Twitch" => $"https://www.twitch.tv/videos/{VID}",
                        _ => string.Empty
                    };
                });
            this.WhenAnyValue(x => x.Minute, x => x.Second, x => x.MillionSecond)
                .Subscribe(_ =>
                {
                    TotalTime = Minute * 60 + Second + MillionSecond / 1000f;
                    Time = TotalTime switch
                    {
                        < 60 => $"{Second:00}.{MillionSecond:000}秒",
                        < 3600 => $"{Minute}分{Second:00}.{MillionSecond:000}秒",
                        _ => $"{Minute / 60}小时{Minute % 60}分{Second:00}.{MillionSecond:000}秒",
                    };
                });
            this.WhenAnyValue(x => x.CountryCode)
                .Where(x => !string.IsNullOrEmpty(x) && x != "无")
                .Subscribe(x => CountryFlagUrl = $"https://www.speedrun.com/images/flags/{x}.png");
        }

        [Reactive]
        public string Level { get; set; }
        [Reactive]
        public string Category { get; set; }
        [Reactive]
        public string Player { get; set; }
        [Reactive]
        public string CountryCode { get; init; } = "cn";
        [Reactive]
        public string UID { get; init; }
        [Reactive]
        public string Platform { get; set; }
        [Reactive]
        public int Minute { get; set; }
        [Reactive]
        public int Second { get; set; }
        [Reactive]
        public int MillionSecond { get; set; }
        [Reactive]
        public string Time { get; set; }
        [Reactive]
        public float TotalTime { get; set; }
        [Reactive]
        public string VideoPlatform { get; init; }
        [Reactive]
        public string VID { get; init; }
        [JsonIgnore]
        public string VideoURL { get; set; }
        [JsonIgnore]
        public string CountryFlagUrl { get; set; }
    }
}
