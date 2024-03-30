using LeanCloud.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOILauncher.Models
{
    internal class PendingRun : LCObject
    {
        public PendingRun() : base(nameof(PendingRun))
        {

        }


        public string Category
        {
            get => (this[nameof(Category)] as string)!;
            set => this[nameof(Category)] = value;
        }
        public string Player
        {
            get => (this["Player"] as string)!;
            set => this[nameof(Player)] = value;
        }
        public string UID
        {
            get => (this["UID"] as string)!;
            set => this[nameof(UID)] = value;
        }
        public string Platform
        {
            get => (this["Platform"] as string)!;
            set => this[nameof(Platform)] = value;
        }
        public string Time
        {
            get => (this["Time"] as string)!;
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
            get => (this["VideoPlatform"] as string)!;
            set => this[nameof(VideoPlatform)] = value;
        }
        public string VID
        {
            get => (this["VID"] as string)!;
            set => this[nameof(VID)] = value;
        }

        public string VideoURL { get; set; }
        public string PlayerURL { get; set; }
    }
}
