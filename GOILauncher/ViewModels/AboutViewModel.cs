using GOILauncher.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOILauncher.ViewModels
{
    internal class AboutViewModel : ViewModelBase
    {

        public AboutViewModel()
        {

        }

        public override void Init()
        {
            Players.Add("hongchafang");
            Players.Add("唯呵WindowsHim");
            Players.Add("ZHYUjun");
        }
        private List<string> Players { get; } = new List<string>();

        public string Thanks
        {
            get
            {
                Players!.Sort();
                string[] playersArray = Players.ToArray();
                string players = playersArray[0];
                for (int i = 1; i < playersArray.Length; i++)
                {
                    players += $",{playersArray[i]}";
                }
                return players;
            }
        }

        private string GOILverison = Models.Version.Instance.ToString();


        public string GOILVerison { get => $"GOILauncher v{GOILverison}";
            set
            {
                this.RaiseAndSetIfChanged(ref GOILverison, value, "GOILVerison");
            }
        }
}
}
