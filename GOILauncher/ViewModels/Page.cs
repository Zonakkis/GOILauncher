using System;
using System.Collections.ObjectModel;

namespace GOILauncher.ViewModels;

public class Page(string label, ViewModelBase viewModel, ObservableCollection<Page>? subPages = null)
{
    public string Label { get; } = label;
    public ViewModelBase View { get; } = viewModel;
    public ObservableCollection<Page>? SubPages { get; } = subPages;
}