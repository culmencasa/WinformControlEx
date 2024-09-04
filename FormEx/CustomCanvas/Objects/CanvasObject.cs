using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms.Canvas
{
    /// <summary>
    /// 画布元素基类
    /// </summary>
    public abstract class CanvasObject
    {
        #region 事件
         

        #endregion

        #region 字段

        protected bool _isSelected;
        protected bool _highLightState;
        protected CustomCanvas _canvas;
        protected Font _font = null;

        #endregion

        #region 属性

        #region 链接属性

        /// <summary>
        /// 画布对象
        /// </summary>
        [Category("元素链接")]
        public virtual CustomCanvas Canvas
        {
            get
            {
                return _canvas;
            }
            set
            {
                _canvas = value;
            }
        }

        #endregion
        
        #region 外观属性

        [Category("元素外观")]
        public virtual Font Font
        {
            get
            {
                // 如果字体为空，则使用画布字体
                if (_font == null)
                {
                    return GetFont();
                }
                else
                { 
                    return _font;
                }
            }
            set
            {
                var changed = _font != value;
                _font = value;
                if (changed)
                {
                    OnFontChanged();
                }
                
            }
        }


        /// <summary>
        /// 画布上坐标X
        /// </summary>
        [Category("元素外观")]
        public virtual float Left
        {
            get;
            set;
        }

        /// <summary>
        /// 画布上坐标Y
        /// </summary>
        [Category("元素外观")]
        public virtual float Top
        {
            get;
            set;
        }


        /// <summary>
        /// 宽度
        /// </summary>
        [Category("元素外观")]
        public virtual float Width
        {
            get;
            set;
        }

        /// <summary>
        /// 高度
        /// </summary>
        [Category("元素外观")]
        public virtual float Height
        {
            get;
            set;
        }

        /// <summary>
        /// 所在画布区域
        /// </summary>
        [Category("元素外观")]
        public virtual RectangleF Bounds
        {
            get
            {
                return new RectangleF(Left, Top, Width, Height);
            }
        }

        /// <summary>
        /// 位置
        /// </summary>
        [Category("元素外观")]
        public virtual PointF Location
        {
            get
            {
                return new PointF(Left, Top);
            }
            set
            {
                Left = value.X;
                Top = value.Y;
            }
        }

        /// <summary>
        /// 大小
        /// </summary>
        [Category("元素外观")]
        public virtual SizeF Size
        {
            get
            {
                return new SizeF(Width, Height);
            }
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        /// <summary>
        /// 前景色
        /// </summary>
        [Category("元素外观")]
        public virtual Color ForeColor
        {
            get;
            set;
        }

        /// <summary>
        /// 背景色
        /// </summary>
        [Category("元素外观")]
        public virtual Color BackColor
        {
            get;
            set;
        }

        #endregion

        #region 信息内容属性

        [Category("元素信息")]
        public virtual string Text
        {
            get;
            set;
        }
        /// <summary>
        /// 名称
        /// </summary>
        [Category("元素信息")]
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 显示的信息
        /// </summary>
        [Category("元素信息")]
        public virtual string ToolTip { get; set; }

        #endregion

        #region 行为属性

        /// <summary>
        /// 是否可见
        /// </summary>
        [Category("元素行为")]
        public virtual bool Visible
        {
            get;
            set;
        }

        /// <summary>
        /// 是否可用
        /// </summary>
        [Category("元素行为")]
        public virtual bool Enabled
        {
            get;
            set;
        }

        [Category("元素行为")]
        public virtual bool AllowMove
        {
            get;
            set;
        }

        [Category("元素行为")]
        public virtual bool AllowResize
        {
            get;
            set;
        }

        [Category("元素行为")]
        public virtual bool AllowSelect
        {
            get;
            set;
        }


        [Category("元素行为")]
        public virtual bool ShowToolTip
        {
            get;
            set;
        }

        #endregion

        #region 状态属性


        /// <summary>
        /// 是否选中
        /// </summary>
        [Category("元素状态")]
        public virtual bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                Render();
            }
        }

        /// <summary>
        /// 是否高亮
        /// </summary>
        [Category("元素状态")]
        public virtual bool HighlightState
        {
            get
            {
                return _highLightState;
            }
            set
            {
                _highLightState = value;
                OnHighlighStateChanged();
            }
        }

        /// <summary>
        /// Z轴顺序，越大越在上层
        /// </summary>
        [Category("元素状态")]
        public virtual int ZIndex
        {
            get;
            set;
        }

        #endregion


        #endregion

        #region 构造函数

        public CanvasObject()
        {
            Visible = true;
            Enabled = true;
            AllowMove = true;
            AllowResize = true;
            AllowSelect = true;

            ForeColor = Color.Black;
        }

        #endregion

        #region public 

        /// <summary>
        /// 渲染
        /// </summary>
        public virtual void Render()
        {
            if (Canvas != null && Canvas.IsHandleCreated)
            {
                Canvas.Invalidate();
            }
        }



        #endregion

        #region Internal方法

        /// <summary>
        /// 绘制自己
        /// </summary>
        /// <param name="graphics">Graphics对象</param>
        internal abstract void DrawContent(Graphics graphics);

        /// <summary>
        /// 是否命中测试
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        internal virtual bool HitTest(PointF point)
        {
            return Bounds.Contains(point);
        }

        internal virtual void OnParentChanged()
        { 
            
        }


        #endregion

        #region protected方法

        protected virtual void OnHighlighStateChanged()
        {

        }

        protected virtual Font GetFont()
        {
            if (Canvas == null)
            {
                throw new NullReferenceException("Canvas不能为空.");
            }

            return Canvas.Font;
        }  

        protected virtual void OnFontChanged()
        { 
            
        }
         

        #endregion
    }

}
