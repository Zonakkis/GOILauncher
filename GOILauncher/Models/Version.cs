using System;

namespace GOILauncher.Models
{
    public class Version
    {
        private Version(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Version(string version)
        {
            var v = version.Split('.', StringSplitOptions.RemoveEmptyEntries);
            X = int.Parse(v[0]);
            Y = int.Parse(v[1]);
            Z = int.Parse(v[2]);
        }

        private int X { get; }
        private int Y { get; }
        private int Z { get; }

        public static readonly Version Instance = new(0, 2, 2);

        private int GetVersionValue()
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
