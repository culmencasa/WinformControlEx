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
        
        /// <summary>
        /// 存储原始数据源对象
        /// </summary>
        public object DataItem { get; set; }

        protected override void DrawImageBorder(Graphics g)
        {
            //base.DrawImageBorder(g);
        }
    }
}
