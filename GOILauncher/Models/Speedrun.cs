using System;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace GOILauncher.Models
{
    public class Speedrun : ReactiveObject
    {
        public Speedrun()
        {
            this.WhenAnyValue(x => x.VideoPlatform)
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
                        case "Twitch":
                            VideoURL = $"https://www.twitch.tv/videos/{VID}";
                            PlayerURL = $"https://www.twitch.tv/{UID}";
                            break;
                    }
                });
            this.WhenAnyValue(x => x.CountryCode)
                .Where(x => !string.IsNullOrEmpty(x) && x != "无")
                .Subscribe(x => CountryFlagUrl = $"https://www.speedrun.com/images/flags/{x}.png");
        }
        public int Rank { get;set; }
        public string Player { get; init; }
        public string Country { get; init; }
        [Reactive]
        public string CountryCode { get; init; }
        public string UID { get; init; }
        public string Platform { get; init; }
        public string Time { get; init; }
        [Reactive]
        public string VideoPlatform { get; init; }
        [Reactive]
        public string VID { get; init; }
        public string VideoURL { get; set; }
        public string PlayerURL { get; set; }
        public string CountryFlagUrl { get; set; }
    }
}
