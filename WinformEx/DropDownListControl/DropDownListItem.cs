using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace System.Windows.Forms
{
    /// <summary>
    /// 下拉框选项
    /// </summary>
    public class DropDownListItem : TileIcon
    {
        public DropDownListItem()
        {

            this.BackColor = Color.White;
            this._activedBackColor = Color.FromArgb(233, 243, 252);

            this.ShowImage = false;
            this.ShowSplitter = false;
        }


        protected override void DrawImageBorder(Graphics g)
        {
            //base.DrawImageBorder(g);
        }
    }
}
