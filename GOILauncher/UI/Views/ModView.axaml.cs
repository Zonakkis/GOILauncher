using Avalonia.Controls;

namespace GOILauncher.UI.Views
{
    public partial class ModView : UserControl
    {
        public ModView()
        {
            InitializeComponent();
        }
        public void Unselect(object? sender,SelectionChangedEventArgs e)
        {
            (sender as ListBox)!.SelectedItem = null;
        }
    }
}