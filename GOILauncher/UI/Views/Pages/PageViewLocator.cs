using Avalonia.Controls;
using Avalonia.Controls.Templates;
using GOILauncher.ViewModels;
using GOILauncher.ViewModels.Pages;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GOILauncher.UI.Views.Pages
{
    public class PageViewLocator : IDataTemplate
    {
        public Control Build(object? data)
        {
            var name = data!.GetType().FullName!.Replace("PageViewModel", "Page").Replace(".ViewModel",".UI.View");
            var type = Type.GetType(name);
            if (type != null)
            {
                return (Control)App.ServiceProvider.GetRequiredService(type);
            }
            return new TextBlock { Text = "Not Found: " + name };
        }

        public bool Match(object? data)
        {
            return data is PageViewModelBase;
        }
    }
}