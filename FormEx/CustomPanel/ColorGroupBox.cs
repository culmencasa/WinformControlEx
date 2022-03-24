using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;

namespace System.Windows.Forms.CustomPanel
{
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
    public class ColorGroupBox : Control
    {
        public ColorGroupBox()
        {
            SetStyle(ControlStyles.ContainerControl, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw, true);
        }
             
    }
}
