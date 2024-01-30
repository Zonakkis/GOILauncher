using Avalonia.Controls;
using Avalonia.Media.Imaging;
using LeanCloud.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GOI地图管理器.Models
{
    public class Map
    {
        public Map(LCObject map)
        {
            MapObject = map;
            this.Name = (string)MapObject["Name"];
            IsLoaded = false;
        }

        public void Load()
        {
            this.Author = (string)MapObject["Author"];
            this.Size = (string)MapObject["Size"];
            LCFile file = new LCFile("Preview.png", new Uri((string)MapObject["Preview"]));
            this.Preview = file.GetThumbnailUrl(720, 480, 100, false, "png");
            IsLoaded = true;
        }

        public LCObject MapObject { get; set; }
        public string Name { get; set; }

        public string Author { get; set; }

        public string Size { get; set; }

        public string Preview { get; set; }

        public bool IsLoaded { get; set; }


    }
}
