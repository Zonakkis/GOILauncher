using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace GOILauncher.Models
{
    public class Speedrun : ReactiveObject
    {
        public Speedrun()
        {
            this.WhenAnyValue(x => x.VID, x => x.VideoPlatform)
                .Subscribe(_ =>
                {
                    switch (VideoPlatform)
                    {
                        case "哔哩哔哩":
                            VideoURL = $"https://www.bilibili.com/video/{VID}";
                            PlayerURL = $"https://space.bilibili.com/{UID}";
                            break;
                        case "YouTube":
                            VideoURL = $"https://www.youtube.com/watch?v={VID}";
                            PlayerURL = $"https://www.youtube.com/channel/{UID}";
                            break;
                    }
                });
        }
        [Reactive]
        public int Rank { get;set; }
        [Reactive]
        public string Player { get; init; }
        [Reactive]
        public string UID { get; init; }
        [Reactive]
        public string Platform { get; init; }
        [Reactive]
        public string Time { get; init; }
        [Reactive]
        public string VideoPlatform { get; init; }
        [Reactive]
        public string VID { get; init; }
        [Reactive]
        public string? VideoURL { get; set; }
        [Reactive]
        public string? PlayerURL { get; set; }
    }
}
