using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
    public class StackPanel : Panel
    {
        #region 枚举

        public enum HAligns
        {
            Left,
            Center,
            Right
        }

        public enum VAligns
        {
            Top,
            Center,
            Bottom
        }

        #endregion

        #region 字段

        private int _spacing = 10;
        private bool _wrapContent = false;
        private bool _verticalCenterContent = true;
        private Orientation _orientation = Orientation.Vertical;
        private HAligns _horizontalAlignment = HAligns.Left;
        private VAligns _verticalAlignment = VAligns.Top;

        #endregion

        #region 属性

        [Category(Consts.DefaultCategory)]
        public Orientation Orientation
        {
            get { return _orientation; }
            set
            {
                _orientation = value;
                UpdateLayout();
            }
        }

        [Category(Consts.DefaultCategory)]
        public int Spacing
        {
            get { return _spacing; }
            set
            {
                _spacing = value;
                UpdateLayout();
            }
        }

        [Category(Consts.DefaultCategory)]
        public bool WrapContent
        {
            get
            {
                return _wrapContent;
            }
            set
            {
                _wrapContent = value;

                UpdateLayout();
            }
        }

        [Category(Consts.DefaultCategory)]
        public HAligns HorizontalAlignment
        {
            get { return _horizontalAlignment; }
            set
            {
                _horizontalAlignment = value;
                UpdateLayout();
            }
        }

        [Category(Consts.DefaultCategory)]
        public VAligns VerticalAlignment
        {
            get { return _verticalAlignment; }
            set
            {
                _verticalAlignment = value;
                UpdateLayout();
            }
        }


        #endregion

        #region 构造

        public StackPanel()
        {
            this.AutoScroll = true;
            this.SizeChanged += StackPanel_SizeChanged; ;
        }

        #endregion

        #region 重写的成员

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);

            UpdateLayout();
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);
            UpdateLayout();
        }

        /// <summary>
        /// 重写Controls属性的实例, 使控件在添加到StackPanel时自动布局
        /// </summary>
        /// <returns></returns>
        protected override ControlCollection CreateControlsInstance()
        {
            return new StackPanelControlCollection(this);
        }

        #endregion

        #region 事件处理

        private void StackPanel_SizeChanged(object sender, EventArgs e)
        {
            // 如果内容换行的话，StackPanel拉伸时也更新布局
            if (WrapContent)
            {
                UpdateLayout();
            }
        }

        public void Control_LocationChanged(object sender, EventArgs e)
        {
            UpdateLayout();
        }

        public void Control_DockChanged(object sender, EventArgs e)
        {
            // 禁用dock
            (sender as Control).Dock = DockStyle.None;
            UpdateLayout();
        }
        public void Control_SizeChanged(object sender, EventArgs e)
        {
            UpdateLayout();
        }


        #endregion

        #region 方法

        protected virtual void UpdateLayout()
        {
            this.SuspendLayout();

            var list = this.Controls as StackPanelControlCollection;

            if (WrapContent)
            {
                #region WrapContent为True时，只以Orientation的方向进行布局，子控件的起始位置为左上

                #endregion
                if (Orientation == Orientation.Vertical)
                {
                    int currentPosition = Padding.Top;
                    int currentColumnLeft = Padding.Left;
                    int maxColumnWidth = 0;

                    foreach (Control child in list.Items)
                    {
                        child.LocationChanged -= Control_LocationChanged;

                        if (currentPosition + child.Height > this.Height)
                        {
                            currentPosition = Padding.Top;
                            currentColumnLeft += maxColumnWidth + Spacing;
                            maxColumnWidth = 0;
                        }


                        int x = currentColumnLeft;
                        if (HorizontalAlignment == HAligns.Center)
                        {
                            x = (this.Width - child.Width) / 2;
                        }
                        else if (HorizontalAlignment == HAligns.Right)
                        {
                            x = this.Width - child.Width - Padding.Right;
                        }



                        var newLocation = new Point(x, currentPosition);
                        child.Location = newLocation;
                        currentPosition += child.Height + Spacing;
                        maxColumnWidth = Math.Max(maxColumnWidth, child.Width);
                        child.LocationChanged += Control_LocationChanged;
                    }

                }
                else if (Orientation == Orientation.Horizontal)
                {

                    int currentPosition = Padding.Left;
                    int currentRowTop = Padding.Top;
                    int maxRowHeight = 0;

                    foreach (Control child in list.Items)
                    {
                        child.LocationChanged -= Control_LocationChanged;

                        // 如果启用换行
                        if (currentPosition + child.Width > this.Width)
                        {
                            currentPosition = Padding.Left;
                            currentRowTop += maxRowHeight + Spacing;
                            maxRowHeight = 0;
                        }


                        int y = currentRowTop;
                        if (VerticalAlignment == VAligns.Center)
                        {
                            y = (this.Height - child.Height) / 2;
                        }
                        else if (VerticalAlignment == VAligns.Bottom)
                        {
                            y = this.Height - child.Height - Padding.Bottom;
                        }

                        var newLocation = new Point(currentPosition, y);
                        child.Location = newLocation;
                        currentPosition += child.Width + Spacing;
                        maxRowHeight = Math.Max(maxRowHeight, child.Height);
                        child.LocationChanged += Control_LocationChanged;
                    }

                }
            }
            else
            {
                #region WrapContent为False时，子控件的起始位置以HorizontalAlignment和VerticalAlignment的方向进行布局

                if (Orientation == Orientation.Vertical)
                {
                    int totalHeight = list.Items.Sum(child => child.Height + Spacing) - Spacing;
                    int currentPosition = Padding.Top;
                    int currentColumnLeft = Padding.Left;
                    int maxColumnWidth = 0;

                    foreach (Control child in list.Items)
                    {
                        child.LocationChanged -= Control_LocationChanged;

                        int x = currentColumnLeft;
                        if (HorizontalAlignment == HAligns.Center)
                        {
                            x = (this.Width - child.Width) / 2;
                        }
                        else if (HorizontalAlignment == HAligns.Right)
                        {
                            x = this.Width - child.Width - Padding.Right;
                        }

                        int y = currentPosition;
                        if (VerticalAlignment == VAligns.Center)
                        {
                            y = (this.Height - totalHeight) / 2 + currentPosition;
                        }
                        else if (VerticalAlignment == VAligns.Bottom)
                        {
                            y = this.Height - totalHeight + currentPosition - Padding.Bottom;
                        }

                        var newLocation = new Point(x, y);
                        child.Location = newLocation;
                        currentPosition += child.Height + Spacing;
                        maxColumnWidth = Math.Max(maxColumnWidth, child.Width);
                        child.LocationChanged += Control_LocationChanged;
                    }
                }
                else if (Orientation == Orientation.Horizontal)
                {
                    int totalWidth = list.Items.Sum(child => child.Width + Spacing) - Spacing;
                    int currentPosition = Padding.Left;
                    int currentRowTop = Padding.Top;
                    int maxRowHeight = 0;

                    foreach (Control child in list.Items)
                    {
                        child.LocationChanged -= Control_LocationChanged;

                        int x = currentPosition;
                        if (HorizontalAlignment == HAligns.Center)
                        {
                            x = (this.Width - totalWidth) / 2 + currentPosition;
                        }
                        else if (HorizontalAlignment == HAligns.Right)
                        {
                            x = this.Width - totalWidth + currentPosition - Padding.Right;
                        }

                        int y = currentRowTop;
                        if (VerticalAlignment == VAligns.Center)
                        {
                            y = (this.Height - child.Height) / 2;
                        }
                        else if (VerticalAlignment == VAligns.Bottom)
                        {
                            y = this.Height - child.Height - Padding.Bottom;
                        }

                        var newLocation = new Point(x, y);
                        child.Location = newLocation;
                        currentPosition += child.Width + Spacing;
                        maxRowHeight = Math.Max(maxRowHeight, child.Height);
                        child.LocationChanged += Control_LocationChanged;
                    }
                }

                #endregion

            }

            this.ResumeLayout(false); 
        }


        #endregion

        #region 自定义StackPanel.Controls集合类

        private class StackPanelControlCollection : Control.ControlCollection
        {
            #region 构造

            public StackPanelControlCollection(StackPanel owner) : base(owner)
            {
                Owner = owner;
                Items = new List<Control>();
            }

            #endregion

            #region 属性

            private new StackPanel Owner
            {
                get;
                set;
            }

            /// <summary>
            /// Controls集合的顺序很迷,改为自己的集合
            /// </summary>
            public List<Control> Items
            {
                get;
                set;
            }

            #endregion

            #region 重写的方法

            public override void Add(Control newControl)
            {
                newControl.LocationChanged -= Owner.Control_LocationChanged;
                newControl.LocationChanged += Owner.Control_LocationChanged;

                newControl.DockChanged -= Owner.Control_DockChanged;
                newControl.DockChanged += Owner.Control_DockChanged;

                newControl.SizeChanged -= Owner.Control_SizeChanged;
                newControl.SizeChanged += Owner.Control_SizeChanged;

                Items.Add(newControl);
                base.Add(newControl);
            }
              
            public override void Remove(Control value)
            {
                value.LocationChanged -= Owner.Control_LocationChanged;
                value.DockChanged -= Owner.Control_DockChanged;
                value.SizeChanged -= Owner.Control_SizeChanged;

                Items.Remove(value);
                base.Remove(value);
            }

            public override System.Collections.IEnumerator GetEnumerator()
            {
                return Items.GetEnumerator();
            }


            #endregion
        }


        #endregion
    }


}
