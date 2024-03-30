using LeanCloud.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOILauncher.Models
{
    internal class Speedrun : LCObject
    {
        public Speedrun() : base("Speedrun")
        {

        }


        public int Rank
        {
            get => (int)this["Rank"];
            set => this[nameof(Rank)] = value;
        }

        public string Player
        {
            get => (this["Player"] as string)!;
        }
        public string UID
        {
            get => (this["UID"] as string)!;
        }
        public string Platform
        {
            get => (this["Platform"] as string)!;
        }
        public string Time
        {
            get => (this["Time"] as string)!;
        }
        public string VideoPlatform
        {
            get => (this["VideoPlatform"] as string)!;
        }
        public string VID
        {
            get => (this["VID"] as string)!;
        }

        public string VideoURL { get; set; }
        public string PlayerURL { get; set; }
    }
}
