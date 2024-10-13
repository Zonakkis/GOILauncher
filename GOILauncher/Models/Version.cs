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
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
        public Version(string version)
        {
            string[] v = version.Split('.', StringSplitOptions.RemoveEmptyEntries);
            X = int.Parse(v[0]);
            Y = int.Parse(v[1]);
            Z = int.Parse(v[2]);
        }


        int X{ get; }
        int Y{ get; }
        int Z{ get; }

        public static readonly Version Instance = new(0,1,1);

        public int GetVersionValue()
        {
            return Convert.ToInt32($"{X}{Y}{Z}");
        }

        public override string ToString()
        {
            return $"{X}.{Y}.{Z}";
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
