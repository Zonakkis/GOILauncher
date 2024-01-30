using GOI地图管理器.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOI地图管理器.Models
{
    public class Setting
    {

        public Setting()
        {
            gamePath = "未选择";
        }
        public string gamePath;

        public void Save()
        {
            StorageHelper.SaveJSON(System.AppDomain.CurrentDomain.BaseDirectory, "settings.json", this, true);
        }
        //public string GamePath
        //{ 
        //    get => _gamePath;
        //    set
        //    {
        //        _gamePath = value;
        //    }
        //}
    }
}
