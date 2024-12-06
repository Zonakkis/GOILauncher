using GOILauncher.Models;
using LeanCloud.Storage;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GOILauncher.ViewModels
{
    internal class ModViewModel : ViewModelBase
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
            LCObject.RegisterSubclass(nameof(Modpack), () => new Modpack());
            LCObject.RegisterSubclass(nameof(LevelLoader), () => new LevelLoader());
            LCObject.RegisterSubclass(nameof(ModpackandLevelLoader), () => new ModpackandLevelLoader());
            LCObject.RegisterSubclass(nameof(OtherMod), () => new OtherMod());
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
            var query = new LCQuery<Modpack>(nameof(Modpack));
            query.OrderByDescending(nameof(Modpack.Build));
            var modpacks = await query.Find();
            foreach (var modpack in modpacks)
            {
                Modpacks.Add(modpack);
            }
        }
        private async Task GetLevelLoaders()
        {
            var query = new LCQuery<LevelLoader>(nameof(LevelLoader));
            query.OrderByDescending(nameof(LevelLoader.Build));
            var levelLoaders = await query.Find();
            foreach (var levelLoader in levelLoaders)
            {
                LevelLoaders.Add(levelLoader);
            }
        }
        private async Task GetModpackandLevelLoaders()
        {
            var query = new LCQuery<ModpackandLevelLoader>(nameof(ModpackandLevelLoader));
            query.OrderByDescending(nameof(ModpackandLevelLoader.Build));
            var modpackandLevelLoaders = await query.Find();
            foreach (var modpackandLevelLoader in modpackandLevelLoaders)
            {
                ModpackandLevelLoaders.Add(modpackandLevelLoader);
            }
        }
        private async Task GetOtherMods()
        {
            var  query = new LCQuery<OtherMod>(nameof(OtherMod));
            query.OrderByAscending(nameof(OtherMod.Name));
            var otherMods = await query.Find();
            foreach (var otherMod in otherMods)
            {
                OtherMods.Add(otherMod);
            }
        }
        public ObservableCollection<Modpack> Modpacks { get; } = []; 
        public ObservableCollection<LevelLoader> LevelLoaders { get; } = [];
        public ObservableCollection<ModpackandLevelLoader> ModpackandLevelLoaders { get; } = [];
        public ObservableCollection<OtherMod> OtherMods { get; } = [];
        [Reactive]
        public bool UnselectedGamePath { get; set; }
        private static Setting Setting => Setting.Instance;
    }
}
