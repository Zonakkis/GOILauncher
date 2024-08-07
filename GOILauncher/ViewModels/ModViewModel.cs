﻿using Downloader;
using DynamicData;
using FluentAvalonia.UI.Controls;
using GOILauncher.Helpers;
using GOILauncher.Models;
using LeanCloud.Storage;
using ReactiveUI;
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

        }

        public override void Init()
        {
            Task.Run(()=>
            {
                LCObject.RegisterSubclass("Modpack", () => new Modpack());
                LCObject.RegisterSubclass("LevelLoader", () => new LevelLoader());
                LCObject.RegisterSubclass("ModpackandLevelLoader", () => new ModpackandLevelLoader());
                LCObject.RegisterSubclass(nameof(OtherMod), () => new OtherMod());
                GetModpacks();
                GetLevelLoaders();
                GetModpackandLevelLoaders();
                GetOtherMods();
            }); 

        }
        public override void OnSelectedViewChanged()
        {
            if(!SelectedGamePathNoteHide && Setting.Instance.gamePath != "未选择")
            {
                SelectedGamePathNoteHide = true;
            }
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
        private async void GetLevelLoaders()
        {
            LCQuery<LevelLoader> query = new LCQuery<LevelLoader>("LevelLoader");
            ReadOnlyCollection<LevelLoader> levelLoaders = await query.Find();
            foreach (LevelLoader levelLoader in levelLoaders)
            {
                LevelLoaders.Add(levelLoader);
            }
        }
        private async void GetModpackandLevelLoaders()
        {
            LCQuery<ModpackandLevelLoader> query = new LCQuery<ModpackandLevelLoader>("ModpackandLevelLoader");
            ReadOnlyCollection<ModpackandLevelLoader> modpackandLevelLoaders = await query.Find();
            foreach (ModpackandLevelLoader modpackandLevelLoader in modpackandLevelLoaders)
            {
                ModpackandLevelLoaders.Add(modpackandLevelLoader);
            }
        }
        private async void GetOtherMods()
        {
            LCQuery<OtherMod> query = new LCQuery<OtherMod>(nameof(OtherMod));
            ReadOnlyCollection<OtherMod> otherMods = await query.Find();
            foreach (OtherMod otherMod in otherMods)
            {
                OtherMods.Add(otherMod);
            }
        }
        public ObservableCollection<Modpack> Modpacks { get; } = new ObservableCollection<Modpack>(); 
        public ObservableCollection<LevelLoader> LevelLoaders { get; } = new ObservableCollection<LevelLoader>();
        public ObservableCollection<ModpackandLevelLoader> ModpackandLevelLoaders { get; } = new ObservableCollection<ModpackandLevelLoader>();
        public ObservableCollection<OtherMod> OtherMods { get; } = new ObservableCollection<OtherMod>();

        private bool selectedGamePathNoteHide;
        public bool SelectedGamePathNoteHide
        {
            get => selectedGamePathNoteHide; set
            {
                this.RaiseAndSetIfChanged(ref selectedGamePathNoteHide, value, "SelectedGamePathNoteHide");
            }
        }
    }
}
