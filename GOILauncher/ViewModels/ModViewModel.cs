using Downloader;
using FluentAvalonia.UI.Controls;
using GOILauncher.Helpers;
using GOILauncher.Models;
using LeanCloud.Storage;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GOILauncher.ViewModels
{
    internal class ModViewModel : ViewModelBase
    {
        public ModViewModel()
        {
            LCObject.RegisterSubclass("Modpack", () => new Modpack());
            GetModpacks();
        }

        private async void GetModpacks()
        {
            LCQuery<Modpack> query = new LCQuery<Modpack>("Modpack");
            ReadOnlyCollection<Modpack> modpacks = await query.Find();
            foreach (Modpack modpack in modpacks)
            {
                Modpacks.Add(modpack);
            }
        }
        public ObservableCollection<Modpack> Modpacks { get; } = new ObservableCollection<Modpack>(); 
    }
}
