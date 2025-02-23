using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Text.Json.Serialization;

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
        }

        [Reactive]
        public string Category { get; init; }
        [Reactive]
        public string Player { get; init; }
        [Reactive]
        public string UID { get; init; }
        [Reactive]
        public string Platform { get; init; }
        [Reactive]
        public string Time { get; init; }
        [Reactive]
        public int Minute { get; init; }
        [Reactive]
        public int Second { get; init; }
        [Reactive]
        public int MillionSecond { get; init; }
        [Reactive]
        public string VideoPlatform { get; init; }
        [Reactive]
        public string VID { get; init; }
        [Reactive]
        [JsonIgnore]
        public string? VideoURL { get; set; }
        [Reactive]
        [JsonIgnore]
        public string? PlayerURL { get; set; }
    }
}
