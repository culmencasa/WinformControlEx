using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.Windows.Forms
{
    [Browsable(false)]
    public partial class CustomNavGroup : UserControl
    {
        public CustomNavGroup()
        {
            InitializeComponent();

            ItemListPanel.HeightChanged += ItemListPanel_HeightChanged;

            // default scheme
            this.BackColor = Color.Transparent;
            HoverBackColor = Color.FromArgb(248, 249, 250);
            HoverForeColor = Color.Black;
            GroupTextColor = Color.Black;
            GroupItemColor = Color.Black;
        }


        [Category("Custom")]
        public int GroupIndent
        {
            get
            {
                return ItemListPanel.Padding.Left;
            }
            set
            {
                ItemListPanel.Padding = new Padding(value,
                    ItemListPanel.Padding.Top,
                    ItemListPanel.Padding.Right,
                    ItemListPanel.Padding.Bottom);

            }
        }

        [Category("Custom")]
        public string GroupText
        {
            get
            {
                return GroupTextLabel.Text;
            }
            set
            {
                GroupTextLabel.Text = value;
            }
        }

        [Category("Custom")]
        public Color HoverBackColor
        {
            get;
            set;
        }

        [Category("Custom")]
        public Color HoverForeColor
        {
            get;
            set;
        } 

        [Category("Custom")]
        public Color GroupTextColor
        {
            get
            {
                return GroupTextLabel.ForeColor;
            }
            set
            {
                GroupTextLabel.ForeColor = value;
            }
        }

        private Color _groupItemColor;

        [Category("Custom")]
        public Color GroupItemColor
        {
            get
            {
                return _groupItemColor;
            }
            set
            {
                _groupItemColor = value;
                foreach (var item in ItemListPanel.Items)
                {
                    item.UnifiedColor = value;
                }
                
            }
        } 


        private void ItemListPanel_HeightChanged(int groupItemsTotalHeight)
        {
            this.Height = GroupTextPanel.Height + groupItemsTotalHeight;
        }

        public int AddGroupItem(string headline, string subheadline=null, string caption=null, Image icon =null, EventHandler action = null)
        {
            var navigationItemControl = new CustomNavItem();
            navigationItemControl.HotTrackBgColor = HoverBackColor;
            navigationItemControl.HotTrackForeColor = HoverForeColor;
            //navigationItemControl.IndentSize = GroupIndent;
            navigationItemControl.UnifiedColor = GroupItemColor;

            navigationItemControl.Caption = caption;
            navigationItemControl.HeadLine = headline;
            navigationItemControl.Subheadline = subheadline;
            navigationItemControl.Icon = icon;
            navigationItemControl.SingleClick += action;
            
            ItemListPanel.Controls.Add(navigationItemControl);


            return ItemListPanel.Controls.IndexOf(navigationItemControl);
        }
    }



    public class NavigationContainer : FlowLayoutPanel
    {
        public NavigationContainer()
        {
            this.SuspendLayout();
            this.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.ClientSizeChanged += AfterClientSizeChanged;
            
            this.ResumeLayout(false);
        }

        public event Action<int> HeightChanged;

        public List<CustomNavItem> Items
        {
            get
            { 
                return this.Controls.OfType<CustomNavItem>().ToList();
            }
        }


        private void AfterClientSizeChanged(object sender, EventArgs e)
        {
            foreach (var control in Items)
            {
                control.Size = new Size(
                    Width - Margin.Left - Margin.Right - Padding.Left - Padding.Right, 
                    control.Height );
            }
            AutoHeight();
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            if (e.Control as CustomNavItem != null)
            {
                var navItem = (CustomNavItem)e.Control;
                navItem.UpdateComponentsHeight();
                navItem.Width = Width - Margin.Left - Margin.Right - Padding.Left - Padding.Right;

            }
            base.OnControlAdded(e);


        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);

            AutoHeight();
        }

        private void AutoHeight()
        {
            int height = 0;
            foreach (CustomNavItem item in this.Controls.OfType<CustomNavItem>())
            {
                height += item.Height;
                height += item.Margin.Top + Margin.Top;
                height += item.Margin.Bottom + Margin.Bottom;
            }

            this.Height  = height  + Padding.Top + Padding.Bottom + 6;

            if (HeightChanged != null)
            {
                HeightChanged(this.Height);
            }
        }

    }

}
