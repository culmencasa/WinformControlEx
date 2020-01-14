using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.Design;

namespace System.Windows.Forms
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
    public class TabStripButton : ToolStripButton
    {
        private Font _selectedFont;

        public TabStripButton() : base() { InitButton(); }
        public TabStripButton(Image image) : base(image) { InitButton(); }
        public TabStripButton(string text) : base(text) { InitButton(); }
        public TabStripButton(string text, Image image) : base(text, image) { InitButton(); }
        public TabStripButton(string Text, Image Image, EventHandler Handler) : base(Text, Image, Handler) { InitButton(); }
        public TabStripButton(string Text, Image Image, EventHandler Handler, string name) : base(Text, Image, Handler, name) { InitButton(); }

        private void InitButton()
        {
            _selectedFont = this.Font;
        }

        public override Size GetPreferredSize(Size constrainingSize)
        {
            Size sz = base.GetPreferredSize(constrainingSize);
            if (this.Owner != null && this.Owner.Orientation == Orientation.Vertical)
            {
                sz.Width += 3;
                sz.Height += 10;
            }
            return sz;
        }


        private Color _hotTextColor = Control.DefaultForeColor;

        [Category("Appearance")]
        [Description("高亮的标签按钮的字体颜色")]
        public Color HotTextColor
        {
            get { return _hotTextColor; }
            set { _hotTextColor = value; }
        }

        private Color m_SelectedTextColor = Control.DefaultForeColor;

        [Category("Appearance")]
        [Description("选择的标签按钮的字体颜色")]
        public Color SelectedTextColor
        {
            get { return m_SelectedTextColor; }
            set { m_SelectedTextColor = value; }
        }

        [Category("Appearance")]
        [Description("选择的标签按钮的字体")]
        public Font SelectedFont
        {
            get { return (_selectedFont == null) ? this.Font : _selectedFont; }
            set { _selectedFont = value; }
        }

        [Browsable(false)]
        [DefaultValue(false)]
        public new bool Checked
        {
            get { return IsSelected; }
            set { }
        }

        [Browsable(false)]
        public bool IsSelected
        {
            get
            {
                TabStrip owner = Owner as TabStrip;
                if (owner != null)
                    return (this == owner.SelectedTab);
                return false;
            }
            set
            {
                if (value == false) return;
                TabStrip owner = Owner as TabStrip;
                if (owner == null) return;
                owner.SelectedTab = this;
            }
        }

        public bool ShowClose { get; set; }

        protected override void OnOwnerChanged(EventArgs e)
        {
            if (Owner != null && !(Owner is TabStrip))
                throw new Exception("Cannot add TabStripButton to " + Owner.GetType().Name);
            base.OnOwnerChanged(e);
        }

    }
}
