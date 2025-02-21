using System;

namespace GOILauncher.Models
{
    public class Version
    {
        public Version(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Version(string version)
        {
            var v = version.Split('.', StringSplitOptions.RemoveEmptyEntries);
            x = int.Parse(v[0]);
            y = int.Parse(v[1]);
            z = int.Parse(v[2]);
        }

        private readonly int x;
        private readonly int y;
        private readonly int z;

        public static readonly Version Instance = new(0, 2, 2);

        private int GetVersionValue()
        {
            return x * 10000 + y * 100 + z;
        }
        public override string ToString()
        {
            return $"v{x}.{y}.{z}";
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
