using GOI地图管理器.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace GOI地图管理器.ViewModels
{
    public class MessageBoxWindowViewModel : ViewModelBase
    {
        public MessageBoxWindowViewModel()
        {
            CloseCommand = ReactiveCommand.Create(Close);
        }
        
        object Close()
        {
            return 0;
        }
        string message;

        public string Message 
        { 
            get => message;
            set => this.RaiseAndSetIfChanged(ref message, value, "Message");
        }

        public ReactiveCommand<Unit, object> CloseCommand { get; }

        
    }
}
