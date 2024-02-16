using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOILauncher.Models
{
    public class Version
    {
        public Version(int x,int y,int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Version(string version)
        {
            string[] v = version.Split('.', StringSplitOptions.RemoveEmptyEntries);
            x = int.Parse(v[0]);
            y = int.Parse(v[1]);
            z = int.Parse(v[2]);
        }


        int x{ get; }
        int y{ get; }
        int z{ get; }

        public static Version Instance = new Version(0,0,1);

        public int GetVersionValue()
        {
            return Convert.ToInt32($"{x}{y}{z}");
        }

        public override string ToString()
        {
            return $"{x}.{y}.{z}";
        }

        public static bool operator >(Version newVersion, Version oldVersion)
        {
            if(newVersion.GetVersionValue() > oldVersion.GetVersionValue())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool operator <(Version newVersion, Version oldVersion)
        {
            if (newVersion.GetVersionValue() < oldVersion.GetVersionValue())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
