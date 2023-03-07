using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace System.Windows.Forms
{
    /// <summary>
    /// 下拉框选项
    /// </summary>
    [ToolboxItem(false)]
    public class DropDownListItem : TileIcon
    {
        public DropDownListItem()
        {

            this.BackColor = Color.White;
            this._selectedBackColor = Color.FromArgb(233, 243, 252);

            this.ShowImage = false;
            this.ShowSplitter = false;
        }


        protected override void DrawImageBorder(Graphics g)
        {
            //base.DrawImageBorder(g);
        }
    }
}
