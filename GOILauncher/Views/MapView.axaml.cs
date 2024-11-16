using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GOILauncher.Models;

namespace GOILauncher.Views
{

    public partial class MapView : UserControl
    {
        public MapView()
        {
            InitializeComponent();
            MapSearcher.ItemFilter += SearchEmployees;
        }

        bool SearchEmployees(string search, object value)
        {
            if (value is Map emp)
            {
                if (emp.Name.Contains(search, System.StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

    }
}