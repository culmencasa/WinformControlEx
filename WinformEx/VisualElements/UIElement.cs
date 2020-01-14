using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace WinformEx
{
    public abstract class UIElement
    {
        #region 静态成员

        static string applicationPath;

        internal static string ApplicationPath
        {
            get
            {
                if (applicationPath == null)
                {
                    applicationPath = System.IO.Path.GetDirectoryName
                            ( System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase );
                }

                return applicationPath;
            }
        }

        #endregion

        #region 事件

        public event MouseEventHandler MouseMove;
        public event MouseEventHandler MouseDown;
        public event MouseEventHandler MouseUp;
        public event EventHandler Click;
        public event EventHandler Invalidate;

        #endregion

        #region 构造方法

        public UIElement()
        {
            _name = "";
            _visible = true;
            _enabled = true;
        }

        #endregion

        #region 字段

        private bool _visible;
        private bool _enabled;
        private string _name;
        private int _height;
        private int _width;
        private int _left;
        private int _top;

        private Color _foreground;
        private Color _background;

        private UIElement _parent;

        #endregion

        #region 属性

        public virtual int Left
        {
            get
            {
                return _left;
            }
            set
            {
                _left = value;
            }
        }

        public virtual int Top
        {
            get
            {
                return _top;
            }
            set
            {
                _top = value;
            }
        }

        public virtual int Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }

        public virtual int Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
            }
        }

        public virtual Point Location
        {
            get
            {
                return new Point(this._left, this._top);
            }
            set
            {
                this._left = value.X;
                this._top = value.Y;
            }
        }

        public virtual Rectangle Bounds
        {
            get
            {
                return new Rectangle(this._left, this._top, this._width, this._height);
            }
        }

        public virtual Rectangle ClientRectangle
        {
            get
            {
                return new Rectangle(0, 0, _width, _height);
            }
        }

        public virtual Size Size
        {
            get
            {
                return new Size(_width, _height);
            }
            set
            {
                _width = value.Width;
                _height = value.Height;
            }
        }

        public virtual UIElement Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
            }
        }

        public virtual string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public virtual bool Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                _visible = value;
            }
        }

        public virtual bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
            }
        }

        public virtual Color ForegroundColor
        {
            get
            {
                return _foreground;
            }
            set
            {
                if (this._foreground != value)
                {
                    this._foreground = value;
                    this.OnInvalidate();
                }
            }
        }

        public virtual Color BackgroundColor
        {
            get
            {
                return _background;
            }
            set
            {
                if (this._background != value)
                {
                    this._background = value;
                    this.OnInvalidate();
                }
            }
        }

        #endregion

        #region 公有方法

        public virtual bool HitTest(Point point)
        {
            return this.Bounds.Contains(point);
        }

        public void Render(Graphics graphics)
        {
            this.OnRender(graphics);
        }
        
        public void Render(PaintEventArgs e)
        {
            this.OnRender(e);
        }        

        public void InvokeHostRedraw()
        {
            if (this.Parent is UIElementHost)
            {
                Object HostParent = ((UIElementHost)this.Parent).Canvas;
                Type HostParentType = HostParent.GetType();
                System.Reflection.MethodInfo mi = HostParentType.GetMethod("Invalidate", new Type[] { typeof(Rectangle) });
                if (mi != null)
                {
                    mi.Invoke(HostParent, new object[] { this.Bounds });
                }
            }
        }

        #endregion

        #region 可继承方法

        /// <summary>
        /// 需要子类实现的方法
        /// </summary>
        /// <param name="graphics">Graphics对象</param>
        protected abstract void OnRender(Graphics graphics);

        protected virtual void OnRender(PaintEventArgs e)
        {
            OnRender(e.Graphics);
        }

        protected virtual void OnInvalidate(UIElement element)
        {
            if (this.Invalidate != null)
            {
                this.Invalidate(element, null);
            }
        }

        protected virtual void OnInvalidate()
        {
            if (this.Invalidate != null)
            {
                this.Invalidate(this, null);
            }
        }

        internal virtual void OnClick(EventArgs e)
        {
            if (this.Click != null)
            {
                this.Click(this, e);
            }
        }

        internal virtual void OnMouseDown(MouseEventArgs e)
        {
            if (this.MouseDown != null)
            {
                this.MouseDown(this, e);
            }
        }

        internal virtual void OnMouseUp(MouseEventArgs e)
        {
            if (this.MouseUp != null)
            {
                this.MouseUp(this, e);
            }
        }

        internal virtual void OnMouseMove(MouseEventArgs e)
        {
            if (this.MouseMove != null)
            {
                this.MouseMove(this, e);
            }
        }

        #endregion

    }
}
