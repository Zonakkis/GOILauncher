using Microsoft.Win32;
using ReactiveUI;
using System.Threading.Tasks;

namespace GOILauncher.ViewModels
{
    internal class ModpackManageViewModel : ViewModelBase
    {
        public override void Init()
        {
            Position = new bool[2];
            SegmentsContent = new bool[2];
            WhenToDisplay = new bool[4];
            this.RaisePropertyChanged(nameof(Position));
            Task.Run(GetPlayerPrefs);
        }

        public void GetPlayerPrefs()
        {
            var currentUserKey = Registry.CurrentUser;
            goiKey = currentUserKey.OpenSubKey("SOFTWARE\\Bennett Foddy\\Getting Over It",true);
            Position[(int)goiKey?.GetValue("Position_h3402582524", 0)] = true;
            if (goiKey is not null)
            {
                SegmentsContent[(int)goiKey.GetValue("Segments_h1071820757", 0)] = true;
                WhenToDisplay[(int)goiKey.GetValue("Display On_h1489980990", 3)] = true;
            }

            this.RaisePropertyChanged(nameof(Position));
            this.RaisePropertyChanged(nameof(SegmentsContent));
            this.RaisePropertyChanged(nameof(WhenToDisplay));
        }

        public void SavePosition()
        {
            Task.Run(async () =>
            {
                await Task.Delay(500);
                for (var i = 0; i < Position.Length; i++)
                {
                    if (Position[i])
                    {
                        goiKey?.SetValue("Position_h3402582524", i);
                        return;
                    }
                }
            });
        }
        public void SaveSegmentsContent()
        {
            Task.Run(async () =>
            {
                await Task.Delay(500);
                for (var i = 0; i < SegmentsContent.Length; i++)
                {
                    if (SegmentsContent[i])
                    {
                        goiKey?.SetValue("Segments_h1071820757", i);
                        return;
                    }
                }
            });
        }
        public void SaveWhenToDisplay()
        {
            Task.Run(async () =>
            {
                await Task.Delay(500);
                for (var i = 0; i < WhenToDisplay.Length; i++)
                {
                    if (WhenToDisplay[i])
                    {
                        goiKey?.SetValue("Display On_h1489980990", i);
                        return;
                    }
                }
            });
        }
        private RegistryKey? goiKey;
        private bool[] position = new bool[2];
        private bool[] Position { get => position; set => this.RaiseAndSetIfChanged(ref position, value); }
        private bool[] SegmentsContent { get; set; } = new bool[2];
        private bool[] WhenToDisplay { get; set; } = new bool[4];
    }
}
