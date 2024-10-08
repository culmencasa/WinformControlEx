﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.ComponentModel;

namespace System.Windows.Forms
{
    [Browsable(false)]
    public class NonFlickerUserControl : UserControl
    {
        public NonFlickerUserControl()
        {
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            SetStyle(
                ControlStyles.UserPaint |
                //ControlStyles.DoubleBuffer
                /* .Net2.0设置Control.DoubleBuffered为true, 
                 *  ControlStyles.AllPaintingInWmPaint 和 
                 *  ControlStyles.OptimizedDoubleBuffer 的值
                 *  都会为 true.
                 */
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();

            /* 事件顺序
             * HandleCreated
             * Load
             * Layout
             * VisibleChanged
             * Paint
             */
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IgnoreParentFormTracking { get; set; }

        protected override CreateParams CreateParams
        {
            get
            {
                var parms = base.CreateParams;
                if (IgnoreParentFormTracking)
                    parms.Style &= ~0x02000000;  // Turn off WS_CLIPCHILDREN
                return parms;
            }
        }

        protected string _text;

        [Category(Consts.DefaultCategory)]
        [Browsable(true)] 
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Localizable(true)]
        [Bindable(true)]
        public override string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                Invalidate();
            }
        }



        /// <summary>
        /// 弃用
        /// </summary>
        public class Natives
        {

            #region 阻止重绘

            [DllImport("user32.dll")]
            public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

            private const int WM_SETREDRAW = 11;

            public static void SuspendDrawing(Control parent)
            {
                SendMessage(parent.Handle, WM_SETREDRAW, false, 0);
            }

            public static void ResumeDrawing(Control parent)
            {
                SendMessage(parent.Handle, WM_SETREDRAW, true, 0);
                parent.Refresh();
            }

            #endregion

        }
    }


}
