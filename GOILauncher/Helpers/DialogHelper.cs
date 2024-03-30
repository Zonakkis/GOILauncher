using FluentAvalonia.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOILauncher.Helpers
{
    internal class DialogHelper
    {

        public async static Task ShowContentDialog(string title, string content)
        {
            var contentDialog = new ContentDialog()
            { 
                FontSize = 18,
                Title = title,
                Content = content,
                CloseButtonText = "好的",
            };
            await contentDialog.ShowAsync();
        }
    }
}
