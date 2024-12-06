using ReactiveUI;

namespace GOILauncher.ViewModels
{
    public class ViewModelBase : ReactiveObject
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
