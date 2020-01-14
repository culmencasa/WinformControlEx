using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinformEx
{
    /// <summary>
    ///  VisualElementView类是包含控件(UIElement类型)的容器, 并负责所有子控件的重绘.
    /// </summary>
    public class UIElementHost : UIElement
    {
        #region 私有字段

        // 保存子元素的集合
        private UIElementCollection _children;
        // 控件"画布". 在指定的Control宿主上绘制子控件.
        private Control _canvas;

        #endregion
        
        #region 属性

        /// <summary>
        ///  表示VisualElementView实例包含的所有UIElement控件集合
        /// </summary>
        public UIElementCollection Children
        {
            get
            {
                return this._children;
            }
            set
            {
                this._children = value;
            }
        }

        /// <summary>
        ///  获取或设置控件绘制工作所属的宿主.
        /// </summary>
        public Control Canvas
        {
            get { return _canvas; }
            set { _canvas = value; }
        }

        #endregion

        #region 构造方法

        /// <summary>
        ///  实例化VisualElementView类. 
        /// </summary>
        public UIElementHost()          
        {
            this._children = new UIElementCollection();
            this._children.SetMember += new UIElementCollection.SetMemeberEventHandler( _children_SetMemberEventHandler );
        }
        /// <summary>
        ///  指定VisualElementView的画布.
        /// </summary>
        /// <param name="host"></param>
        public UIElementHost(Control canvashost) : this()
        {
            this._canvas = canvashost;
        }

        #endregion

        #region 公共方法

        /// <summary>
        ///  添加子元素 UIElement
        /// </summary>
        /// <typeparam name="T">UIElement类型</typeparam>
        /// <param name="buildFunc">创建并返回UIElement实例的方法</param>
        /// <returns>UIElement类型</returns>
        public T AddElement<T>(Func<T> buildFunc) where T : UIElement
        {
            if (buildFunc == null)
                throw new ArgumentNullException();

            T member = buildFunc();
            this._children.Add(member as UIElement);           
            return member;
        }

        public int AddElement(UIElement uicontrol)
        {
            _children.Add(uicontrol);
            return _children.Count - 1;
        }

        #endregion

        #region 重写的方法

        protected override void OnRender(Graphics graphics)
        {
            if (graphics.Clip.IsInfinite(graphics))
            {
                foreach (UIElement element in _children)
                {
                    if (element == null || !element.Visible)
                    {
                        continue;
                    }
                    element.Render(graphics);
                }
            }
            else
            {
                Rectangle gxRect = Rectangle.Ceiling(graphics.ClipBounds);
                Region gxClipBounds = new Region(gxRect);

                // 传递Graphics对象给子控件
                foreach (UIElement element in _children)
                {
                    if (element == null || !element.Visible)
                    {
                        continue;
                    }
                    // 不绘制可见区域以外的部分
                    Rectangle clipRect = Rectangle.Intersect(gxRect, element.Bounds);
                    if (clipRect.IsEmpty)
                    {
                        continue;
                    }
                    else
                    {
                        graphics.Clip = new Region(clipRect);
                    }

                    element.Render(graphics);
                }

                graphics.Clip = gxClipBounds;
            }
        }

        #endregion

        #region 私有方法

        // 重绘UIElement集合的成员
        private void InvalidateChild(UIElement element)
        {
            if (this._canvas != null)
            {
                this._canvas.Invalidate( element.Bounds );                
            }
        }      

        // 为UIElement集合的成员设置属性和事件方法
        private void _children_SetMemberEventHandler(UIElement obj)
        {
            obj.Parent = this;            
            obj.Invalidate += delegate(object sender, EventArgs args)
            {
                this.InvalidateChild( sender as UIElement );
            };
        }

        #endregion
    }
}
