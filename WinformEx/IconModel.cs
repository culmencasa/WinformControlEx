using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace System.Windows.Forms
{
    public abstract class IconModel
    {
        #region 属性

        /// <summary>
        ///  图标的宽度和高度.
        /// </summary>
        public Size CustomSize
        {
            get;
            set;
        }

        public Padding Margin
        {
            get;
            set;
        }

        /// <summary>
        ///  获取或设置边框相对控件的边界
        /// </summary>
        public Padding Padding
        {
            get;
            set;
        }

        /// <summary>
        ///  图标的字体设置
        /// </summary>
        public Font IconFont
        {
            get;
            set;
        }

        #endregion
    }
}
