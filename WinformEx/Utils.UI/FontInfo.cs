using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace System.Drawing
{
    public class FontInfo
    {
        public float SizeInPoint; // 单个字的大小
        public float Width;
        public float Height;

        // 在指定的矩形宽度内算出指定文字数对应的最佳大小(磅值). 
        public static FontInfo GetFontSize(string fontFamilyName, int textLenght, int limitedWidth)
        {
            float defaultSize = limitedWidth / textLenght / 1.33f; // 按1磅值等于1.33像素来计算一个默认大小 
            if (defaultSize <= 1)
            {
                return new FontInfo { SizeInPoint = 1, Width = 1, Height = 1 };
            }

            float fontWidth = 0;
            float fontHeight = 0;
            float sizeInPoint = defaultSize;
            float increment = 0.1f;
            while (true)
            {
                // 这里默认文字的宽和高相同的或宽大于高, 以字体的宽度为标准计算字体的Size
                using (Font font = new Font(fontFamilyName, sizeInPoint))
                {
                    fontWidth = 96 / (72 / font.Size);
                    fontHeight = font.GetHeight();

                    // 以宽为标准
                    if (fontWidth * textLenght > limitedWidth)
                    {
                        sizeInPoint -= increment;
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return new FontInfo() { Height = fontHeight, Width = fontWidth, SizeInPoint = sizeInPoint };
        }
    }
}
