﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System.Windows.Forms
{
    public static class FormExtension
    {
        /// <summary>
        /// 启用窗体自绘和双缓冲
        /// </summary>
        /// <param name="target"></param>
        public static void Enable2DBuffer(this Form target)
        {            
            int currentStyle = (int)Win32.GetWindowLong(target.Handle, (-16));

            ControlStyles newStyles = 
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor;


            currentStyle |= (int)newStyles;
            Win32.SetWindowLong(target.Handle, (-16), currentStyle);
        }



        /// <summary>
        /// 设置窗体圆角
        /// </summary>
        /// <param name="target"></param>
        /// <param name="diameter"></param>
        public static void UpdateFormRoundCorner(this Form target, int diameter)
        {
            if (diameter == 0)
            {
                target.Region = new Region(new Rectangle(0, 0, target.MaximumSize.Width, target.MaximumSize.Height));
            }
            else
            {        
                IntPtr hrgn = Win32.CreateRoundRectRgn(0, 0, target.Width, target.Height, diameter / 2 + 4, diameter / 2 + 4);
                target.Region = System.Drawing.Region.FromHrgn(hrgn);
                target.Update();
                Win32.DeleteObject(hrgn);
            }
        }

        /// <summary>
        /// 获取窗体标题栏高度(仅针对原生窗体)
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static int GetTitleBarHeight(this Form form)
        {
            Rectangle screenRectangle = form.RectangleToScreen(form.ClientRectangle);

            int titleHeight = screenRectangle.Top - form.Top;

            return titleHeight;
        }

        public static bool IsHighResolution(this Form form)
        {
            SizeF currentScreen = form.CurrentAutoScaleDimensions;
            if (currentScreen.Height == 192)
            {
                return true;
            }
            return false;
        }


        public static void MakeMoves(this Form form)
        {
            Win32.ReleaseCapture();
            var pt = MAKELONG(Control.MousePosition.X, Control.MousePosition.Y);
            Win32.SendMessage(form.Handle, (int)WindowMessages.WM_NCLBUTTONDOWN, Win32.HTCAPTION, pt);
        }
        static int MAKELONG(int low, int high)
        {
            return (high << 16) | (low & 0xffff);
        }
    }
}
