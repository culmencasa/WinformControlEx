using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace System.Windows.Forms
{
    public class CustomListBox : ListBox
    {
        public CustomListBox()
        {
            SelectionForeColor = Color.White;
            SelectionBackColor = Color.RoyalBlue;

            DrawMode = DrawMode.OwnerDrawVariable;
            this.ItemHeight = 32;

            //issue: 控件Dock之后会在底部留下一块空白, 原ListBox就有这个bug.
            //       猜测也有可能是设定了ItemHeight的高度造成的.

            this.DrawItem += CustomListBox_DrawItem;
        }

        private void CustomListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            var control = this;
            if (control.Items.Count > 0)
            {
                var txt = control.GetItemText(control.Items[e.Index]);
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    using (var brush = new SolidBrush(SelectionBackColor))
                    {
                        e.Graphics.FillRectangle(brush, e.Bounds);
                        TextRenderer.DrawText(e.Graphics, txt, control.Font, e.Bounds, SelectionForeColor,
                            TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
                    }
                }
                else
                {
                    using (var brush = new SolidBrush(this.BackColor))
                    {
                        e.Graphics.FillRectangle(brush, e.Bounds);
                        TextRenderer.DrawText(e.Graphics, txt, control.Font, e.Bounds, ForeColor,
                            TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
                    }
                }
            }

        }

        public Color SelectionForeColor { get; set; }

        public Color SelectionBackColor { get; set; }



    }
}
