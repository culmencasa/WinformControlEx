using System;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Forms
{

    public class SelectedTabChangedEventArgs : EventArgs
    {
        public readonly TabStripButton SelectedTab;

        public SelectedTabChangedEventArgs(TabStripButton tab)
        {
            SelectedTab = tab;
        }

    }

}
