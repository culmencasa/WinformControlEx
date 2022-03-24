using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Utils.UI
{
    public static class ColorEx
    {
        /// <summary>
        /// 颜色加深
        /// </summary>
        /// <param name="colorIn"></param>
        /// <param name="percent"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static Color DarkenColor(Color colorIn, int percent)
        {
            if (percent < 0 || percent > 100)
                throw new ArgumentOutOfRangeException("percent");

            int a, r, g, b;

            a = colorIn.A;
            r = colorIn.R - (int)((colorIn.R / 100f) * percent);
            g = colorIn.G - (int)((colorIn.G / 100f) * percent);
            b = colorIn.B - (int)((colorIn.B / 100f) * percent);

            return Color.FromArgb(a, r, g, b);
        }


        /// <summary>
        /// 颜色减淡
        /// </summary>
        /// <param name="colorIn"></param>
        /// <param name="percent"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static Color LightenColor(Color colorIn, int percent)
        {
            if (percent < 0 || percent > 100)
                throw new ArgumentOutOfRangeException("percent");

            int a, r, g, b;

            a = colorIn.A;
            r = colorIn.R + (int)(((255f - colorIn.R) / 100f) * percent);
            g = colorIn.G + (int)(((255f - colorIn.G) / 100f) * percent);
            b = colorIn.B + (int)(((255f - colorIn.B) / 100f) * percent);

            return Color.FromArgb(a, r, g, b);
        }


    }
}
