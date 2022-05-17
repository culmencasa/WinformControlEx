using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace System.Windows.Forms.CustomListBox
{
    public class CustomListBox : ListBox
    {
        public CustomListBox()
        {
            SelectionForeColor = Color.White;
            SelectionBackColor = Color.RoyalBlue;

            DrawMode = DrawMode.OwnerDrawFixed;
            this.ItemHeight = 32;
            this.DrawItem += ListBoxEx_DrawItem;            
        }



        public Color SelectionForeColor { get; set; }

        public Color SelectionBackColor { get; set; }

        private void ListBoxEx_DrawItem(object sender, DrawItemEventArgs e)
        {
            var control = sender as ListBox;
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
    }
}
