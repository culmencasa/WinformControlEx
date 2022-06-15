using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FormExCore.ThirdParty
{

    /// <summary>
    /// ref: https://github.com/sh4d0w4RCH3R415/DropShadow
    /// </summary>
    public class DropShadowCS
    {
        private bool m_aeroEnabled;

        private const int CS_DROPSHADOW = 0x00020000;

        [DllImport("dwmapi")]
        private static extern int DwmExtendFrameIntoClientArea(IntPtr handle, ref MARGINS pMarInset);

        [DllImport("dwmapi")]
        private static extern int DwmSetWindowAttribute(IntPtr handle, int attr, ref int attrValue, int attrSize);

        [DllImport("dwmapi")]
        private static extern int DwmIsCompositionEnabled(ref int pfEnabled);

        [DllImport("user32")]
        private static extern bool ReleaseCapture();

        [DllImport("user32")]
        private static extern int SendMessage(IntPtr handle, int msg, int wp, int lp);

        private struct MARGINS
        {
            public int LeftWidth;
            public int RightWidth;
            public int TopHeight;
            public int BottomHeight;
        }

        public CreateParams CreateWNDParams(CreateParams cp)
        {
            m_aeroEnabled = CheckAeroEnabled();
            if (!m_aeroEnabled)
            {
                cp.ClassStyle |= CS_DROPSHADOW;
            }
            return cp;
        }

        public void CreateDropShadow(Form window, Control captionBar)
        {
            window.Paint += delegate (object sender, PaintEventArgs e)
            {
                var gfx = e.Graphics;
                if (m_aeroEnabled)
                {
                    var v = 2;
                    DwmSetWindowAttribute(window.Handle, 2, ref v, 4);
                    MARGINS margins = new MARGINS
                    {
                        BottomHeight = 1,
                        LeftWidth = 0,
                        RightWidth = 0,
                        TopHeight = 0
                    };
                    DwmExtendFrameIntoClientArea(window.Handle, ref margins);
                }
            };
            captionBar.MouseDown += delegate (object sender, MouseEventArgs e)
            {
                ReleaseCapture();
                SendMessage(window.Handle, 161, 2, 0);
            };
        }
        public void CreateDropShadow(Form window, Control[] captionControls)
        {
            window.Paint += delegate (object sender, PaintEventArgs e)
            {
                var gfx = e.Graphics;
                if (m_aeroEnabled)
                {
                    var v = 2;
                    DwmSetWindowAttribute(window.Handle, 2, ref v, 4);
                    MARGINS margins = new MARGINS
                    {
                        BottomHeight = 1,
                        LeftWidth = 0,
                        RightWidth = 0,
                        TopHeight = 0
                    };
                    DwmExtendFrameIntoClientArea(window.Handle, ref margins);
                }
            };
            foreach (Control captionControl in captionControls)
            {
                captionControl.MouseDown += delegate (object sender, MouseEventArgs e)
                {
                    ReleaseCapture();
                    SendMessage(window.Handle, 161, 2, 0);
                };
            }
        }
        public void CreateDropShadow(Control control)
        {
            control.Paint += delegate (object sender, PaintEventArgs e)
            {
                var gfx = e.Graphics;
                if (m_aeroEnabled)
                {
                    var v = 2;
                    DwmSetWindowAttribute(control.Handle, 2, ref v, 4);
                    MARGINS margins = new MARGINS
                    {
                        BottomHeight = 1,
                        LeftWidth = 0,
                        RightWidth = 0,
                        TopHeight = 0
                    };
                    DwmExtendFrameIntoClientArea(control.Handle, ref margins);
                }
            };
        }

        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int enabled = 0;
                DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }

    }
}
