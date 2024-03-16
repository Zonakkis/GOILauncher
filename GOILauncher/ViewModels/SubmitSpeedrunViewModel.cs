using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace GOILauncher.ViewModels
{
    internal class SubmitSpeedrunViewModel : ViewModelBase
    {
        public SubmitSpeedrunViewModel()
        {
        }

        public override void Init()
        {
            SelectSpeedrunType = true;
            Categories = ["Glitchless", "Snake"];
            Platforms = ["PC", "Android", "iOS"];
            VideoPlatforms = ["哔哩哔哩"];
            Category = Categories[0];
            Platform = Platforms[0];
            VideoPlatform = VideoPlatforms[0];
        }

        public void ToggleView(int para)
        {
            switch (para)
            {
                case 1:
                    SubmitSpeedrun = true;
                    SelectSpeedrunType = false;
                    break;
                case 2:
                    SubmitLevel = true;
                    SelectSpeedrunType = false;
                    break;
                case 3:
                    SubmitSpeedrun = false;
                    SubmitLevel = false;
                    SelectSpeedrunType = true;
                    break;
            }
        }

        private bool selectSpeedrunType;
        public bool SelectSpeedrunType
        {
            get
            {
                return selectSpeedrunType;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref selectSpeedrunType, value, nameof(SelectSpeedrunType));
            }
        }

        private bool submitSpeedrun;
        public bool SubmitSpeedrun
        {
            get
            {
                return submitSpeedrun;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref submitSpeedrun, value, nameof(SubmitSpeedrun));
            }
        }

        private bool submitLevel;
        public bool SubmitLevel
        {
            get
            {
                return submitLevel;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref submitLevel, value, nameof(SubmitLevel));
            }
        }

        private string[] categories;
        public string[] Categories
        {
            get
            {
                return categories;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref categories, value, nameof(Categories));
            }
        }

        private string category;
        public string Category
        {
            get
            {
                return category;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref category, value, nameof(Category));
            }
        }

        private string[] platforms;
        public string[] Platforms
        {
            get
            {
                return platforms;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref platforms, value, nameof(Platforms));
            }
        }

        private string platform;
        public string Platform
        {
            get
            {
                return platform;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref platform, value, nameof(Platform));
            }
        }
        private int minute;
        public int Minute
        {
            get
            {
                return minute;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref minute, value, nameof(Minute));
            }
        }
        private int second;
        public int Second
        {
            get
            {
                return second;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref second, value, nameof(Second));
            }
        }
        private int millionSecond;
        public int MillionSecond
        {
            get
            {
                return millionSecond;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref millionSecond, value, nameof(MillionSecond));
            }
        }
        private string[] videoPlatforms;
        public string[] VideoPlatforms
        {
            get
            {
                return videoPlatforms;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref videoPlatforms, value, nameof(VideoPlatforms));
            }
        }

        private string videoPlatform;
        public string VideoPlatform
        {
            get
            {
                return videoPlatform;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref videoPlatform, value, nameof(VideoPlatform));
            }
        }

    }
}
