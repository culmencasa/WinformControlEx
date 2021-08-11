using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.ComponentModel;

namespace System.Windows.Forms
{
    public  class TileIconList : FlowLayoutPanel
    {
        private int _borderSize = 1;

        public TileIconList()
        {
            //SetStyles();  //bug: win7下会闪烁，xp下却需要。

            SetStyles();
            this.AutoScroll = true;
            this.BorderColor = Color.FromArgb(207, 212, 216);
            this.BorderSize = 1;
        }


        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        var parms = base.CreateParams;
        //        parms.Style &= ~0x02000000;  // Turn off WS_CLIPCHILDREN
        //        return parms;
        //    }
        //}

        #region 属性

        public int BorderSize
        {
            get
            {
                return _borderSize;
            }
            set {
                _borderSize = value;
                this.Padding = new Padding(_borderSize);
            }
        }

        [DefaultValue(typeof(Color), "Color.Empty")]
        public Color BorderColor { get; set; }

        #endregion

        #region 私有方法
        
        protected virtual void SetStyles()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();
        }

        #endregion

        #region 公共方法
        
        public void CleanUp(bool dispose = true)
        {
            for (int i = this.Controls.Count - 1; i >= 0; i--)
            {
                var ctrl = this.Controls[i];
                this.Controls.Remove(ctrl);

                if (dispose)
                    ctrl.Dispose();
            }

        }

        public void Remove(Func<Control, bool> selector)
        {
            for (int i = this.Controls.Count - 1; i >= 0; i--)
            {
                var ctrl = this.Controls[i];
                if (selector(ctrl))
                {
                    this.Controls.Remove(ctrl);
                    break;
                }
            }
        }

        #endregion


        #region 重写的方法

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            Win32.ShowScrollBar(this.Handle, 0, false);
        }



        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);

            TileIcon newItem = e.Control as TileIcon;
            if (newItem == null)
            {
                this.Controls.Remove(newItem);
                return;
            }

            newItem.Width = this.Width - this.Padding.Left - this.Padding.Right;
            newItem.Dock = DockStyle.Top;
            newItem.MouseWheel += new MouseEventHandler(newItem_MouseWheel);

            if (this.VerticalScroll.Maximum > this.Height)
            {
                foreach (var item in this.Controls.OfType<TileIcon>())
                {
                    item.Width = this.Width - this.Padding.Left - this.Padding.Right - 20;
                }
            }
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);
            TileIcon newItem = e.Control as TileIcon;
            if (newItem != null)
            {
                newItem.MouseWheel -= new MouseEventHandler(newItem_MouseWheel);
            }


            if (!this.VerticalScroll.Visible)
            {
                foreach (var item in this.Controls.OfType<TileIcon>())
                {
                    item.Width = this.Width - this.Padding.Left - this.Padding.Right;
                }
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Control p = this.Parent;
            if (p != null)
            {
                Graphics g = p.CreateGraphics();
                if (this.BorderColor != Color.Empty)
                {
                    using (Pen borderPen = new Pen(this.BorderColor, _borderSize))
                    {
                        g.DrawRectangle(borderPen, new Rectangle(
                            this.Bounds.X - _borderSize, 
                            this.Bounds.Y - _borderSize,
                            this.Bounds.Width + _borderSize * 2,
                            this.Bounds.Height + _borderSize * 2));
                    }

                    //p.Invalidate();
                }
            }
        }

        #endregion

        #region 事件处理

        void newItem_MouseWheel(object sender, MouseEventArgs e)
        {
            OnMouseWheel(e);
        }

        #endregion

    }
}
