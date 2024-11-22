using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOILauncher.Models
{
    public class Version
    {
        public Version(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Version(string version)
        {
            string[] v = version.Split('.', StringSplitOptions.RemoveEmptyEntries);
            X = int.Parse(v[0]);
            Y = int.Parse(v[1]);
            Z = int.Parse(v[2]);
        }
        int X { get; }
        int Y { get; }
        int Z { get; }

        public static readonly Version Instance = new(0, 2, 0);
        public int GetVersionValue()
        {
            return X * 10000 + Y * 100 + Z;
        }
        public override string ToString()
        {
            return $"{X}.{Y}.{Z}";
        }
        public static bool operator >(Version newVersion, Version oldVersion)
        {
            return newVersion.GetVersionValue() > oldVersion.GetVersionValue();
        }
        public static bool operator <(Version newVersion, Version oldVersion)
        {
            return newVersion.GetVersionValue() < oldVersion.GetVersionValue();
        }
    }
}
