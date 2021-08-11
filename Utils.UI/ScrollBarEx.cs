using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;

namespace System.Drawing
{
    /// <summary>
    /// 滚动条控制类
    /// </summary>
    public class ScrollBarEx : NativeWindow, IDisposable
    {

        public enum ScrollBarTypes
        {
            /// <summary>
            /// 水平滚动条
            /// </summary>
            HorizontalBar = 0,
            /// <summary>
            /// 垂直滚动条
            /// </summary>
            VerticalBar = 1,
        }

        #region 常量

        private const int WS_HSCROLL = 1;
        private const int WS_VSCROLL = 2;
        private const int ESB_DISABLE_BOTH = 3;
        private const int ESB_ENABLE_BOTH = 0;
        private const int GWL_STYLE = (-16);

        #endregion

        #region 字段

        // 相关联的控件
        private Control _owner;

        #endregion

        #region 构造函数

        /// <summary>
        /// 创建一个ScrollBarEx的新实例
        /// </summary>
        /// <param name="owner">要控制的滚动条控件</param>
        /// <param name="scrollBar">滚动条类型</param>
        public ScrollBarEx(Control owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner参数不能为空。");
            }
            base.AssignHandle(owner.Handle);
            _owner = owner;
        }

        #endregion

        #region 属性

        public bool HideHScroll
        {
            get;
            set;
        }

        public bool HideVScroll
        {
            get;
            set;
        }

        #endregion

        #region 重写的成员

        protected override void WndProc(ref Message m)
        {
            if (HideHScroll && HideVScroll)
            {
                HideAll(ref m);
            }
            else if (HideHScroll)
            {
                HideHScrollBar(ref m);
            }
            else if (HideVScroll)
            {
                HideVScrollBar(ref m);
            }
            base.WndProc(ref m);
        }

        #endregion

        #region 私有方法

        private void HideHScrollBar(ref Message m)
        {
            Win32.ShowScrollBar(_owner.Handle, WS_HSCROLL, false);
            //int dwStyle = Win32.GetWindowLong(base.Handle, GWL_STYLE);

            //switch (_scrollBar)
            //{
            //    case ScrollBarTypes.HorizontalBar:
            //        if ((dwStyle & WS_HSCROLL) == WS_HSCROLL)
            //        {
            //            Win32.ShowScrollBar(base.Handle, (int)_scrollBar, false);
            //        }
            //        break;
            //    case ScrollBarTypes.VerticalBar:
            //        if ((dwStyle & WS_VSCROLL) == WS_VSCROLL)
            //        {
            //            Win32.ShowScrollBar(base.Handle, (int)_scrollBar, false);
            //        }
            //        break;
            //}
        }

        private void HideVScrollBar(ref Message m)
        {
            Win32.ShowScrollBar(_owner.Handle, WS_VSCROLL, false);
        }

        private void HideAll(ref Message m)
        {
            Win32.ShowScrollBar(_owner.Handle, ESB_DISABLE_BOTH, false);
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 判断是否出现垂直滚动条
        /// </summary>
        /// <param name="ctrl">待测控件</param>
        /// <returns>出现垂直滚动条返回true，否则为false</returns>
        public static bool IsVerticalScrollBarVisible(Control ctrl)
        {
            if (!ctrl.IsHandleCreated)
                return false;

            return (Win32.GetWindowLong(ctrl.Handle, GWL_STYLE) & WS_VSCROLL) != 0;
        }

        /// <summary>
        /// 判断是否出现水平滚动条
        /// </summary>
        /// <param name="ctrl">待测控件</param>
        /// <returns>出现水平滚动条返回true，否则为false</returns>
        public static bool IsHorizontalScrollBarVisible(Control ctrl)
        {
            if (!ctrl.IsHandleCreated)
                return false;
            return (Win32.GetWindowLong(ctrl.Handle, GWL_STYLE) & WS_HSCROLL) != 0;
        }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            base.ReleaseHandle();
            _owner = null;
        }

        #endregion

    }
}
