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
            Trace.WriteLine(textBox!.Text);
            Player.Text = await BilibiliHelper.GetUserNameFromUID(await BilibiliHelper.GetUIDFromBVID(textBox.Text!));
        }

        private void Binding(object? sender, RoutedEventArgs e)
        {
        }
    }
}