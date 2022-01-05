using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System.Windows.Forms
{
    public static class FormExtension
    {

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

        private static void InvokeSetStyle(Control target, ControlStyles styles)
        {
            try
            {
                Type type = target.GetType();
                BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
                MethodInfo method = type.GetMethod("SetStyle", flags);

                if (method != null)
                {
                    object[] param = { styles, true };
                    method.Invoke(target, param);
                }
            }
            catch (Security.SecurityException)
            {

            }
        }


        public static void UpdateFormRoundCorner(this Form target, int diameter)
        {
            if (diameter == 0)
            {
                target.Region = new Region(new Rectangle(0, 0, target.MaximumSize.Width, target.MaximumSize.Height));
            }
            else
            {
                // 防止控件撑出窗体            
                IntPtr hrgn = Win32.CreateRoundRectRgn(0, 0, target.Width, target.Height, diameter / 2 + 4, diameter / 2 + 4);
                target.Region = System.Drawing.Region.FromHrgn(hrgn);
                target.Update();
                Win32.DeleteObject(hrgn);
            }
        }

        public static int GetTitleBarHeight(this Form form)
        {
            Rectangle screenRectangle = form.RectangleToScreen(form.ClientRectangle);

            int titleHeight = screenRectangle.Top - form.Top;

            return titleHeight;
        }


    }
}
