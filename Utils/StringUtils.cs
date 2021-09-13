using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
	public static class StringUtils
    {
        public static bool IsEmpty(this string value)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsNotEmpty(this string value)
        {
            return !IsEmpty(value);
        }
    }
}
