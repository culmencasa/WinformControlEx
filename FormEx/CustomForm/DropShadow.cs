using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{

    public class DropShadow : Form
    {
        #region 字段

        private Bitmap _shadowBitmap;
        private float _shadowOpacity = 1;

        #endregion

        #region 构造

        public DropShadow(Form f)
        {
            Owner = f;
            ShadowRadius = 10;
            ShadowColor = Color.Black;
            ShowInTaskbar = false;

            // 点击穿透
            int wl = Win32.GetWindowLong(Handle, -20);
            wl = wl | 0x80000 | 0x20;
            Win32.SetWindowLong(Handle, -20, wl);

            FormBorderStyle = FormBorderStyle.None;

            // 绑定事件
            Owner.LocationChanged += UpdateLocation;
            Owner.Resize += new EventHandler(Owner_Resize);
            Owner.FormClosed += (sender, eventArgs) => 
            { 
                Close(); 
            };
            Owner.VisibleChanged += (sender, eventArgs) =>
            {
                if (Owner != null)
                {
                    Visible = Owner.Visible;
                }
                else
                {
                    Visible = false;
                }
            };


        }

        #endregion

        #region 属性

        /// <summary>
        ///  阴影偏移
        /// </summary>
        public Point ShadowOffset { get; set; }

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
        ///  边框半径(需要刷新)
        /// </summary>
        public int BorderRadius { get; set; }

        /// <summary>
        ///  阴影透明度
        /// </summary>
        public float ShadowOpacity
        {
            get { return _shadowOpacity; }
            set
            {
                _shadowOpacity = value;
                SetBitmap(ShadowBitmap, _shadowOpacity);
            }
        }

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
        
        public void UpdateLocation(Object sender = null, EventArgs eventArgs = null)
        {
            if (Owner == null)
                return;
            Point pos = Owner.Location;
            pos.Offset(ShadowOffset);
            pos.Offset(-ShadowRadius, -ShadowRadius);
            Location = pos;
        }

        public void Refresh(bool redraw = true)
        {
            if (Owner == null)
            {
                return;
            }

            if (redraw)
            {
                ShadowBitmap = DrawShadow();
            }

            SetBitmap(ShadowBitmap, ShadowOpacity);
            UpdateLocation();

            if (Owner.Region == null)
            {
                IntPtr hrgn = Win32.CreateRoundRectRgn(0, 0, Owner.Width + 1, Owner.Height + 1, 0, 0);
                Owner.Region = System.Drawing.Region.FromHrgn(hrgn);
            }

            // 设置显示区域
            Region shadowRegion = Region.FromHrgn(Win32.CreateRoundRectRgn(0, 0, Width, Height, BorderRadius, BorderRadius));
            Region ownerRegion = Owner.Region.Clone();
            ownerRegion.Translate(ShadowRadius, ShadowRadius);
            shadowRegion.Exclude(ownerRegion);
            Region = shadowRegion;

            if (this.Owner.TopMost)
            {
                this.TopMost = true;
                this.Owner.TopMost = true;
            }
        }


        public void SetBitmap(Bitmap bitmap, float opacity)
        {
            GraphicsExtension.SelectBitmapIntoLayerWindow(this, bitmap, (byte)(opacity * 255));
        }

        #endregion

        #region 私有方法

        private void DrawFadeRectangle(Graphics g, RectangleF wrapRect, Color fadeColor, int shadowRadius)
        {
            using (GraphicsPath path = g.GenerateRoundedRectangle(wrapRect, shadowRadius, RectangleEdgeFilter.All))
            using (var pthGrBrush3 = new PathGradientBrush(path))
            {
                pthGrBrush3.CenterPoint = new PointF(Owner.Width / 2f, Owner.Height / 2f);
                pthGrBrush3.CenterColor = ShadowColor;
                pthGrBrush3.SurroundColors = new[] { fadeColor };
                g.FillPath(pthGrBrush3, path);
            }

        }

        /// <summary>
        /// 需优化
        /// </summary>
        /// <returns></returns>
        private Bitmap DrawShadow()
        {
            // 阴影窗体的大小
            int width = Owner.Width + ShadowRadius * 2;
            int height = Owner.Height + ShadowRadius * 2;
            var bitmap = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bitmap);

            // 第一个阴影的位置
            RectangleF wrapRect = new RectangleF(
                    (bitmap.Width - Owner.Width) / 2 - 3,
                    (bitmap.Height - Owner.Height) / 2 - 3, Owner.Width + 4, Owner.Height + 4);
            using (Brush br = new SolidBrush(Color.FromArgb(50, 0, 0, 0)))
            {
                wrapRect.Inflate(-2, -2);
                //g.FillRoundedRectangle(br, wrapRect, ShadowRadius / 2, RectangleEdgeFilter.All);
                g.FillRoundedRectangle(br, wrapRect, ShadowRadius, RectangleEdgeFilter.All);
            }
            Color fadeColor = Color.FromArgb(200, 0, 0,0);
            //DrawFadeRectangle(g, wrapRect, fadeColor, this.ShadowRadius);
            // 层叠第二个阴影
            wrapRect.Inflate(4, 4);
            fadeColor = Color.FromArgb(10, 0, 0, 0);
            DrawFadeRectangle(g, wrapRect, fadeColor, this.ShadowRadius + 6);
            // 层叠第三个阴影
            wrapRect.Inflate(2, 2);
            fadeColor = Color.FromArgb(2, 0, 0, 0);
            DrawFadeRectangle(g, wrapRect, fadeColor, this.ShadowRadius + 10);
            // 层叠第四个阴影
            wrapRect.Inflate(1, 1);
            fadeColor = Color.FromArgb(1, 0,0, 0);
            DrawFadeRectangle(g, wrapRect, fadeColor, this.ShadowRadius + 16);

            Width = width;
            Height = height;

            g.Dispose();

            return bitmap;
        }

        #endregion

        #region 事件处理

        void Owner_Resize(object sender, EventArgs e)
        {
            if (Owner == null)
                return;

            if (Owner.WindowState == FormWindowState.Minimized || Owner.WindowState == FormWindowState.Maximized)
            {
                this.Visible = false;
                return;
            }
            else
            {
                this.Visible = true;
            }



            IntPtr hrgn = Win32.CreateRoundRectRgn(0, 0, Owner.Width + 1, Owner.Height + 1, 0, 0);
            Owner.Region = System.Drawing.Region.FromHrgn(hrgn);

            // 使用Invalidate而不是Refresh，以免影响主窗体上的控件背景显示白块
            this.Invalidate();
        }

        #endregion
    }


}

