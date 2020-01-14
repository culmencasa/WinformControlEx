using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinformEx
{
    public class UIFormCanvas : Form
    {
        #region 事件
        
        /// <summary>
        ///  每当窗体第一次显示时发生
        /// </summary>
        public new event EventHandler Shown;

        #endregion

        #region 字段

        private Image _backgroundImage;
        private UIElementHost _canvasContainer;
        private int _previousIndex = -1;
        private bool _isFirstTime = true;
        private bool _screenShot; // 指示是否已绘过图
        private Rectangle _redrawBounds = Rectangle.Empty;

        protected Bitmap _offScreenBitmap;
        protected Graphics _offScreenGraphics;


        #endregion

        #region 构造方法

        public UIFormCanvas()
        {
            _canvasContainer = new UIElementHost(this);

            //this.Font = new Font("微软雅黑", 12, FontStyle.Regular);
            this.Activated += new EventHandler(UIForm_Activated);
            this.Closing += new System.ComponentModel.CancelEventHandler(UIForm_Closing);

            ResetOffscreenBitmap();
        }


        #endregion

        #region 属性

        /// <summary>
        ///  获取窗体的控件集合
        /// </summary>
        public UIElementHost Canvas
        {
            get
            {
                return this._canvasContainer;
            }
        }

        public new Image BackgroundImage
        {
            get
            {
                return this._backgroundImage;
            }
            set
            {
                this._backgroundImage = value;
            }
        }

        internal Bitmap ScreenShot
        {
            get
            {
                if (_screenShot)
                {
                    return _offScreenBitmap;
                }
                else
                {
                    return (Bitmap)_backgroundImage;
                }
            }
        }

        #endregion

        #region 虚方法

        public new virtual void Invalidate(Rectangle bounds)
        {
            _redrawBounds = bounds;
            base.Invalidate(bounds);
        }

        private void ResetOffscreenBitmap()
        {
            if (_offScreenBitmap != null)
            {
                // 创建新的Grahpics对象
                if (_offScreenBitmap.Width != this.Width || _offScreenBitmap.Height != this.Height)
                {
                    _offScreenBitmap.Dispose();
                    _offScreenGraphics.Dispose();
                    _offScreenBitmap = new Bitmap(this.Width, this.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                    _offScreenGraphics = Graphics.FromImage(_offScreenBitmap);
                }
            }
            else
            {
                _offScreenBitmap = new Bitmap(this.Width, this.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                _offScreenGraphics = Graphics.FromImage(_offScreenBitmap);
            }
        }

        #endregion

        #region 重写方法
 
        protected override void OnResize(EventArgs e)
        {
            // Resize发生在OnPaint之前, 先清掉原有的图像
            ResetOffscreenBitmap();

            base.OnResize(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // 防止闪烁, 在OnPaint中处理背景图片
        }
       
        protected override void OnPaint(PaintEventArgs e)
        {
            if (_offScreenGraphics == null)
            {
                base.OnPaint(e);
                return;
            }

            Rectangle RefreshArea = Rectangle.Empty;
            // 调用Invalidate(Rectange)方法时
            if (_redrawBounds != Rectangle.Empty)
            {
                RefreshArea = _redrawBounds;
                _offScreenGraphics.Clip = new Region(RefreshArea);
                // 画设为无效的区域背景
                this._offScreenGraphics.Clear(this.BackColor);
                if (_backgroundImage != null)
                {
                    _offScreenGraphics.DrawImage(
                        _backgroundImage,
                        RefreshArea.Left,
                        RefreshArea.Top,
                        RefreshArea,
                        GraphicsUnit.Pixel);
                }

                // 传递Grahpics对象给子元素重绘
                if (this._canvasContainer != null)
                {
                    this._canvasContainer.Render(_offScreenGraphics);
                }

                e.Graphics.DrawImage(_offScreenBitmap, 0, 0);
                _redrawBounds = Rectangle.Empty;
                _screenShot = true;
            }
            else
            {
                RefreshArea = this.ClientRectangle;
                _offScreenGraphics.Clip = new Region(RefreshArea);

                this._offScreenGraphics.Clear(this.BackColor);
                // 画背景
                if (this._backgroundImage != null)
                {
                    _offScreenGraphics.DrawImage(_backgroundImage, 0, 0);
                }
                // 传递Grahpics对象给子元素重绘
                if (this._canvasContainer != null)
                {
                    this._canvasContainer.Render(_offScreenGraphics);
                }

                e.Graphics.DrawImage(_offScreenBitmap, 0, 0);
                _screenShot = true;

                base.OnPaint(e);
            }

        }

        protected override void OnClick(EventArgs e)
        {
            foreach (UIElement element in this._canvasContainer.Children)
            {
                // EventArgs不包含鼠标坐标, 这里需要转换屏幕坐标为客户端坐标
                if (element.HitTest(PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y))))
                {
                    element.OnClick(EventArgs.Empty);
                    break;
                }
            }
            //Invalidate();
            base.OnClick(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            int i = 0;
            foreach (UIElement element in this._canvasContainer.Children)
            {
                if (element.HitTest(new Point(e.X, e.Y)))
                {
                    _previousIndex = i;
                    element.OnMouseDown(e);
                    //Invalidate();
                    break;
                }
                i++;
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (_previousIndex != -1)
            {
                _canvasContainer.Children[_previousIndex].OnMouseUp(e);
                _previousIndex = -1;
                //Invalidate();
            }
            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            // 编写消息发送给UIMessageWindow处理            
            //Message msg = Message.Create(_msgWin.Hwnd, UIMessageWindow.WM_CUSTOMMSG, (IntPtr)e.X, (IntPtr)e.Y);
            //MessageWindow.SendMessage(ref msg);
            if (_previousIndex != -1)
            {
                _canvasContainer.Children[_previousIndex].OnMouseMove(e);
                //Invalidate();
            }
            base.OnMouseMove(e);
        }

        #endregion

        #region 事件处理方法

        void UIForm_Activated(object sender, EventArgs e)
        {
            if (_isFirstTime)
            {
                if (Shown != null)
                {
                    Shown(sender, e);
                }
            }

            _isFirstTime = false;
        }

        void UIForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.Font != null)
            {
                this.Font.Dispose();
                this.Font = null;
            }
        }

        #endregion

        #region 公共方法

        public void Minimize()
        {
            int SW_MINIMIZE = 6;
            Win32.ShowWindowAsync(this.Handle, SW_MINIMIZE);
        }

        #endregion
    }
}
