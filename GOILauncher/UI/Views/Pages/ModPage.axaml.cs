using Avalonia.Controls;

namespace GOILauncher.UI.Views.Pages
{
    public partial class ModPage : UserControl
    {
        public ModPage()
        {
            InitializeComponent();
        }
        public void Unselect(object? sender,SelectionChangedEventArgs e)
        {
            (sender as ListBox)!.SelectedItem = null;
        }
    }
}