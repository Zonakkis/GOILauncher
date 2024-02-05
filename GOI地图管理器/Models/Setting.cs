using GOI地图管理器.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public string GamePath 
        {
            get => gamePath;
            set
            {
                gamePath = value;
            }
        }

        public void Save()
        {
            StorageHelper.SaveJSON(System.AppDomain.CurrentDomain.BaseDirectory, "Settings.json", this, true);
        }
    }
}
