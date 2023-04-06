using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Utils.UI;

namespace System.Windows.Forms
{
    /// <summary>
    /// 阴影窗体
    /// </summary>
    public class DropShadow : Form
    {
        #region 字段

        private Bitmap _shadowBitmap;
        private float _shadowOpacity = 1;

        #endregion

        #region 构造

        public DropShadow(Form owner)
        {
            Owner = owner;
            ShadowRadius = 10;
            ShadowColor = Color.Black;
            ShowInTaskbar = false;

            // 点击穿透
            int wl = Win32.GetWindowLong(Handle, -20);
            wl = wl | 0x80000 | 0x20;
            Win32.SetWindowLong(Handle, -20, wl);

            FormBorderStyle = FormBorderStyle.None;

            // 绑定事件
            WireupOwnerEvents();

        }

        private void Owner_FormClosed(object? sender, FormClosedEventArgs e)
        {
            if (IsSelfAlive() && !IsOwnerAlive())
            {
                UnwireOwnerEvents();
                Close();
                Dispose();
            }
        }

        private void Owner_VisibleChanged(object? sender, EventArgs e)
        {
            if (Owner != null)
            {
                Visible = Owner.Visible;
            }
            else
            {
                Visible = false;
            }
        }


        #endregion

        #region 属性

        /// <summary>
        ///  阴影偏移
        ///  <para>默认阴影窗体扩大了20x20</para>
        /// </summary>
        public Point ShadowOffset { get; set; } = new Point(40, 40);

        /// <summary>
        ///  设置阴影颜色(需要调用Refresh)
        /// </summary>
        public Color ShadowColor { get; set; }

        /// <summary>
        ///  阴影图片
        /// </summary>
        public Bitmap ShadowBitmap
        {
            get { return _shadowBitmap; }
            set
            {
                _shadowBitmap = value;
                SetBitmap(_shadowBitmap, ShadowOpacity);
            }
        }

        /// <summary>
        ///  阴影半径(需要刷新)
        /// </summary>
        public int ShadowRadius { get; set; }

        /// <summary>
        ///  主窗体的边框半径(需要刷新)
        /// </summary>
        public int BorderRadius { get; set; }

        /// <summary>
        ///  主窗体的边框颜色
        /// </summary>
        public Color BorderColor { get; set; } = Color.DarkGray;


        /// <summary>
        ///  阴影透明度
        /// </summary>
        public float ShadowOpacity
        {
            get { return _shadowOpacity; }
            set
            {
                _shadowOpacity = value;
            }
        }

        public bool Blur { get; set; } = false;
        #endregion

        #region 重写的成员

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00080000; // WS_EX_LAYERED
                return cp;
            }
        }

        #endregion

        #region 公开的方法

        public void Redraw(bool redraw = true)
        {
            if (Owner == null)
            {
                return;
            }

            if (redraw)
            {
                ShadowBitmap = DrawShadowBitmap();
            }

            SetBitmap(ShadowBitmap, ShadowOpacity);
            UpdateLocation();

            if (Owner.Region == null)
            {
                IntPtr hrgn = Win32.CreateRoundRectRgn(0, 0, Owner.Width, Owner.Height, 0, 0);
                Owner.Region = System.Drawing.Region.FromHrgn(hrgn);
            } 

            // 设置显示区域
            Region shadowRegion = Region.FromHrgn(Win32.CreateRoundRectRgn(0, 0,
                ShadowBitmap.Width,
                ShadowBitmap.Height,
                BorderRadius,
                BorderRadius));
            
            Region ownerRegion = Owner.Region.Clone();
            ownerRegion.Translate(ShadowOffset.X, ShadowOffset.Y);

            // issue: 区域大小不正确
            //shadowRegion.Exclude(ownerRegion);

            shadowRegion.Exclude(ownerRegion);


            Region = shadowRegion;

            if (this.Owner.TopMost)
            {
                this.TopMost = true;
                this.Owner.TopMost = true;
            }
        }



        #endregion

        #region 私有方法

        private void WireupOwnerEvents()
        {
            if (IsOwnerAlive())
            {
                //Owner.Move += OnLocationChanged;
                Owner.Shown += Owner_Shown;
                Owner.LocationChanged += OnLocationChanged;
                Owner.Resize += Owner_Resize; // issue: win11下Resize事件不触发
                Owner.ResizeBegin += Owner_ResizeBegin;
                Owner.ResizeEnd += Owner_ResizeEnd;
                Owner.FormClosed += Owner_FormClosed;
                Owner.VisibleChanged += Owner_VisibleChanged;
            }
        }

        private void Owner_Shown(object sender, EventArgs e)
        {
            Redraw(true);
        }


        private void UnwireOwnerEvents()
        {
            if (IsOwnerAlive())
            {
                //Owner.Move -= OnLocationChanged;
                Owner.Shown -= Owner_Shown;
                Owner.LocationChanged -= OnLocationChanged;
                Owner.Resize -= new EventHandler(Owner_Resize);
                Owner.ResizeBegin -= Owner_ResizeBegin;
                Owner.ResizeEnd -= Owner_ResizeEnd;
                Owner.VisibleChanged -= Owner_VisibleChanged;
                Owner.FormClosed -= Owner_FormClosed;
            }
        }

        private void DrawFadeRectangle(Graphics g, RectangleF wrapRect, Color fadeColor, int shadowRadius)
        {
            using (GraphicsPath path = g.GenerateRoundedRectangle(wrapRect, shadowRadius, RectangleEdgeFilter.All))
            using (var pthGrBrush3 = new PathGradientBrush(path))
            {
                pthGrBrush3.CenterPoint = new PointF(Owner.Width / 2f, Owner.Height / 2f);
                pthGrBrush3.CenterColor = Color.Transparent;
                pthGrBrush3.SurroundColors = new[] { fadeColor };
                g.FillPath(pthGrBrush3, path);
            }

        }

        private void SetBitmap(Bitmap bitmap, float opacity)
        {
            GraphicsExtension.SelectBitmapIntoLayerWindow(this, bitmap, (byte)(opacity * 255));
        }

        /// <summary>
        /// 生成阴影图片
        /// </summary>
        /// <returns></returns>
        private Bitmap DrawShadowBitmap()
        {
            Win32.RECT rect = new Win32.RECT();
            bool s = Win32.GetWindowRect(Owner.Handle, ref rect);

            // 阴影窗体的大小 (画板要比Owner窗体大才能看到效果)
            int width = Owner.Width + ShadowOffset.X * 2;
            int height = Owner.Height + ShadowOffset.Y * 2;
            var bitmap = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                Color fadeColor = ShadowColor;

                // 从bitmap中心向外扩散
                RectangleF wrapRect = new RectangleF(
                        (bitmap.Width - Owner.Width) / 2,
                        (bitmap.Height - Owner.Height) / 2,
                        Owner.Width, 
                        Owner.Height);

                // 画10个阴影形成渐变
                int repeat = 10;
                int alpha = 10;
                if (Owner.Width < 300 || Owner.Height < 200)
                {
                    repeat = 5;
                    alpha = 5;
                }
                for (int i = 0; i < repeat; i++)
                {
                    wrapRect.Inflate(1, 1);
                    fadeColor = Color.FromArgb(alpha - i, 0, 0, 0);
                    DrawFadeRectangle(g, wrapRect, fadeColor, this.ShadowRadius + i * 2);
                }

                Width = width;
                Height = height;
            }


            Bitmap reproccessBitmap = bitmap;
            // 模糊
            if (Blur)
            {
                var blur = new SuperfastBlur.GaussianBlur(reproccessBitmap);
                reproccessBitmap = blur.Process(4);
            }

            // 画上边框
            using (var g = Graphics.FromImage(reproccessBitmap))
            {
                // issue: 当borderSize大于1时, 计算有点问题
                int borderSize = 1;
                using (Pen borderPen = new Pen(BorderColor, borderSize))
                {
                    g.DrawRoundedRectangle(borderPen,
                        (bitmap.Width - Owner.Width) / 2f - borderSize - 0.25f,
                        (bitmap.Height - Owner.Height) / 2f - borderSize - 0.25f,
                        Owner.Width * 1f + 0.25f,
                        Owner.Height * 1f + 0.25f,
                        ShadowRadius);
                }
            }

            return reproccessBitmap;
        }



        private void OnLocationChanged(Object sender = null, EventArgs eventArgs = null)
        {
            if (Owner == null)
                return;

            if (Owner.WindowState == FormWindowState.Normal)
            {
                UpdateLocation();
            }
        }

        private void UpdateLocation()
        {
            Point pos = Owner.Location;

            // 如果窗体(Owner)作为一个控件放在另一个窗体中
            if (Owner.Parent != null)
            {
                pos = Owner.PointToScreen(Point.Empty);
                //pos = Owner.Parent.PointToClient(pos);
                pos.Offset(-ShadowOffset.X, -ShadowOffset.Y);
                Location = pos;
            }
            else
            {
                pos.Offset(-ShadowOffset.X, -ShadowOffset.Y);
                Location = pos;
            }
        }
        #endregion

        #region 事件处理


        private void Owner_ResizeBegin(object sender, EventArgs e)
        {
            //this.Visible = false;
        }
        private void Owner_ResizeEnd(object sender, EventArgs e)
        {
            //if (Owner == null)
            //    return;

            //if (Owner.WindowState == FormWindowState.Minimized || Owner.WindowState == FormWindowState.Maximized)
            //{
            //    this.Visible = false;
            //    return;
            //}
            //else
            //{
            //    this.Visible = true;
            //}


            //Redraw(false);
        }

        protected FormWindowState OwnerLastWindowState { get; set; }
        private bool IsOwnerAlive()
        {
            if (Owner == null || Owner.IsDisposed)
                return false;

            if (!Owner.IsHandleCreated)
                return false;

            return true;
        }

        private bool IsSelfAlive()
        {
            if (this.IsDisposed)
                return false;

            if (!this.IsHandleCreated)
                return false;

            return true;
        }


        void Owner_Resize(object sender, EventArgs e)
        {
            if (!IsOwnerAlive())
            {
                this.Visible = false;
                return;
            }


            FormWindowState lastState = OwnerLastWindowState;
            if (Owner.WindowState != lastState)
            {
                OwnerLastWindowState = Owner.WindowState;
                //OnFormWindowStateChanged(new FormWindowStateArgs()
                //{
                //    LastWindowState = lastState,
                //    NewWindowState = WindowState
                //});

                if (OwnerLastWindowState == FormWindowState.Minimized)
                {
                    this.Visible = false;
                }
                else if (OwnerLastWindowState == FormWindowState.Normal)
                {
                    this.Visible = true;
                    //Owner.Region = null;
                    //Redraw(false);



                    UpdateLocation();
                }
                else if (OwnerLastWindowState == FormWindowState.Maximized)
                {
                    this.Visible = false;
                }
            }
            else
            {
                // 在AeroSnap后设为null重新计算Region
                Owner.Region = null;
                Redraw(true);
            }
        }

        #endregion


    }


}

