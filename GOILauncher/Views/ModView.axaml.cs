using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using System.Diagnostics;

namespace GOILauncher.Views
{
    public partial class ModView : UserControl
    {
        public ModView()
        {
            InitializeComponent();
        }

        void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listbox = sender as ListBox;
            listbox!.SelectedItem = null;
        }
    }
}