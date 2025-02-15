using GOILauncher.Models;
using LeanCloud;
using LeanCloud.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GOILauncher.Services;

public class LeanCloudService
{
    public static void Initialize()
    {
        LCApplication.Initialize("3Dec7Zyj4zLNDU0XukGcAYEk-gzGzoHsz", "uHF3AdKD4i3RqZB7w1APiFRF", "https://3dec7zyj.lc-cn-n1-shared.com");
    }

    private static LCQuery<LCObject> BuildQuery(string className)
    {
        return new LCQuery<LCObject>(className)
                    .Select("-createdAt")
                    .Select("-updatedAt")
                    .Select("-objectId");
    }

    public static async Task<List<Mod>> GetMods(string modName)
    {
        var LCMods = await BuildQuery(modName).OrderByDescending(nameof(Mod.Build)).Find();
        return LCMods.Select(LCMod => new Mod
            {
                Name = (LCMod[nameof(Mod.Name)] as string) ?? string.Empty,
                Build = (LCMod[nameof(Mod.Build)] as string)!,
                Url = (LCMod[nameof(Mod.Url)] as string)!,
                TargetGameVersion = (LCMod[nameof(Mod.TargetGameVersion)] as List<object>)!
                                    .Select(x => x.ToString()).ToList()!,
            })
            .ToList();
    }
}