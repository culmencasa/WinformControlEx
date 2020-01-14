//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
    public static class ControlExtension
    {
        private static System.Reflection.Assembly mscorlibAssembly;       

        public static bool IsDesignTime(this Control ctl)
        {

            // Determine if this instance is running against .NET Framework by using the MSCoreLib PublicKeyToken
            if (mscorlibAssembly == null)
            {
                mscorlibAssembly = typeof(int).Assembly;
            }
            if ((mscorlibAssembly != null))
            {
                if (mscorlibAssembly.FullName.ToUpper().EndsWith("B77A5C561934E089"))
                {
                    return true;
                }
            }
            return false;

        }

        public static void RemoveStyle(this Control control, int style)
        {
            if (!control.IsDesignTime())
            {
                int currentStyle = (int)Win32.GetWindowLong(control.Handle, (-16));

                //if (value)
                //{
                //currentStyle |= style;
                //}
                //else
                //{
                //    currentStyle = (int)NativeMethods.GetWindowLong(control.Handle, (-16));
                currentStyle &= ~style;

                //}
                Win32.SetWindowLong(control.Handle, (-16), currentStyle);
                Win32.SetWindowPos(control.Handle, 0, 
                    0, 0, control.Width, control.Height, 
                    (Win32.SWP_NOSIZE | Win32.SWP_NOACTIVATE | Win32.SWP_NOMOVE | Win32.SWP_FRAMECHANGED));
            }



        }

        public static void SetStyle(this Control control, int style)
        {
            if (!control.IsDesignTime())
            {
                int currentStyle = (int)Win32.GetWindowLong(control.Handle, (-16));

                //if (value)
                //{
                        currentStyle |= style;
                //}
                //else
                //{
                //    currentStyle = (int)NativeMethods.GetWindowLong(control.Handle, (-16));
                //    currentStyle &= ~NativeMethods.WS_BORDER;

                //}
                Win32.SetWindowLong(control.Handle, (-16), currentStyle);
                Win32.SetWindowPos(control.Handle, 0, 0, 0, control.Width, control.Height, Win32.SWP_NOSIZE | Win32.SWP_NOACTIVATE | Win32.SWP_NOMOVE | Win32.SWP_FRAMECHANGED);
            }
        }

        public static void ShowBorder(this Control control, bool value)
        {
            if (!control.IsDesignTime())
            {
                int currentStyle = (int)Win32.GetWindowLong(control.Handle, (-16));

                if (value)
                {
                    currentStyle |= Win32.WS_BORDER;
                }
                else
                {
                    currentStyle = (int)Win32.GetWindowLong(control.Handle, (-16));
                    currentStyle &= ~Win32.WS_BORDER;

                }
                Win32.SetWindowLong(control.Handle, (-16), currentStyle);
                Win32.SetWindowPos(control.Handle, 0, 0, 0, control.Width, control.Height, Win32.SWP_NOSIZE |
                    Win32.SWP_NOACTIVATE | Win32.SWP_NOMOVE | Win32.SWP_FRAMECHANGED);
            }            

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

    }
}
