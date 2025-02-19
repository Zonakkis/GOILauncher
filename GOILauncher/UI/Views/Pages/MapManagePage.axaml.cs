using Avalonia.Controls;
using System;
using System.ComponentModel;
using System.Diagnostics;
using Avalonia;
using ReactiveUI.Fody.Helpers;
using ReactiveUI;

namespace GOILauncher.UI.Views.Pages;

public partial class MapManagePage : UserControl
{
    public MapManagePage()
    {
        InitializeComponent();
    }

    public static readonly DirectProperty<MapManagePage, int> IsCapableProperty =
        AvaloniaProperty.RegisterDirect<MapManagePage, int>(nameof(SelectedMapsCount),
            x => x.SelectedMapsCount,
            (x, value) => x.SelectedMapsCount = value);

    private int _selectedMapsCount;
    public int SelectedMapsCount
    {
        get => _selectedMapsCount;
        set => SetAndRaise(IsCapableProperty, ref _selectedMapsCount, value);
    }
    private void MapDataGridSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        SelectedMapsCount += e.AddedItems.Count;
        SelectedMapsCount -= e.RemovedItems.Count;
    }
}