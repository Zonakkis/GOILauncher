using Avalonia.Controls;
using Avalonia.Controls.Templates;
using GOILauncher.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GOILauncher
{
    public class ViewLocator : IDataTemplate
    {
        public Control Build(object data)
        {
            var name = data.GetType().FullName!.Replace("ViewModel", "View").Replace(".View",".UI.View");
            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control)App.ServiceProvider.GetRequiredService(type);
            }

            return new TextBlock { Text = "Not Found: " + name };
        }

        public bool Match(object data)
        {
            return data is ViewModelBase;
        }
    }
}