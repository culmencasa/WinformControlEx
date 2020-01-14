using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

namespace System.Windows.Forms
{
    /// <summary>
    /// 下拉框窗体
    /// </summary>
    partial class DropDownList : Form
    {
        #region 委托和事件

        public delegate void ItemClickedHandler(object sender, DropDownListItem selectedItem);

        public event ItemClickedHandler ItemClicked;

        #endregion

        #region 字段
        
        private DropDownBox _linkedControl;


        #endregion

        #region 构造

        public DropDownList(DropDownBox linkedControl)
        {
            InitializeComponent();

            this.Opacity = 1;
            this.BackColor = Color.White;
            this.BorderColor = Color.FromArgb(182, 201, 216); 
            this.StartPosition = FormStartPosition.Manual;
            this.Deactivate += new EventHandler(DropDownList_Deactivate);
            this.Activated += new EventHandler(DropDownList_Activated);

            this.Padding = new Padding(1);

            _linkedControl = linkedControl;
            _linkedControl.Resize += new EventHandler(LinkedControl_Resize);
            this.Width = _linkedControl.Width;
            this.MaxHeight = 200;
        }

        #endregion

        #region 属性

        [DefaultValue(typeof(Int32), "200")]
        public int MaxHeight { get; set; }

        [DefaultValue(typeof(Color), "180, 202, 215")]
        public Color BorderColor
        {
            get;
            set;
        }

        public override System.Drawing.Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                this.ucItemPanel.ForeColor = value;
                
            }
        }

        #endregion

        #region 事件处理

        void DropDownList_Activated(object sender, EventArgs e)
        {
            this.Width = _linkedControl.Width;
            bool scrollVisible = ScrollBarEx.IsVerticalScrollBarVisible(this);//this.ucItemPanel.VerticalScroll.Maximum > this.ucItemPanel.Height;

            int height = 0;
            foreach (var item in this.ucItemPanel.Controls.OfType<DropDownListItem>())
            {
                if (!scrollVisible)
                {
                    item.Width = this.ucItemPanel.Width;// -1;
                }
                else
                {
                    item.Width = this.ucItemPanel.Width
                        - item.Margin.Left - item.Margin.Right
                        - item.Padding.Left - item.Padding.Right
                        - ucItemPanel.Padding.Left - ucItemPanel.Padding.Right
                        - 2; // border width
                }
                if (_linkedControl != null)
                    item.Height = _linkedControl.ItemHeight;

                height += item.Height;
            }

            if (!scrollVisible)
            {
                height = height + 2;
            }

            this.Height = height;
            //this.Shown += (a, b) =>
            //{
            //    this.Size = new Size(_linkedControl.Width, this.Height);
            //};

            //this.BeginInvoke((Action)delegate
            //{
            //    this.Size = new Size(_linkedControl.Width, this.Height);
            //});

        }

        void DropDownList_Deactivate(object sender, EventArgs e)
        {
            this.Hide();
        }

        // 相关联的父控件在拉伸大小时
        void LinkedControl_Resize(object sender, EventArgs e)
        {
            // 与父控件宽度保持一致
            this.Width = this._linkedControl.Width;
        }


        void DropDownListItem_Click(object sender, EventArgs e)
        {
            DropDownListItem currentItem = sender as DropDownListItem;
            if (ItemClicked != null)
            {
                ItemClicked(this, currentItem);
            }
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 增加一项
        /// </summary>
        /// <param name="item"></param>
        public void Add(DropDownListItem item)
        {
            if (!this.ucItemPanel.Controls.Contains(item))
            {
                this.ucItemPanel.Controls.Add(item);
                this.ucItemPanel.Controls.SetChildIndex(item, this.ucItemPanel.Controls.Count - 1);
                if (_linkedControl != null)
                {
                    item.ForeColor = _linkedControl.ForeColor;
                }
                item.Click += new EventHandler(DropDownListItem_Click);
            }
        }

        /// <summary>
        /// 删除一项
        /// </summary>
        /// <param name="item"></param>
        public void Remove(DropDownListItem item)
        {
            if (this.ucItemPanel.Controls.Contains(item))
            {
                this.ucItemPanel.Controls.Remove(item);
                item.Click -= new EventHandler(DropDownListItem_Click);
            }
        }

        public bool Contains(DropDownListItem item)
        {
            return this.ucItemPanel.Controls.Contains(item);
        }

        public void Foreach(Action<DropDownListItem> check)
        {
            if (check != null)
            {
                var array = this.ucItemPanel.Controls.OfType<DropDownListItem>().ToArray();
                for (int i = array.Length - 1; i >= 0; i--)
                {
                    check(array[i]);
                }

            }
        }

        /// <summary>
        /// 清除所有项
        /// </summary>
        public void Clear()
        {
            this.ucItemPanel.CleanUp(false);
        }

        public int GetIndex(DropDownListItem item)
        {
            if (this.ucItemPanel.Controls.Contains(item))
            {
                return this.ucItemPanel.Controls.GetChildIndex(item);
            }
            return -1;
        }

        public DropDownListItem GetItem(int index)
        {
            foreach (var item in this.ucItemPanel.Controls.OfType<DropDownListItem>())
            {
                if (this.GetIndex(item) == index)
                {
                    return item;
                }
            }

            return null;
        }


        #endregion

        #region 可重写的方法

        protected virtual void DrawBorder(Graphics g)
        {
            using (Pen borderPen = new Pen(BorderColor))
            {
                g.DrawRectangle(borderPen, new Rectangle(0, 0, this.Width - 1, this.Height - 1));
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            base.OnPaint(e);

            DrawBorder(g);

        }

        #endregion

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            //ScrollBarEx sb = new ScrollBarEx(this.ucItemPanel);
            //sb.HideVScroll = true;
        }
    }
}
