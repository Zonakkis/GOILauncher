using Avalonia;
using Avalonia.Controls;
using GOILauncher.Models;
using GOILauncher.ViewModels.Models;
using System.Collections.ObjectModel;
using System.Reactive;
using ReactiveUI;

namespace GOILauncher.UI.Controls;

public partial class ModExpander : UserControl
{
    public static readonly StyledProperty<string> ModNameProperty =
        AvaloniaProperty.Register<ModExpander, string>(nameof(ModName));

    public static readonly StyledProperty<bool> IsOtherModProperty =
        AvaloniaProperty.Register<ModExpander, bool>(nameof(IsOtherMod));

    public static readonly StyledProperty<string> AuthorProperty =
        AvaloniaProperty.Register<ModExpander, string>(nameof(Author));

    public static readonly StyledProperty<ObservableCollection<ModViewModel>> ModsProperty =
        AvaloniaProperty.Register<ModExpander, ObservableCollection<ModViewModel>>(nameof(Mods));

    public string ModName
    {
        get => GetValue(ModNameProperty);
        set => SetValue(ModNameProperty, value);
    }
    public bool IsOtherMod
    {
        get => GetValue(IsOtherModProperty);
        set => SetValue(IsOtherModProperty, value);
    }
    public string Author
    {
        get => GetValue(AuthorProperty);
        set => SetValue(AuthorProperty, value);
    }
    public ObservableCollection<ModViewModel> Mods
    {
        get => GetValue(ModsProperty);
        set => SetValue(ModsProperty, value);
    }

    public ModExpander()
    {
        InitializeComponent();

    }
    public void Unselect(object? sender, SelectionChangedEventArgs e)
    {
        (sender as ListBox)!.SelectedItem = null;
    }
}