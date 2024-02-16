﻿using GOI地图管理器.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOI地图管理器.ViewModels
{
    internal class AboutViewModel : ViewModelBase
    {

        private List<string> Players { get; } = new List<string>() {"hongchafang","维呵WindowsHim" };

        public string Thanks
        {
            get
            {
                Players.Sort();
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
