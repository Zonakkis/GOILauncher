﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using GOILauncher.Infrastructures.Interfaces;
using GOILauncher.Infrastructures.LeanCloud;
using GOILauncher.Models;
using GOILauncher.Services;
using ReactiveUI.Fody.Helpers;

namespace GOILauncher.ViewModels.Pages
{
    public class AboutPageViewModel(AppService appService, ILeanCloud leanCloud) : PageViewModelBase
    {
        public override void Init()
        {
            Dispatcher.UIThread.InvokeAsync(GetCredits);
        }

        private async Task GetCredits()
        {
            var query = new LeanCloudQuery<Credit>()
                            .OrderByAscending("player")
                            .Select("player");
            _players = await leanCloud.Find(query);
            Thanks = string.Join(',', _players.Select(x => x.Player));
        }

        private List<Credit> _players = [];
        [Reactive]
        public string Thanks { get; set; } = string.Empty;
        public Version Version { get; set; } = appService.Version;
    }
}
