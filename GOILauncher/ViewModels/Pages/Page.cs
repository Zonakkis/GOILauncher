using System;
using System.Collections.ObjectModel;

namespace GOILauncher.ViewModels.Pages;

public class Page(string label, PageViewModelBase pageViewModel, ObservableCollection<Page>? subPages = null)
{
    public string Label { get; } = label;
    public PageViewModelBase View { get; } = pageViewModel;
    public ObservableCollection<Page>? SubPages { get; } = subPages;
}