using LeanCloud.Storage;

namespace GOILauncher.Models
{
    public class PendingRun() : LCObject(nameof(PendingRun))
    {
        public string Category
        {
            get => (this[nameof(Category)] as string)!;
            set => this[nameof(Category)] = value;
        }
        public string Player
        {
            get => (this[nameof(Player)] as string)!;
            set => this[nameof(Player)] = value;
        }
        public string? UID
        {
            get => (this[nameof(UID)] as string)!;
            set => this[nameof(UID)] = value;
        }
        public string Platform
        {
            get => (this[nameof(Platform)] as string)!;
            set => this[nameof(Platform)] = value;
        }
        public string Time
        {
            get => (this[nameof(Time)] as string)!;
            set => this[nameof(Time)] = value;
        }
        public int Minute
        {
            get => (int)this[nameof(Minute)];
            set => this[nameof(Minute)] = value;
        }
        public int Second
        {
            get => (int)this[nameof(Second)];
            set => this[nameof(Second)] = value;
        }
        public int MillionSecond
        {
            get => (int)this[nameof(MillionSecond)];
            set => this[nameof(MillionSecond)] = value;
        }
        public string VideoPlatform
        {
            get => (this[nameof(VideoPlatform)] as string)!;
            set => this[nameof(VideoPlatform)] = value;
        }
        public string VID
        {
            get => (this[nameof(VID)] as string)!;
            set => this[nameof(VID)] = value;
        }

        public string? VideoURL { get; set; }
        public string? PlayerURL { get; set; }
    }
}
