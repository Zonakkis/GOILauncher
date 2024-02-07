using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using GOI地图管理器.ViewModels;
using ReactiveUI;
using System;

namespace GOI地图管理器.Views
{
    public partial class MessageBoxWindow : ReactiveWindow<MessageBoxWindowViewModel>
    {
        public MessageBoxWindow()
        {
            InitializeComponent();
            this.WhenActivated(d => d(ViewModel!.CloseCommand.Subscribe(Close)));
        }
    }
}