using Avalonia.Controls.Mixins;
using ReactiveUI;
using System.Diagnostics;

namespace GOILauncher.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        public virtual void OnSelectedViewChanged()
        {
            return;
        }

    }
}
