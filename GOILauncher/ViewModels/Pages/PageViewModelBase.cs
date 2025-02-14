using ReactiveUI;

namespace GOILauncher.ViewModels.Pages
{
    public class PageViewModelBase : ViewModelBase
    {
        public virtual void OnSelectedViewChanged()
        {
        }
        public virtual void Init()
        {
        }

        public bool Initialized;
    }
}
