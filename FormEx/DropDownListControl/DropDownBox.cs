using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
    /// <summary>
    /// 下拉框控件
    /// </summary>
    public partial class DropDownBox : Control
    {
        #region 内嵌类

        /// <summary>
        /// 下拉按钮类
        /// </summary>
        public class DropDownButton
        {
            private Control _parent;

            public float BorderWidth { get; set; }

            public Color BorderColor { get; set; }

            public Color BackColor { get; set; }

            public Color ForeColor { get; set; }

            public Size ButtonSize { get; set; }

            public Point Location { get; set; }

            public Rectangle Bounds
            {
                get
                {
                    Rectangle rect = new Rectangle(
                        this.Location.X, this.Location.Y,
                        this.ButtonSize.Width, this.ButtonSize.Height);

                    return rect;
                }
            }

            public DropDownButton()
            {
                this.ButtonSize = new Size(20, 20);
                this.Location = new Point(0, 0);
                this.ForeColor = Color.FromArgb(122, 162, 187);
                this.BackColor = Color.FromArgb(237, 248, 254);
                this.BorderColor = Color.White;
                this.BorderWidth = 1;
            }

            public DropDownButton(Control parent) : this()
            {
                _parent = parent;
                _parent.Resize += new EventHandler(_parent_Resize);
                Resize();
            }

            private void Resize()
            {
                if (_parent != null)
                {
                    this.Location = new Point(_parent.Width - this.ButtonSize.Width - (int)BorderWidth * 2, (_parent.Height - this.ButtonSize.Height) / 2);
                }
            }

            void _parent_Resize(object sender, EventArgs e)
            {
                Resize();
            }
         }

        /// <summary>
        /// 数据源类
        /// </summary>
        public class ItemCollection : List<DropDownListItem>
        {
            // 存储父级DropDownBox引用
            private DropDownBox _parent;
            
            // 构造函数，接受父级引用
            public ItemCollection(DropDownBox parent = null)
            {
                _parent = parent;
            }
            
            // 在ItemCollection类中
            public void Add(string displayValue, object bindingValue = null, Image icon = null)
            {
                DropDownListItem item = new DropDownListItem();
                item.Text = displayValue;
                item.DefaultImage = icon;
                item.Tag = bindingValue;
                
                // 只有当父级引用存在时才设置字体
                if (_parent != null)
                {
                    item.Font = _parent.Font;
                }
                
                this.Add(item);
                OnListChanged(EventArgs.Empty);
            }

            public void Insert(int index, string displayValue, object bindingValue = null, Image icon = null)
            {
                DropDownListItem item = new DropDownListItem();
                item.Text = displayValue;
                item.DefaultImage = icon;
                item.Tag = bindingValue;
                
                // 只有当父级引用存在时才设置字体
                if (_parent != null)
                {
                    item.Font = _parent.Font;
                }
            
                this.Insert(index, item); // 注意：这里应该使用传入的index参数，而不是硬编码的0
                OnListChanged(EventArgs.Empty);
            }

            public event EventHandler ListChanged;

            protected virtual void OnListChanged(EventArgs e)
            {
                if (ListChanged != null)
                {
                    ListChanged(this, e);
                }
            }
        }

        #endregion
        
        #region 委托和事件

        public delegate void SelectionChangedEventHandler(object sender, DropDownListItem previousItem, DropDownListItem currentItem);

        public event SelectionChangedEventHandler SelectionChanged;

        #endregion

        #region 字段

        // 文本缩进
        private int textIndent;
        // 控件的文本
        private string displayText;
        // 下拉窗体
        private DropDownList frmDropDownList;
        // 下拉按钮
        private DropDownButton btnDropDown;
        // 下拉窗体的数据源
        private ItemCollection dataSource;

        private DropDownListItem selectedItem;

        #endregion

        #region 属性

        /// <summary>
        /// 项的高度
        /// </summary>
        public int ItemHeight { get; set; }

        /// <summary>
        /// 数据源
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ItemCollection DataSource
        {
            get
            {
                if (dataSource == null)
                    dataSource = new ItemCollection(this); // 传递this作为父级引用
        
                return dataSource;
            }
            set
            {
                bool hasChanged = (dataSource != value);
                dataSource = value;
                if (hasChanged)
                    RefreshData();
            }
        }

        /// <summary>
        /// 要显示的属性名称
        /// </summary>
        public string DisplayMember { get; set; }

        /// <summary>
        /// 要作为值的属性名称
        /// </summary>
        public string ValueMember { get; set; }

        /// <summary>
        /// 边框色
        /// </summary>
        [DefaultValue(typeof(Color), "180, 202, 215")]
        public Color BorderColor
        {
            get;
            set;
        }

        /// <summary>
        /// 前景色
        /// </summary>
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                this.frmDropDownList.ForeColor = value;
            }
        }

        /// <summary>
        /// 当前选中项的索引
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                if (SelectedItem == null)
                    return -1;

                return this.frmDropDownList.GetIndex(SelectedItem);
            }
            set
            {
                this.SelectedItem = this.frmDropDownList.GetItem(value);
            }
        }

        /// <summary>
        /// 当前选中项
        /// </summary>
        public DropDownListItem SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                SetSelectedValue(value);
            }
        }

        /// <summary>
        /// 当前选中的原始数据源对象
        /// </summary>
        public object SelectedObject
        {
            get
            {
                if (this.selectedItem == null)
                    return null;
                
                return this.selectedItem.DataItem;
            }
        }

        /// <summary>
        /// 当前选中项的值
        /// </summary>
        public object SelectedValue
        {
            get
            {
                if (this.selectedItem == null)
                    return null;

                return this.selectedItem.Tag;
            }
            set
            {
                if (this.dataSource != null)
                {
                    foreach (var item in this.dataSource)
                    {
                        if (item.Tag != null && item.Tag.Equals(value))
                        {
                            SetSelectedValue(item);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 当前选中项的文字
        /// </summary>
        public new string Text
        {
            get
            {
                return displayText;
            }
            set
            {
                base.Text = value;
                displayText = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 文字的缩进
        /// </summary>
        public int TextIndent
        {
            get { return textIndent; }
            set { textIndent = value; }
        }

        #endregion
        
        #region 构造

        public DropDownBox()
        {
            InitializeComponent();

            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();


            this.DataSource.ListChanged += new EventHandler(dataSource_DataSourceChanged);
            this.textIndent = 6;
            this.displayText = string.Empty;
            base.Text = string.Empty;
            this.BorderColor = Color.FromArgb(180, 202, 215);
            this.ItemHeight = 30;
            
            this.frmDropDownList = new DropDownList(this);
            this.frmDropDownList.ItemClicked += new System.Windows.Forms.DropDownList.ItemClickedHandler(frmDropDownList_ItemClicked); 

            this.btnDropDown = new DropDownButton(this);
            this.MouseClick += new MouseEventHandler(CustomComboBox_MouseClick);
            this.ParentChanged += new EventHandler(DropDownBox_ParentChanged);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var parms = base.CreateParams;
                parms.Style &= ~0x02000000;  // Turn off WS_CLIPCHILDREN
                return parms;
            }
        }
        #endregion
        
        #region 事件处理

        // 父控件变更时
        void DropDownBox_ParentChanged(object sender, EventArgs e)
        {
            // 为父窗体添加事件
            if (DesignMode)
                return;

            this.Parent.HandleCreated += (o, a) =>{
                Form parentForm = FormManager.GetTopForm(this.Parent);
                parentForm.LocationChanged -= new EventHandler(ParentForm_LocationChanged);
                parentForm.LocationChanged += new EventHandler(ParentForm_LocationChanged);
            };
            
        }

        void ParentForm_LocationChanged(object sender, EventArgs e)
        {
            Form parentForm = sender as Form;
            if (!parentForm.IsHandleCreated || parentForm.IsDisposed)
            {
                return;
            }

            Point p = parentForm.PointToScreen(this.Location);
            frmDropDownList.Location = new Point(p.X, p.Y + this.Height);
        }

        void dataSource_DataSourceChanged(object sender, EventArgs e)
        {
            if (this.DataSource != null)
            {
                this.frmDropDownList.Foreach((ctrl)=> {
                    
                    bool exists = false;
                    foreach (var item in this.DataSource)
                    {
                        if (item == ctrl)
                        {
                            exists = true;
                            break;
                        }
                    }

                    if (exists == false)
                    {
                        this.frmDropDownList.Remove(ctrl);
                    }
                
                });
                
                foreach (var item in this.DataSource)
                {
                    this.frmDropDownList.Add(item);
                }
            }
        }
        

        void frmDropDownList_ItemClicked(object sender, DropDownListItem e)
        {
            this.SelectedItem = e;
            this.frmDropDownList.Hide();
        }

        void CustomComboBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != Forms.MouseButtons.Left)
                return;

            //if (DropDownButtonHitTest(e.Location))
            {
                Form parentForm = FormManager.GetTopForm(this.Parent);


                Point screenLocation = Point.Empty;
                if (this.Parent as Form != null)
                {
                    screenLocation = parentForm.PointToScreen(this.Location);
                    frmDropDownList.Location = new Point(
                        screenLocation.X, 
                        screenLocation.Y + this.Height - 1);
                }
                else
                {
                    screenLocation = this.PointToScreen(this.Location);
                    frmDropDownList.Location = new Point(
                        screenLocation.X - this.Location.X,
                        screenLocation.Y + this.Height - this.Location.Y - 2);
                }
                //frmDropDownList.TopMost = true;
                frmDropDownList.Show();
                frmDropDownList.Activate();
            }
        }

        #endregion

        #region 公开方法

        public void Clear()
        {
            this.DataSource.Clear();
            this.frmDropDownList.Clear();
            this.SelectedIndex = -1;
        }

        /// <summary>
        /// 设置数据源为任意类型的列表
        /// </summary>
        /// <typeparam name="T">列表项类型</typeparam>
        /// <param name="list">数据列表</param>
        /// <param name="displayMember">要显示的属性名称</param>
        /// <param name="valueMember">要作为值的属性名称</param>
        public void SetDataSource<T>(List<T> list, string displayMember = null, string valueMember = null)
        {
            Clear();
            
            // 设置显示和值成员
            if (!string.IsNullOrEmpty(displayMember))
                this.DisplayMember = displayMember;
            if (!string.IsNullOrEmpty(valueMember))
                this.ValueMember = valueMember;
            
            if (list == null || list.Count == 0)
                return;
            
            foreach (T item in list)
            {
                // 创建DropDownListItem
                DropDownListItem dropDownItem = new DropDownListItem();
                
                // 设置与DropDownBox相同的字体 - 添加这一行
                dropDownItem.Font = this.Font;
                
                // 设置显示文本
                if (!string.IsNullOrEmpty(this.DisplayMember))
                {
                    // 使用反射获取属性值
                    var prop = typeof(T).GetProperty(this.DisplayMember);
                    if (prop != null)
                    {
                        var displayValue = prop.GetValue(item, null);
                        dropDownItem.Text = displayValue?.ToString() ?? string.Empty;
                    }
                }
                else
                {
                    // 如果没有指定DisplayMember，使用ToString()
                    dropDownItem.Text = item?.ToString() ?? string.Empty;
                }
                
                // 设置Tag值（仅存储值字段，不再存储整个对象）
                if (!string.IsNullOrEmpty(this.ValueMember))
                {
                    // 使用反射获取属性值
                    var prop = typeof(T).GetProperty(this.ValueMember);
                    if (prop != null)
                    {
                        dropDownItem.Tag = prop.GetValue(item, null);
                    }
                }
                else
                {
                    // 如果没有指定ValueMember，Tag可以设置为null或保持为空
                    dropDownItem.Tag = null;
                }
                
                // 使用新的DataItem属性存储原始数据源对象
                dropDownItem.DataItem = item;
                
                // 添加到数据源
                this.DataSource.Add(dropDownItem);
            }
            
            // 手动刷新下拉框内容，确保数据显示出来
            RefreshData();
        }
        
        #endregion
        
        // 数据源变更处理
        protected virtual void RefreshData()
        {
            this.frmDropDownList.Clear();
            if (this.DataSource != null)
            {
                foreach (var item in this.DataSource)
                {
                    this.frmDropDownList.Add(item);
                }
            }
        }

        private void SetSelectedValue(DropDownListItem value)
        {
            bool hasChanged = selectedItem != value;
            var previousItem = selectedItem;
            selectedItem = value;

            if (hasChanged)
            {
                if (selectedItem == null)
                {
                    this.Text = string.Empty;
                }
                else
                {
                    this.Text = selectedItem.Text;
                }

                if (this.SelectionChanged != null)
                {
                    SelectionChanged(this, previousItem, selectedItem);
                }
            }
        }

        #region 可重写的方法
        
        /// <summary>
        /// 画边框
        /// </summary>
        /// <param name="g"></param>
        protected virtual void DrawBorder(Graphics g)
        {
            using (Pen borderPen = new Pen(BorderColor))
            {
                g.DrawRectangle(borderPen, new Rectangle(0, 0, this.Width - 1, this.Height - 1));

                if (frmDropDownList.Visible == true)
                {
                    Pen p = new Pen(this.BackColor);
                    g.DrawLine(p, 0, this.Height, this.Width - 1, this.Height - 1);
                    p.Dispose();
                }
            }
        }
        /// <summary>
        /// 画下拉按钮
        /// </summary>
        /// <param name="g"></param>
        protected virtual void DrawDropDownButton(Graphics g)
        {
            using (Brush bgBrush = new SolidBrush(btnDropDown.BackColor))
            {
                g.FillRectangle(bgBrush, btnDropDown.Bounds);
            }

            using (GraphicsPath path = GraphicsExtension.Create7x4In7x7DownTriangleFlag(btnDropDown.Bounds))
            {
                using (Pen p = new Pen(btnDropDown.ForeColor))
                {
                    g.DrawPath(p, path);
                }
            }
        }
        /// <summary>
        /// 画文本
        /// </summary>
        /// <param name="g"></param>
        protected virtual void DrawText(Graphics g)
        {
            using (Brush foreBrush = new SolidBrush(this.ForeColor))
            {
                string s = this.Text == null ? string.Empty : this.Text;
                SizeF sf = TextRenderer.MeasureText(s, this.Font);
                
                // 启用文本抗锯齿和清晰类型渲染
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                
                // 精确计算文字位置，确保垂直居中
                float yPos = (this.Height - sf.Height) / 2;
                
                g.DrawString(s, this.Font, foreBrush, this.Padding.Left + textIndent, yPos);
            }
        }

        #endregion
        
        #region 重写的成员

        // 重写背景重绘
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }
        // 重写前景重绘
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
             
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            
            DrawBorder(g);
            DrawText(g);
            DrawDropDownButton(g);

            base.OnPaint(e);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 是否点击了下拉按钮
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private bool DropDownButtonHitTest(Point point)
        {
            return btnDropDown.Bounds.Contains(point);
        }

        #endregion
        

    }
}
