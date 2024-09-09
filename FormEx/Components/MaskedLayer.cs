using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace System.Windows.Forms
{
    public class MaskedLayer : Control
    {
        #region 构造函数


        public MaskedLayer(Form form)
        {
            this.DoubleBuffered = true;


            Attach(form);


        }

        #endregion

        #region 字段

        private float _circleRadius = 0f; // 圆的半径
        private int _maxRadius; // 最大半径 
        private float _velocity = 25f; // 初始速度
        private float _velocityIncrement = 10f; // 每次 Tick 的速度增量

        private int _layerOpacity = 220; // 背景透明度
        private Color _layerColor = Color.Black;

        private Thread _animationThread;
        private bool _isAnimating;

        private Bitmap _drawingImageBuffer;
        private Bitmap _ownerScreenshot;

        protected FormWindowState OwnerLastWindowState;

        #endregion

        #region 属性


        [Category(Consts.DefaultCategory)]
        [DefaultValue(typeof(Color), "Black")]
        public Color LayerColor { get => _layerColor; set => _layerColor = value; }


        [Category(Consts.DefaultCategory)]
        [DefaultValue(220)]
        public int LayerOpacity
        {
            get
            { 
                return _layerOpacity;
            }
            set
            { 
                _layerOpacity = value;
            }
        }

        [Browsable(false)]
        public Form Owner { get; protected set; }


        #endregion

        #region 公开方法

        public new void Show()
        {
            ConfigureTimerAndVelocity();
            //_animationTimer.Start();


            _isAnimating = true;
            _animationThread = new Thread(Animate);
            _animationThread.Start();
        }

        public void Close()
        {
            _isAnimating = false;
            if (_animationThread != null && _animationThread.IsAlive)
            {
                _animationThread.Join(); // 等待线程结束
            }

        }
         


        #endregion

        #region Owner相关方法

        protected void Attach(Form owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException();
            }
            if (Owner == owner)
            {
                return;
            }

            if (owner.TopLevel == false)
            {
                throw new Exception("只能使用顶层窗体.");
            }

            Owner = owner;
            Owner.Activated += Owner_Activated;
            Owner.ResizeEnd += Owner_ResizeEnd;
            Owner.Resize += Owner_Resize;

            CopyOwnerScreenshot();
            Owner.Controls.Add(this);
            this.BringToFront();
            this.Dock = DockStyle.Fill;

            // 计算最大半径
            _maxRadius = (int)Math.Sqrt(Math.Pow(Owner.Width, 2) + Math.Pow(Owner.Height, 2));

        }

        protected void Owner_Resize(object sender, EventArgs e)
        {
            FormWindowState lastState = OwnerLastWindowState;
            if (Owner.WindowState != lastState)
            {
                OwnerLastWindowState = Owner.WindowState;
                if (Owner.WindowState == FormWindowState.Maximized)
                {
                    Owner.Controls.Remove(this);
                    Owner.Refresh();  
                    CopyOwnerScreenshot();
                    Owner.Controls.Add(this);
                    this.Dock = DockStyle.Fill;
                    this.BringToFront();

                    DrawMaxiumImageBuffer(); 
                    Invalidate();
                }
            }
        }

        protected void Owner_ResizeEnd(object sender, EventArgs e)
        {
            Owner.Controls.Remove(this); 
            CopyOwnerScreenshot();
            Owner.Controls.Add(this);
            this.Dock = DockStyle.Fill;
            this.BringToFront();

            DrawMaxiumImageBuffer();
            Invalidate();
        }

        protected void Detach()
        {
            if (Owner != null && Owner.IsHandleCreated)
            {
                Owner.Activated -= Owner_Activated;
                Owner.ResizeEnd -= Owner_ResizeEnd;
                Owner.Resize -= Owner_Resize;
                Owner.Controls.Remove(this);
            }
        }


        protected void Owner_Activated(object sender, EventArgs e)
        {
            // 防止其他控件抢焦点（例如ListBox)
            this.BringToFront();
            Invalidate();
        }

        protected void CopyOwnerScreenshot()
        {
            if (Owner.ClientRectangle.Width == 0 || Owner.ClientRectangle.Height == 0)
            {
                return;
            }

            // 获取窗口和客户区的矩形
            RECT windowRect, clientRect;
            GetWindowRect(Owner.Handle, out windowRect);
            GetClientRect(Owner.Handle, out clientRect);

            // 计算客户区的宽度和高度
            int clientWidth = clientRect.Right - clientRect.Left;
            int clientHeight = clientRect.Bottom - clientRect.Top;

            // 计算窗口的整体宽度和高度
            int windowWidth = windowRect.Right - windowRect.Left;
            int windowHeight = windowRect.Bottom - windowRect.Top;

            // 创建整个窗口的截图
            using (Bitmap fullWindowBitmap = new Bitmap(windowWidth, windowHeight))
            {
                using (Graphics fullG = Graphics.FromImage(fullWindowBitmap))
                {
                    IntPtr fullHDC = fullG.GetHdc();
                    PrintWindow(Owner.Handle, fullHDC, 0);
                    fullG.ReleaseHdc(fullHDC);
                }

                // 计算客户区在窗口中的相对位置(未测试DPI情况）
                int borderWidth = (windowWidth - clientWidth) / 2;
                int titleHeight = Owner.GetTitleBarHeight();

                // 创建客户区的位图并裁剪内容
                _ownerScreenshot = new Bitmap(clientWidth, clientHeight);
                using (Graphics g = Graphics.FromImage(_ownerScreenshot))
                {
                    g.SetFastRendering();
                    //g.Clear(Color.FromArgb(_layerOpacity, LayerColor));

                    // 裁剪客户区部分
                    Rectangle srcRect = new Rectangle(borderWidth, titleHeight, clientWidth, clientHeight);
                    g.DrawImage(fullWindowBitmap, 0, 0, srcRect, GraphicsUnit.Pixel);
                }
            }
        }

        #endregion

        #region 动画

        private void ConfigureTimerAndVelocity()
        {
            int formWidth = Owner.Width;
            int formHeight = Owner.Height;
             

            // 固定重绘次数为20
            int tickCount = 20;
             

            // 计算所需的总增量
            float totalIncrement = _maxRadius / tickCount;

            // 设置每次 Tick 的速度增量
            _velocityIncrement = totalIncrement;

            // 根据窗体大小调整速度
            if (formWidth >= 1920 && formHeight >= 1080)
            {
                _velocityIncrement *= 1.5f; // 大分辨率下增加增量
            }
            else if (formWidth >= 1280 && formHeight >= 720)
            {
                // 中等分辨率，不需要额外调整
            }
            else
            {
                _velocityIncrement *= 0.75f; // 小分辨率下减少增量
            }

        }


        private void Animate()
        {
            DateTime startTime = DateTime.Now;
            while (_isAnimating)
            {
                _circleRadius += _velocityIncrement;

                if (_circleRadius > _maxRadius + 1)
                {
                    _circleRadius = _maxRadius + 1;
                    _isAnimating = false;
                    return;
                }

                try
                {
                    RedrawImageBuffer();
                    Invoke(new Action(() => Invalidate())); // 需要在UI线程中调用Invalidate
                }
                catch
                {
                    // 太快会对象占用冲突, 异常吞掉
                }
                finally
                {

                    Thread.Sleep(5);
                }
            }
        }


        #endregion

        #region 重绘

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics; 
            if (_drawingImageBuffer != null)
            {
                g.DrawImage(_drawingImageBuffer, 0, 0);
                //Debug.WriteLine(_drawingImageBuffer.Width + " ," + _drawingImageBuffer.Height);
            }
        }



        private void RedrawImageBuffer()
        {
            if (_drawingImageBuffer == null || _drawingImageBuffer.Size != this.ClientSize)
            {
                _drawingImageBuffer = new Bitmap(this.Width, this.Height);
            }

            using (Graphics g = Graphics.FromImage(_drawingImageBuffer))
            {
                g.DrawImage(_ownerScreenshot, 0, 0);
                g.SetFastRendering();

                // 绘制圆形遮罩
                using (Brush circleBrush = new SolidBrush(Color.FromArgb(_layerOpacity, LayerColor)))
                {
                    float startX = this.Width / 2 - _circleRadius;
                    float startY = this.Height - _circleRadius;

                    g.FillEllipse(circleBrush, startX, startY, _circleRadius * 2, _circleRadius * 2);
                }
            }
        }

        // 切换到最大化时直接绘制遮罩
        private void DrawMaxiumImageBuffer()
        {
            if (_drawingImageBuffer == null || _drawingImageBuffer.Size != this.ClientSize)
            {
                _drawingImageBuffer = new Bitmap(this.Width, this.Height);
            }

            using (Graphics g = Graphics.FromImage(_drawingImageBuffer))
            {
                g.SetFastRendering();
                g.DrawImage(_ownerScreenshot, 0, 0);

                // 绘制遮罩
                using (Brush brush = new SolidBrush(Color.FromArgb(_layerOpacity, LayerColor)))
                {
                    g.FillRectangle(brush, 0, 0, this.Width, this.Height);
                }
            }
        }

        #endregion

        #region WINAPI

        [DllImport("user32.dll")]
        private static extern bool PrintWindow(IntPtr hwnd, IntPtr hdcBlt, uint nFlags);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);


        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        #endregion
    }
}
