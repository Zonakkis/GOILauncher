using Avalonia.Controls.Mixins;
using ReactiveUI;
using System;
using System.Diagnostics;

namespace GOILauncher.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        public virtual void OnSelectedViewChanged()
        {
            return;
        }
        public virtual void Init()
        {
            return;
        }

        public bool Initialized;
    }
}
