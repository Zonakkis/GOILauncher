using Avalonia.Controls.Mixins;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ReactiveUI;
using System.Diagnostics;

namespace GOI地图管理器.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        public virtual void OnSelectedViewModelChanged()
        {
            return;
        }

    }
}
