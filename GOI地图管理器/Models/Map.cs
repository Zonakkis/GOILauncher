using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOI地图管理器.Models
{
    public class Map
    {
        // Token: 0x0600004F RID: 79 RVA: 0x00003973 File Offset: 0x00001B73
        public Map(string name, string author, string size, string preview)
        {
            this.Name = name;
            this.Author = author;
            this.Size = size;
            this.Preview = preview;
        }

        // Token: 0x17000010 RID: 16
        // (get) Token: 0x06000050 RID: 80 RVA: 0x0000399E File Offset: 0x00001B9E
        // (set) Token: 0x06000051 RID: 81 RVA: 0x000039A6 File Offset: 0x00001BA6
        public string Name { get; set; }

        // Token: 0x17000011 RID: 17
        // (get) Token: 0x06000052 RID: 82 RVA: 0x000039AF File Offset: 0x00001BAF
        // (set) Token: 0x06000053 RID: 83 RVA: 0x000039B7 File Offset: 0x00001BB7
        public string Author { get; set; }

        // Token: 0x17000012 RID: 18
        // (get) Token: 0x06000054 RID: 84 RVA: 0x000039C0 File Offset: 0x00001BC0
        // (set) Token: 0x06000055 RID: 85 RVA: 0x000039C8 File Offset: 0x00001BC8
        public string Size { get; set; }

        // Token: 0x17000013 RID: 19
        // (get) Token: 0x06000056 RID: 86 RVA: 0x000039D1 File Offset: 0x00001BD1
        // (set) Token: 0x06000057 RID: 87 RVA: 0x000039D9 File Offset: 0x00001BD9
        public string Preview { get; set; }
    }
}
