using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using FluentAvalonia.UI.Controls;
using GOILauncher.Helpers;
using System.Diagnostics;

namespace GOILauncher.Views
{

    public partial class SubmitSpeedrunView : UserControl
    {
        public SubmitSpeedrunView()
        {
            InitializeComponent();
        }

        private async void InputBVIDCompleted(object? sender,RoutedEventArgs e)
        {
            TextBox? textBox = sender as TextBox;
            Player.Text = (await BilibiliHelper.GetResultFromBVID(textBox.Text!)).Name;
        }

        private void Binding(object? sender, RoutedEventArgs e)
        {
        }
    }
}