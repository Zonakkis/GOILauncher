using LeanCloud.Storage;

namespace GOILauncher.Models
{
    internal class Speedrun() : LCObject(nameof(Speedrun))
    {
        public int Rank
        {
            get => (int)this[nameof(Rank)];
            set => this[nameof(Rank)] = value;
        }
        public string Player => (this[nameof(Player)] as string)!;
        public string UID => (this[nameof(UID)] as string)!;
        public string Platform => (this[nameof(Platform)] as string)!;
        public string Time => (this[nameof(Time)] as string)!;
        public string VideoPlatform => (this[nameof(VideoPlatform)] as string)!;
        public string VID => (this[nameof(VID)] as string)!;
        public string? VideoURL { get; set; }
        public string? PlayerURL { get; set; }
    }
}
