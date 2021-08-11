using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Windows.Forms
{
    public class DragDropInfo
    {
        public Object DragSender { get; set; }

        public Object CarriedData { get; private set; }

        public DragDropInfo(Object sender, Object control)
        {
            this.DragSender = sender;
            this.CarriedData = control;
        }
    }
}
