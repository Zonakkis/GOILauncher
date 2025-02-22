using ReactiveUI.Fody.Helpers;

namespace GOILauncher.Models
{
    public class Speedrun() 
    {
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
