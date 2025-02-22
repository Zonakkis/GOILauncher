using ReactiveUI.Fody.Helpers;

namespace GOILauncher.Models
{
    public class PendingRun()
    {
        [Reactive]
        public string Category { get; init; }
        [Reactive]
        public string Player { get; init; }
        [Reactive]
        public string? UID { get; init; }
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
        public string? VideoURL { get; set; }
        [Reactive]
        public string? PlayerURL { get; set; }
    }
}
