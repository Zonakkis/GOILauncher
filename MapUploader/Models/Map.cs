using Avalonia.Controls;
using Avalonia.Media.Imaging;
using LC.Newtonsoft.Json.Linq;
using LeanCloud.Storage;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MapUploader.Models
{
    public class Map:LCObject, INotifyPropertyChanged
    {
        public Map() : base("Map")
        {

        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }
        public override string ToString()
        {
            return Name;
        }
        public string Name 
        {
            get=> (this["Name"] as string)!;
            set => this["Name"] = value;
        }
        public string Author 
        {
            get => (this["Author"] as string)!;
            set => this["Author"] = value;
        }
        public string Size 
        { 
            get => (this["Size"] as string)!; 
            set => this["Size"] = value;
        }
        public string Preview
        {
            get => (this["Preview"] as string)!;
            set => this["Preview"] = value;
        }
        public List<string> DownloadURL
        {
            get => (this["DownloadURL"] as List<string>)!;
            set
            {
                this["DownloadURL"] = value;
            }
        }
    }
}
