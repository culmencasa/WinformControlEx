using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
    public class DpiScaleForm : Form
    {
        /* 用户控件中如果存在一些硬编码像素大小的情景, 例如
         *  绘制基于像素大小的矩形, 指定大小的字体
         *  绘制指定像素位置的图像
         *  拉伸图标大小或者将一个32x32大小的图标放入PictureBox
         * 等等，需要自己处理缩放因子
        */
        private SizeF currentScaleFactor = new SizeF(1f, 1f);


        public DpiScaleForm()
        { 
            // AutoScale是.net1.0的旧属性，使用AutoScaleMode将启用.net2.0以后的缩放机制
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;

        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            base.ScaleControl(factor, specified);

            //Record the running scale factor used
            this.currentScaleFactor = new SizeF(
               this.currentScaleFactor.Width * factor.Width,
               this.currentScaleFactor.Height * factor.Height);


            // 通过调用Form.Scale()方法计算缩放因子（即当前DPI设置 / 96），该方法会递归调用子控件的ScaleControl()进行缩放。
            // 并非所有的子控件都能完好地缩放，例如ListView。这时需要自己处理

            //Toolkit.ScaleListViewColumns(listView1, factor);
        }




        #region Native

        public static short LOWORD(int number)
        {
            return (short)number;
        }

        const int WM_DPICHANGED = 0x02E0;

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetWindowPos(HandleRef hWnd, HandleRef hWndInsertAfter, int x, int y, int cx, int cy, int flags);

        #endregion

        #region Scaling

        private bool isMoving = false;
        private bool shouldScale = false;

        protected int oldDpi;
        protected int currentDpi;
        const int designTimeDpi = 96;

        protected override void OnResizeBegin(EventArgs e)
        {
            base.OnResizeBegin(e);
            this.isMoving = true;
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
            this.isMoving = false;
            if (shouldScale)
            {
                shouldScale = false;
                HandleDpiChanged();
            }
        }

        protected override void OnMove(EventArgs e)
        {
            base.OnMove(e);
            if (this.shouldScale && CanPerformScaling())
            {
                this.shouldScale = false;
                HandleDpiChanged();
            }
        }

        private bool CanPerformScaling()
        {
            Screen screen = Screen.FromControl(this);
            if (screen.Bounds.Contains(this.Bounds))
            {
                return true;
            }

            return false;
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                //This message is sent when the form is dragged to a different monitor i.e. when
                //the bigger part of its are is on the new monitor. Note that handling the message immediately
                //might change the size of the form so that it no longer overlaps the new monitor in its bigger part
                //which in turn will send again the WM_DPICHANGED message and this might cause misbehavior.
                //Therefore we delay the scaling if the form is being moved and we use the CanPerformScaling method to 
                //check if it is safe to perform the scaling.
                case WM_DPICHANGED:
                    oldDpi = currentDpi;
                    currentDpi = LOWORD((int)m.WParam);

                    if (oldDpi != currentDpi)
                    {
                        if (this.isMoving)
                        {
                            shouldScale = true;
                        }
                        else
                        {
                            HandleDpiChanged();
                        }

                        OnDPIChanged();
                    }

                    base.WndProc(ref m);
                    break;
                //case Win32API.WM_NCHITTEST:
                //    m.Result = (IntPtr)Win32API.HTCAPTION;
                //    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        private void HandleDpiChanged()
        {
            if (oldDpi != 0)
            {
                float scaleFactor = (float)currentDpi / oldDpi;

                //the default scaling method of the framework
                this.Scale(new SizeF(scaleFactor, scaleFactor));

                //fonts are not scaled automatically so we need to handle this manually
                this.ScaleFonts(scaleFactor);

                //perform any other scaling different than font or size (e.g. ItemHeight)
                this.PerformSpecialScaling(scaleFactor);
            }
            else
            {
                //the special scaling also needs to be done initially
                this.PerformSpecialScaling((float)currentDpi / designTimeDpi);
            }
        }

        protected virtual void PerformSpecialScaling(float scaleFactor)
        {
            //this.radClock1.ClockElement.ScaleTransform = new SizeF(scaleFactor, scaleFactor);
            //this.radTreeView1.ItemHeight = (int)(this.radTreeView1.ItemHeight * scaleFactor);
            //this.radTreeView1.TreeViewElement.Update(RadTreeViewElement.UpdateActions.StateChanged);
        }

        protected virtual void ScaleFonts(float scaleFactor)
        {
            //Go through all controls in the control tree and set their Font property
            //Note that this might not work with some RadElements which have the Font property
            //set via theme or a local setting and they need to be handled separately (e.g. TreeNodeElement)
            ScaleFontForControl(this, scaleFactor);
        }

        private static void ScaleFontForControl(Control control, float factor)
        {
            control.Font = new Font(control.Font.FontFamily,
                   control.Font.Size * factor,
                   control.Font.Style);

            foreach (Control child in control.Controls)
            {
                ScaleFontForControl(child, factor);
            }
        }

        protected virtual void OnDPIChanged()
        {
            //if (label1 != null)
            //    label1.Text = "Current DPI:" + currentDpi;
        }

        #endregion

    }
}
