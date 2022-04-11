using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Utils.UI
{
    public static partial class ControlExtension
    {
        #region ComboBox相关


        public static void DisableMouseWheel(this ComboBox comboBox)
        {
            comboBox.MouseWheel -= ComboBox_MouseWheel;
            comboBox.MouseWheel += ComboBox_MouseWheel;
        }

        private static void ComboBox_MouseWheel(object sender, MouseEventArgs e)
        {
            ComboBox control = (ComboBox)sender;
            if (!control.DroppedDown)
            {
                ((HandledMouseEventArgs)e).Handled = true;
            }
        }

        #endregion
    }
}
