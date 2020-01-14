using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Reflection;

namespace System.Windows.Forms
{
    /// <summary>
    /// 图像占位符
    /// </summary>
    public class ImagePlaceholder : Control
    {
        public enum TextLocations
        { 
            Bottom = 0,
            Top,
            
        }

        /// <summary>
        /// 图像依据控件大小的变化模式
        /// </summary>
        public enum SizeModes
        { 
            /// <summary>
            /// 拉伸
            /// </summary>
            Stretch = 0,
            /// <summary>
            /// 居中
            /// </summary>
            Center = 1,
            /// <summary>
            /// 无
            /// </summary>
            None = 2,
        }

        #region 字段

        Bitmap _offscreenBackgroundImage;
        SizeModes _sizeMode;
        bool _hidden;
        bool _useOwnPaint = true;

        #endregion
        
        #region 构造

        public ImagePlaceholder()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | 
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.DoubleBuffer | 
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.Selectable, true);
            UpdateStyles();

            this.Size = new Size(75, 75);
            this.BackColor = Color.Transparent;
            this.Visible = _useOwnPaint;
        }

        #endregion

        #region 属性

        internal Control LastParent { get; set; }
        
        /// <summary>
        /// 正常图像
        /// </summary>
        [DefaultValue(null)]
        public Image NormalImage { get; set; }

        /// <summary>
        /// 悬浮图像
        /// </summary>
        [DefaultValue(null)]
        public Image HoverImage { get; set; }

        /// <summary>
        /// 按下图像
        /// </summary>
        [DefaultValue(null)]
        public Image PressImage { get; set; }

        /// <summary>
        /// 是否悬浮
        /// </summary>
        [Browsable(false)]
        protected bool Hovering { get; set; }

        /// <summary>
        /// 是否按下
        /// </summary>
        [Browsable(false)]
        protected bool Pressing { get; set; }

        /// <summary>
        /// 使用父控件的Background图像作为自身的图像（false时复制父控件背景）
        /// </summary>
        public bool UseParentDesignBg { get; set; }

        /// <summary>
        /// 设置是否自绘制，为false时会为父控件添加绘图事件，
        /// </summary>
        public bool UseOwnPaint {
            get
            {
                return _useOwnPaint;
            }
            set
            {
                _useOwnPaint = value;
                if (!DesignMode)
                {
                    this.Visible = value;
                }
            }
        }

        /// <summary>
        /// 是否隐藏（UseOwnPaint为false时有效）
        /// </summary>
        public bool Hidden
        {
            get
            {
                return _hidden;
            }
            set
            {
                _hidden = value;
            }
        }

        /// <summary>
        /// 控件的文本
        /// </summary>
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                ForceRefresh();
            }
        }

        /// <summary>
        /// 文本的位置（上或下）
        /// </summary>
        public TextLocations TextLocation { get; set; }

        /// <summary>
        /// 获取或设置图像在控件上是拉伸还是居中显示
        /// </summary>
        public SizeModes SizeMode
        {
            get
            {
                return _sizeMode;
            }
            set
            {
                _sizeMode = value;
            }
        }

        /// <summary>
        /// 是否显示边框
        /// </summary>
        public bool ShowBorder
        {
            get;
            set;
        }

        /// <summary>
        /// 边框色
        /// </summary>
        public Color BorderColor
        {
            get;
            set;
        }

        /// <summary>
        /// 控件的实际区域（滚动容器中）
        /// </summary>
        public Rectangle ActualBounds
        {
            get
            {
                Rectangle actualBounds = this.Bounds;

                // 如果是在可滚动的容器内， 计算控件实际偏移后的位置
                if (this.Parent as ScrollableControl != null)
                {
                    ScrollableControl parent = this.Parent as ScrollableControl;
                    actualBounds = new Rectangle(
                        this.Bounds.X - parent.HorizontalScroll.Value,
                        this.Bounds.Y - parent.VerticalScroll.Value,
                        this.Bounds.Width,
                        this.Bounds.Height);
                }

                return actualBounds;
            }
        }



        #endregion

        #region 重写的成员

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            if (DesignMode)
                return;
            
            if (UseOwnPaint)// 还有问题未解决
            {
                //if (this.Parent != null && this.LastParent != null && this.Parent.Name != this.LastParent.Name)
                //{
                //    LastParent.Paint -= new PaintEventHandler(ImagePlaceholderParent_Paint);
                //    LastParent = this.Parent;
                //} 

                //if (this.Parent != null)
                //{
                //    // 如果未注册过Paint事件，则注册。如果已注册过，则不重复注册。
                //    if (!IsEventHandlerRegistered("ImagePlaceholderParent_Paint"))
                //    {
                //        this.Parent.Paint += new PaintEventHandler(ImagePlaceholderParent_Paint);
                //    }
                //}
                this.Visible = true;
                return;
            }

            if (this.Parent != null && this.LastParent != null && this.Parent.Name != this.LastParent.Name)
            {
                LastParent.Paint -= new PaintEventHandler(ImagePlaceholderParent_Paint);
                LastParent.MouseEnter -= new EventHandler(Parent_MouseEnter);
                LastParent.MouseMove -= new MouseEventHandler(Parent_MouseMove);
                LastParent.MouseDown -= new MouseEventHandler(Parent_MouseDown);
                LastParent.MouseUp -= new MouseEventHandler(Parent_MouseUp);
                LastParent = this.Parent;
            } 
            if (this.Parent != null)
            {
                // 如果未注册过Paint事件，则注册。如果已注册过，则不重复注册。
                if (!IsEventHandlerRegistered("ImagePlaceholderParent_Paint"))
                {
                    this.Parent.Paint += new PaintEventHandler(ImagePlaceholderParent_Paint);
                }
                this.Parent.MouseEnter += new EventHandler(Parent_MouseEnter);
                this.Parent.MouseMove += new MouseEventHandler(Parent_MouseMove);
                this.Parent.MouseDown += new MouseEventHandler(Parent_MouseDown);
                this.Parent.MouseUp += new MouseEventHandler(Parent_MouseUp);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //if (DesignMode)
            //{
            //    base.OnPaintBackground(e);
            //} 

            if (UseOwnPaint)
                base.OnPaintBackground(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // 禁用重绘事件，仅在设计器模式下启用
            if (DesignMode || UseOwnPaint)
            {
                base.OnPaint(e);
                Graphics g = e.Graphics;
                g.Clear(this.BackColor);

                if (Parent != null && this.BackColor == Color.Transparent)
                {
                    //g.Clear(Parent.BackColor);
                    // 找到有背景图像的上级控件
                    Control parentThatOwnsBgImage = Parent;
                    Image bgImage = Parent.BackgroundImage;
                    if (parentThatOwnsBgImage.BackgroundImage == null && parentThatOwnsBgImage.BackColor == Color.Transparent)
                    {
                        while (parentThatOwnsBgImage != null)
                        {
                            parentThatOwnsBgImage = parentThatOwnsBgImage.Parent;
                            if (parentThatOwnsBgImage != null 
                                && parentThatOwnsBgImage.BackgroundImage != null)
                            {
                                bgImage = parentThatOwnsBgImage.BackgroundImage;
                                break;
                            }
                        }
                    }


                    if (bgImage != null)
                    {
                        // 算控件在上级控件（有背景图像的上级容器）中的位置，算得不对，需要修改
                        Rectangle boundsInParent = parentThatOwnsBgImage.RectangleToClient(this.ActualBounds); 
                        g.DrawImage(bgImage, this.ClientRectangle, boundsInParent.X, boundsInParent.Y, this.Width, this.Height, GraphicsUnit.Pixel);
                    }
                }

                if (this.BackgroundImage != null)
                {
                    g.DrawImage(this.BackgroundImage, 0, 0);
                }

                if (this.NormalImage != null)
                {
                    g.DrawImage(this.NormalImage, this.ClientRectangle);
                }

            }
        }

        #endregion

        #region 内部方法

        internal void ForceRefresh()
        {
            if (DesignMode)
                return;

            if (this.Parent != null)
            {
                this.Parent.Invalidate(this.Bounds);
            }
        }

        internal virtual void DrawOnParentGraphics(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.None;
            Rectangle actualBounds = this.ActualBounds;

            if (!DesignMode)
            {

                g.Clip = new Region(actualBounds);

                if (!UseParentDesignBg)
                {
                    #region 画背景
                    // 1. 如果背景缓存从未创建，则表示父控件还未完成第一次绘图事件。
                    // 所以当背景缓存不为空的判断要放在前面，以免背景缓存复制失败。
                    if (_offscreenBackgroundImage != null) 
                    {
                        // bug: 如果控件有一部分在父控件显示区域外，滚动到全部显示时，会发现背景不对。
                        g.Clear(_offscreenBackgroundImage.GetPixel(1, 1));
                        g.DrawImage(_offscreenBackgroundImage, actualBounds);
                    }
                    // 2. 复制本控件在父控件的所在区域，作为自身的背景缓存，下次重绘时需要使用背景缓存清掉已有绘图。
                    if (_offscreenBackgroundImage == null)
                    {
                        _offscreenBackgroundImage = new Bitmap(this.Width, this.Height);

                        if (this.BackColor == Color.Transparent)
                        {
                            Graphics targetGraphics = Graphics.FromImage(_offscreenBackgroundImage);
                            Graphics sourceGraphics = g;// Parent.CreateGraphics();
                            IntPtr hdcBuffer = targetGraphics.GetHdc();
                            IntPtr hdcOrg = sourceGraphics.GetHdc();
                            Win32.BitBlt(
                                hdcBuffer,
                                0,
                                0,
                                this.Width,
                                this.Height,
                                hdcOrg,
                                actualBounds.X,
                                actualBounds.Y,
                                TernaryRasterOperations.SRCCOPY);

                            targetGraphics.ReleaseHdc(hdcBuffer);
                            sourceGraphics.ReleaseHdc(hdcOrg);
                            targetGraphics.Dispose();
                        }
                        else
                        {
                            Graphics targetGraphics = Graphics.FromImage(_offscreenBackgroundImage);
                            targetGraphics.Clear(this.BackColor);
                        }

                    }
                    #endregion
                }
                else
                {
                    if (this.BackColor == Color.Transparent)
                    {
                        g.Clear(Parent.BackColor);
                        if (Parent.BackgroundImage != null)
                            g.DrawImage(Parent.BackgroundImage, actualBounds, actualBounds.X, actualBounds.Y, this.Width, this.Height, GraphicsUnit.Pixel);
                    }
                    else
                    {
                        g.Clear(this.BackColor);
                    }
                }

                if (this.BackgroundImage != null)
                {
                    g.DrawImage(this.BackgroundImage, 0, 0);
                }

                #region 画图像
                Image currentImage = this.NormalImage;
                if (this.HoverImage != null && Hovering)
                    currentImage = this.HoverImage;
                if (this.PressImage != null && Pressing)
                    currentImage = this.PressImage;

                if (currentImage != null)
                {
                    switch (this.SizeMode)
                    {
                        case SizeModes.Stretch:
                            g.DrawImage(currentImage, actualBounds);
                            break;
                        case SizeModes.Center:
                            g.DrawImage(currentImage, (actualBounds.Width - currentImage.Width) / 2, (actualBounds.Height - currentImage.Height) / 2);
                            break;
                    }
                }
                #endregion

                if (ShowBorder)
                {
                    using (Pen borderPen = new Pen(this.BorderColor, 1))
                    {
                        g.DrawRectangle(borderPen, new Rectangle(actualBounds.X, actualBounds.Y, this.Width - 1, this.Height - 1));
                    }
                }

                g.ResetClip();
            }

            #region 画文字

            DrawTextOnParentGraphics(g);

            #endregion

        }

        internal virtual void DrawTextOnParentGraphics(Graphics g)
        {
            Rectangle actualBounds = this.ActualBounds;

            using (StringFormat textformat = new StringFormat())
            using (SolidBrush textBrush = new SolidBrush(this.ForeColor))
            {
                textformat.Alignment = StringAlignment.Center;
                textformat.LineAlignment = StringAlignment.Near;

                SizeF stringSize = TextRenderer.MeasureText("　", this.Font);
                Rectangle stringRect = new Rectangle();
                if (TextLocation == TextLocations.Bottom)
                {
                    stringRect.X = actualBounds.Left - this.Margin.Left;
                    stringRect.Y = actualBounds.Bottom + this.Margin.Bottom;
                    stringRect.Width = actualBounds.Width + this.Margin.Left + this.Margin.Right;
                    stringRect.Height = (int)stringSize.Height;
                }
                else if (TextLocation == TextLocations.Top)
                {
                    stringRect.X = actualBounds.Left - this.Margin.Left;
                    stringRect.Y = actualBounds.Top - this.Margin.Top - (int)stringSize.Height;
                    stringRect.Width = actualBounds.Width + this.Margin.Left + this.Margin.Right;
                    stringRect.Height = (int)stringSize.Height;
                }

                g.DrawString(
                    this.Text,
                    this.Font,
                    textBrush,
                    stringRect,
                    textformat
                );
            }
        }
        
        protected Delegate[] GetPaintEventInvocationList()
        {
            Delegate[] PaintEventInvocationList = null;

            Control ParentForm = this.Parent as Control;
            if (ParentForm == null)
                return PaintEventInvocationList;

            string PaintEventKeyName = "EventPaint";

            PropertyInfo FormEventProperty; // 窗体事件属性信息
            EventHandlerList FormEventPropertyInstance; // 窗体事件属性对象
            FieldInfo PaintEventKeyField;   // Paint事件对应的键值字段信息
            Object PaintEventKeyInstance; // Paint事件对应的键值对象
            Delegate PaintEventDelegate;

            Type FormType = typeof(Control);
            Type BaeTypeOfPaintEvent = typeof(Control);
            

            FormEventProperty = FormType.GetProperty("Events", BindingFlags.Instance | BindingFlags.NonPublic);
            FormEventPropertyInstance = FormEventProperty.GetValue(ParentForm, null) as EventHandlerList;

            PaintEventKeyField = BaeTypeOfPaintEvent.GetField(PaintEventKeyName, BindingFlags.Static | BindingFlags.NonPublic);

            PaintEventKeyInstance = PaintEventKeyField.GetValue(ParentForm);
            PaintEventDelegate = FormEventPropertyInstance[PaintEventKeyInstance];

            if (PaintEventDelegate != null)
                PaintEventInvocationList = PaintEventDelegate.GetInvocationList();

            return PaintEventInvocationList;
        }

        protected bool IsEventHandlerRegistered(string delegateName)
        {
            var eventlist = GetPaintEventInvocationList();
            if (eventlist != null)
            {
                foreach (Delegate existingHandler in eventlist)
                {
                    if (existingHandler.Method.Name == delegateName)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion

        #region 为父控件添加的事件处理
        
         void ImagePlaceholderParent_Paint(object sender, PaintEventArgs e)
        {
            Control parentControl = sender as Control;
            Graphics g = e.Graphics;

            foreach (Control ctrl in parentControl.Controls)
            {
                ImagePlaceholder placeholder = ctrl as ImagePlaceholder;
                if (placeholder != null)
                {
                    //if (Rectangle.Intersect(parentControl.DisplayRectangle, this.ActualBounds) == Rectangle.Empty)
                    //    continue;

                    if (!placeholder.DesignMode && placeholder.Hidden)
                        continue;

                    if (!UseOwnPaint)
                    {
                        placeholder.DrawOnParentGraphics(g);
                    }
                    else
                    {
                        placeholder.DrawTextOnParentGraphics(g);
                    }
                    //break;
                }
            }
        }

        void Parent_MouseUp(object sender, MouseEventArgs e)
        {
            Point p = e.Location;

            if (this.Pressing)
            {
                this.Pressing = false;
                ForceRefresh();

                if (this.Bounds.Contains(p))
                {
                    //if (!this.Enabled)
                    //    return;
                    
                    OnClick(EventArgs.Empty);
                    OnMouseClick(e);
                }
            }
        }

        void Parent_MouseDown(object sender, MouseEventArgs e)
        {
            Point p = e.Location;
            if (this.Bounds.Contains(p))
            {
                if (!Hidden && !this.Pressing)
                {
                    this.Pressing = true;
                    ForceRefresh();
                }
            }
        }

        void Parent_MouseMove(object sender, MouseEventArgs e) 
        {
            Point p = e.Location;  
            if (this.Bounds.Contains(p))
            {
                if (!this.Hovering)
                {
                    this.Hovering = true;
                    ForceRefresh();
                }
            }
            else if (this.Hovering)
            {
                this.Hovering = false;
                ForceRefresh();
            }
        }

        void Parent_MouseEnter(object sender, EventArgs e)
        {
        }

        #endregion




    }


}
