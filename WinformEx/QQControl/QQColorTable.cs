using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace System.Windows.Forms
{

    internal class ColorTable
    {
        public static Color QQBorderColor = Color.LightBlue;  //LightBlue = Color.FromArgb(173, 216, 230)
        public static Color QQHighlightColor = RenderHelper.GetColor(QQBorderColor, 255, -63, -11, 23);   //Color.FromArgb(110, 205, 253)
        public static Color QQHighlightInnerColor = RenderHelper.GetColor(QQBorderColor, 255, -100, -44, 1);   //Color.FromArgb(73, 172, 231);
    }
}
