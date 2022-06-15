using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FormExCore
{
    public class OcnLabel : CustomLabel
    {
        public OcnLabel()
        {
            this.ClientSizeChanged += OcnLabel_ClientSizeChanged;
        }

        private void OcnLabel_ClientSizeChanged(object? sender, EventArgs e)
        {
            this.ClientSizeChanged -= OcnLabel_ClientSizeChanged;
            if (DesignMode && AutoSize)
            {
                AutoSize = false;
                TextAlign = ContentAlignment.MiddleLeft;
                Size = new Size(130, 25);
            }
        }
    }
}
