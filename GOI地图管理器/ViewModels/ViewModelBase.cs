using Avalonia.Controls.Mixins;
using ReactiveUI;
using System.Diagnostics;

namespace GOI地图管理器.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        public virtual void OnSelectedViewChanged()
        {
            return;
        }

    }
}
