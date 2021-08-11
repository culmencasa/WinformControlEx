using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms.VisualStyles;
using VisualStyles = System.Windows.Forms.VisualStyles;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Windows.Forms.Design;

namespace System.Windows.Forms
{
    /// <summary>
    /// TabStrip(带标签控件的工具条控件)
    /// </summary>
    public class TabStrip : ToolStrip
    {
        public event EventHandler<SelectedTabChangedEventArgs> SelectedTabChanged;

        #region 字段

        private TabStripRenderer _renderer = new TabStripRenderer();
        protected TabStripButton _currentTab;
        DesignerVerb insPage = null;

        #endregion

        #region 构造

        public TabStrip() : base() 
        { 
            InitControl();
        }

        public TabStrip(params TabStripButton[] buttons) : base(buttons)
        {
            InitControl();
        }

        #endregion

        #region 属性

        /// <summary>
        /// 无效的属性
        /// </summary>
        public new ToolStripRenderer Renderer
        {
            get { return _renderer; }
            set { base.Renderer = _renderer; }
        }

        /// <summary>
        /// 获取或设置TabStrip的布局风格
        /// </summary>
        public new ToolStripLayoutStyle LayoutStyle
        {
            get { return base.LayoutStyle; }
            set 
            {
                switch (value)
                {
                    case ToolStripLayoutStyle.StackWithOverflow:
                    case ToolStripLayoutStyle.HorizontalStackWithOverflow:
                    case ToolStripLayoutStyle.VerticalStackWithOverflow:
                        base.LayoutStyle = ToolStripLayoutStyle.StackWithOverflow;
                        break;
                    case ToolStripLayoutStyle.Table:
                        base.LayoutStyle = ToolStripLayoutStyle.Table;
                        break;
                    case ToolStripLayoutStyle.Flow:
                        base.LayoutStyle = ToolStripLayoutStyle.Flow;
                        break;
                    default:
                        base.LayoutStyle = ToolStripLayoutStyle.StackWithOverflow;
                        break;
                }
            }
        }

        [Obsolete("过时的属性。改用 RenderStyle")]
        [Browsable(false)]
        public new ToolStripRenderMode RenderMode
        {
            get { return base.RenderMode; }
            set { RenderStyle = value; }
        }

        /// <summary>
        /// 获取或设置TabStrip控件的呈现样式
        /// </summary>
        [Category("Appearance")]
        [Description("使用该属性设置TabStrip控件样式，而不是RenderMode.")]
        public ToolStripRenderMode RenderStyle
        {
            get { return _renderer.RenderMode; }
            set 
            {
                _renderer.RenderMode = value;
                this.Invalidate();
            }
        }
        
        /// <summary>
        /// 获取或设置是否使用系统VisualStyle样式
        /// </summary>
        [Category("Appearance")]
        [Description("获取或设置是否使用系统VisualStyle样式")]
        public bool UseVisualStyles
        {
            get { return _renderer.UseVS; }
            set 
            {
                _renderer.UseVS = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 获取或设置标签按钮是否翻转显示
        /// </summary>
        [Category("Appearance")]
        [Description("获取或设置标签按钮是否翻转显示)")]
        public bool FlipButtons
        {
            get { return _renderer.Mirrored; }
            set 
            { 
                _renderer.Mirrored = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 获取或设置选择的标签
        /// </summary>
        public TabStripButton SelectedTab
        {
            get { return _currentTab; }
            set
            {
                if (value == null)
                    return;
                if (_currentTab == value)
                    return;
                if (value.Owner != this)
                    throw new ArgumentException("Cannot select TabButtons that do not belong to this TabStrip");
                OnItemClicked(new ToolStripItemClickedEventArgs(value));
            }
        }

        #endregion

        #region 重写的成员

        public override ISite Site
        {
            get
            {
                ISite site = base.Site;
                if (site != null && site.DesignMode)
                {
                    IContainer comp = site.Container;
                    if (comp != null)
                    {
                        IDesignerHost host = comp as IDesignerHost;
                        if (host != null)
                        {
                            IDesigner designer = host.GetDesigner(site.Component);
                            if (designer != null && !designer.Verbs.Contains(insPage))
                                designer.Verbs.Add(insPage);
                        }
                    }
                }
                return site;
            }
            set
            {
                base.Site = value;
            }
        }

        protected override void OnItemAdded(ToolStripItemEventArgs e)
        {
            base.OnItemAdded(e);
            if (e.Item is TabStripButton)
                SelectedTab = (TabStripButton)e.Item;
        }

        protected override void OnItemClicked(ToolStripItemClickedEventArgs e)
        {
            TabStripButton clickedBtn = e.ClickedItem as TabStripButton;
            if (clickedBtn != null)
            {
                this.SuspendLayout();
                _currentTab = clickedBtn;
                this.ResumeLayout();
                OnTabSelected(clickedBtn);
            }
            base.OnItemClicked(e);
        }

        #endregion


        protected void InitControl()
        {
            base.RenderMode = ToolStripRenderMode.ManagerRenderMode;
            base.Renderer = _renderer;

            _renderer.RenderMode = this.RenderStyle;
            insPage = new DesignerVerb("Insert tab page", new EventHandler(OnInsertPageClicked));
        }

        protected void OnInsertPageClicked(object sender, EventArgs e)
        {
            ISite site = base.Site;
            if (site != null && site.DesignMode)
            {
                IContainer container = site.Container;
                if (container != null)
                {
                    TabStripButton btn = new TabStripButton();
                    container.Add(btn);
                    btn.Text = btn.Name;
                }
            }
        }

        protected void OnTabSelected(TabStripButton tab)
        {
            this.Invalidate();
            if (SelectedTabChanged != null)
                SelectedTabChanged(this, new SelectedTabChangedEventArgs(tab));
        }

    }


}
