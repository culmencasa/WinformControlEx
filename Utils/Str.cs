using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class Str
    {
        public static bool IsNotEmpty(this string target)
        {
            if (target == null)
                return false;

            if (target.Length == 0)
                return false;

            return true;
        }
    }
}
