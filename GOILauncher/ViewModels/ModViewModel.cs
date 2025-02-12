using GOILauncher.Models;
using LeanCloud.Storage;
using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GOILauncher.ViewModels
{
    public class ModViewModel : ViewModelBase
    {
        public ModViewModel()
        {
            UnselectedGamePath = !Setting.IsDefault(nameof(Setting.GamePath));
            Setting.GamePathChanged += () =>
            {
                UnselectedGamePath = true;
            };
        }

        public override void Init()
        {
            _ = GetModpacks();
            _ = GetLevelLoaders();
            _ = GetModpackandLevelLoaders();
            _ = GetOtherMods();
        }
        public override void OnSelectedViewChanged()
        {

        }
        private async Task GetModpacks()
        {
            var query = new LCQuery<LCObject>("Modpack");
            query.OrderByDescending(nameof(Mod.Build));
            var modpacks = await query.Find();
            foreach (var modpack in modpacks)
            {
                Modpack.Add(new Mod()
                {
                    Name = "Modpack",
                    Build = (modpack[nameof(Mod.Build)] as string)!,
                    Url = (modpack[nameof(Mod.Url)] as string)!,
                    TargetGameVersion = (modpack[nameof(Mod.TargetGameVersion)] as List<object>)!
                        .Select(x => x.ToString()).ToList()!,
                });
            }
        }
        private async Task GetLevelLoaders()
        {
            var query = new LCQuery<LCObject>("LevelLoader");
            query.OrderByDescending(nameof(Mod.Build));
            var levelLoaders = await query.Find();
            foreach (var levelLoader in levelLoaders)
            {
                LevelLoader.Add(new Mod()
                {
                    Name = "LevelLoader",
                    Build = (levelLoader[nameof(Mod.Build)] as string)!,
                    Url = (levelLoader[nameof(Mod.Url)] as string)!,
                    TargetGameVersion = (levelLoader[nameof(Mod.TargetGameVersion)] as List<object>)!
                        .Select(x => x.ToString()).ToList()!,
                });
            }
        }
        private async Task GetModpackandLevelLoaders()
        {
            var query = new LCQuery<LCObject>("ModpackandLevelLoader");
            query.OrderByDescending(nameof(Mod.Build));
            var modpackandLevelLoaders = await query.Find();
            foreach (var modpackandLevelLoader in modpackandLevelLoaders)
            {
                ModpackandLevelLoader.Add(new Mod()
                {
                    Name = "ModpackandLevelLoader",
                    Build = (modpackandLevelLoader[nameof(Mod.Build)] as string)!,
                    Url = (modpackandLevelLoader[nameof(Mod.Url)] as string)!,
                    TargetGameVersion = (modpackandLevelLoader[nameof(Mod.TargetGameVersion)] as List<object>)!
                        .Select(x => x.ToString()).ToList()!,
                });
            }
        }
        private async Task GetOtherMods()
        {
            var query = new LCQuery<LCObject>("OtherMod");
            query.OrderByDescending(nameof(Mod.Build));
            var otherMods = await query.Find();
            foreach (var otherMod in otherMods)
            {
                OtherMod.Add(new Mod()
                {
                    Name = (otherMod[nameof(Mod.Name)] as string)!,
                    Author = (otherMod[nameof(Mod.Author)] as string)!,
                    Build = (otherMod[nameof(Mod.Build)] as string)!,
                    Url = (otherMod[nameof(Mod.Url)] as string)!,
                    TargetGameVersion = (otherMod[nameof(Mod.TargetGameVersion)] as List<object>)!
                        .Select(x => x.ToString()).ToList()!,
                });
            }
        }
        public ObservableCollection<Mod> Modpack { get; set; } = [];
        public ObservableCollection<Mod> LevelLoader { get; set; } = [];
        public ObservableCollection<Mod> ModpackandLevelLoader { get; set; } = [];
        public ObservableCollection<Mod> OtherMod { get; set; } = [];
        [Reactive]
        public bool UnselectedGamePath { get; set; }
        private static Setting Setting => Setting.Instance;
    }
}
