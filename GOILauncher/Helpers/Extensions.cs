using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOILauncher.Helpers
{
    internal static class Extensions
    {
        public static string Concatenate(this List<string> list, string separator)
        {
            if(list is null ||list.Count==0)
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(list[0]);
            for (int i = 1; i < list.Count; i++) 
            {
                sb.Append(separator);
                sb.Append(list[i]);
            }
            return sb.ToString();
        }

    }
}
