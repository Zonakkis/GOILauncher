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
            this.WhenAnyValue(x => x.VideoId, x => x.VideoPlatform)
                .Where(x => !string.IsNullOrEmpty(x.Item1) &&
                            !string.IsNullOrEmpty(x.Item2))
                .Subscribe(_ =>
                {
                    VideoURL = VideoPlatform switch
                    {
                        "哔哩哔哩" => $"https://www.bilibili.com/video/{VideoId}",
                        "YouTube" => $"https://www.youtube.com/watch?v={VideoId}",
                        "Twitch" => $"https://www.twitch.tv/videos/{VideoId}",
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
                .Where(x => !string.IsNullOrEmpty(x))
                .Subscribe(x => CountryFlagUrl = $"https://www.speedrun.com/images/flags/{x}.png");
        }

        [Reactive]
        [JsonPropertyName("level")]
        public string Level { get; set; }
        [Reactive]
        [JsonPropertyName("category")]
        public string Category { get; set; }
        [Reactive]
        [JsonPropertyName("player")]
        public string Player { get; set; }
        [Reactive]
        [JsonPropertyName("country_code")]
        public string CountryCode { get; init; } = "cn";
        [Reactive]
        [JsonPropertyName("user_id")]
        public string UID { get; init; }
        [Reactive]
        [JsonPropertyName("platform")]
        public string Platform { get; set; }
        [Reactive]
        public int Minute { get; set; }
        [Reactive]
        public int Second { get; set; }
        [Reactive]
        public int MillionSecond { get; set; }
        [Reactive]
        [JsonPropertyName("time")]
        public string Time { get; set; }
        [Reactive]
        public float TotalTime { get; set; }
        [Reactive]
        [JsonPropertyName("video_platform")]
        public string VideoPlatform { get; init; }
        [Reactive]
        [JsonPropertyName("video_id")]
        public string VideoId { get; init; }
        [JsonIgnore]
        public string VideoURL { get; set; }
        [JsonIgnore]
        public string CountryFlagUrl { get; set; }
    }
}
