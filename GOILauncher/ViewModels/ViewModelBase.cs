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

    }
    public interface IPage
    {
        public virtual void OnSelectedViewChanged()
        {
            return;
        }
        public string Label { get; }
        public Type ModelType { get; }
        public ViewModelBase View { get; }
    }
}
