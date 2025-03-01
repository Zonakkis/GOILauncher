using System;
using System.Reactive.Linq;
using System.Text.Json.Serialization;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace GOILauncher.Models
{
    public class Speedrun : ReactiveObject
    {
        public Speedrun()
        {
            this.WhenAnyValue(x => x.VideoPlatform, x => x.VideoId, x => x.UserId)
                .Where(x => !string.IsNullOrEmpty(x.Item1) &&
                            !string.IsNullOrEmpty(x.Item2) &&
                            !string.IsNullOrEmpty(x.Item3))
                .Subscribe(_ =>
                {
                    switch (VideoPlatform)
                    {
                        case "哔哩哔哩":
                            VideoUrl = $"https://www.bilibili.com/video/{VideoId}";
                            PlayerUrl = $"https://space.bilibili.com/{UserId}";
                            break;
                        case "YouTube":
                            VideoUrl = $"https://www.youtube.com/watch?v={VideoId}";
                            PlayerUrl = $"https://www.youtube.com/channel/{UserId}";
                            break;
                        case "Twitch":
                            VideoUrl = $"https://www.twitch.tv/videos/{VideoId}";
                            PlayerUrl = $"https://www.twitch.tv/{UserId}";
                            break;
                    }
                });
            this.WhenAnyValue(x => x.CountryCode)
                .Where(x => !string.IsNullOrEmpty(x))
                .Subscribe(x => CountryFlagUrl = $"https://www.speedrun.com/images/flags/{x}.png");
        }
        public int Rank { get;set; }
        [JsonPropertyName("player")]
        public string Player { get; init; }
        [JsonPropertyName("country")]
        public string Country { get; init; }
        [Reactive]
        [JsonPropertyName("country_code")]
        public string CountryCode { get; init; }
        [Reactive]
        [JsonPropertyName("user_id")]
        public string UserId { get; init; }
        [JsonPropertyName("platform")]
        public string Platform { get; init; }
        [JsonPropertyName("time")]
        public string Time { get; init; }
        [Reactive]
        [JsonPropertyName("video_platform")]
        public string VideoPlatform { get; init; }
        [Reactive]
        [JsonPropertyName("video_id")]
        public string VideoId { get; init; }
        public string VideoUrl { get; set; }
        public string PlayerUrl { get; set; }
        public string CountryFlagUrl { get; set; }
    }
}
