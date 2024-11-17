using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOILauncher.Helpers
{
    internal static class StorageUnitConvertHelper
    {
        public static string ByteTo(double bytes)
        {
            if (bytes == 0)
            {
                return "0.00B";
            }
            if (bytes < 1024)
            {
                return $"{bytes:0.00}B";
            }
            else if (bytes < 1048576)
            {
                return $"{bytes / 1024D:0.00}KB";
            }
            else
            {
                return $"{bytes / 1048576D:0.00}MB";
            }
        }
    }
}
