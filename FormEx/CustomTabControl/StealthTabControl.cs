using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Windows.Forms
{
    /// <summary>
    /// 运行时隐藏TabBar的TabControl
    /// </summary>
    public class StealthTabControl : TabControl
    {
        public StealthTabControl()
        {
            HideTabBarStep1();
        }

        protected override void WndProc(ref Message m)
        {
            HideTabBarStep2(ref m);
        }

        #region  隐藏标签栏的两个步骤

        protected void HideTabBarStep1()
        {
            if (!this.DesignMode)
            {
                this.Multiline = true;
            }
        }

        protected void HideTabBarStep2(ref Message m)
        {
            if (m.Msg == 0x1328 && !this.DesignMode)
            {
                m.Result = new IntPtr(1);
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        #endregion


    }
}
