using Downloader;
using DynamicData;
using FluentAvalonia.UI.Controls;
using GOILauncher.Helpers;
using GOILauncher.Models;
using LeanCloud.Storage;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
            SelectedGamePathNoteHide = Setting.IsDefault(nameof(Setting.GamePath));
            Setting.GamePathChanged += () =>
            {
                SelectedGamePathNoteHide = true;
            };
        }

        public override void Init()
        {
            LCObject.RegisterSubclass(nameof(Modpack), () => new Modpack());
            LCObject.RegisterSubclass(nameof(LevelLoader), () => new LevelLoader());
            LCObject.RegisterSubclass(nameof(ModpackandLevelLoader), () => new ModpackandLevelLoader());
            LCObject.RegisterSubclass(nameof(OtherMod), () => new OtherMod());
            GetModpacks();
            GetLevelLoaders();
            GetModpackandLevelLoaders();
            GetOtherMods();
        }
        public override void OnSelectedViewChanged()
        {

        }
        private async void GetModpacks()
        {
            LCQuery<Modpack> query = new(nameof(Modpack));
            query.OrderByDescending(nameof(Modpack.Build));
            ReadOnlyCollection<Modpack> modpacks = await query.Find();
            foreach (Modpack modpack in modpacks)
            {
                Modpacks.Add(modpack);
            }
        }
        private async void GetLevelLoaders()
        {
            LCQuery<LevelLoader> query = new(nameof(LevelLoader));
            query.OrderByDescending(nameof(LevelLoader.Build));
            ReadOnlyCollection<LevelLoader> levelLoaders = await query.Find();
            foreach (LevelLoader levelLoader in levelLoaders)
            {
                LevelLoaders.Add(levelLoader);
            }
        }
        private async void GetModpackandLevelLoaders()
        {
            LCQuery<ModpackandLevelLoader> query = new(nameof(ModpackandLevelLoader));
            query.OrderByDescending(nameof(ModpackandLevelLoader.Build));
            ReadOnlyCollection<ModpackandLevelLoader> modpackandLevelLoaders = await query.Find();
            foreach (ModpackandLevelLoader modpackandLevelLoader in modpackandLevelLoaders)
            {
                ModpackandLevelLoaders.Add(modpackandLevelLoader);
            }
        }
        private async void GetOtherMods()
        {
            LCQuery<OtherMod> query = new(nameof(OtherMod));
            query.OrderByAscending(nameof(OtherMod.Name));
            ReadOnlyCollection<OtherMod> otherMods = await query.Find();
            foreach (OtherMod otherMod in otherMods)
            {
                OtherMods.Add(otherMod);
            }
        }
        public ObservableCollection<Modpack> Modpacks { get; } = []; 
        public ObservableCollection<LevelLoader> LevelLoaders { get; } = [];
        public ObservableCollection<ModpackandLevelLoader> ModpackandLevelLoaders { get; } = [];
        public ObservableCollection<OtherMod> OtherMods { get; } = [];
        [Reactive]
        public bool SelectedGamePathNoteHide { get; set; }
        private static Setting Setting => Setting.Instance;
    }
}
