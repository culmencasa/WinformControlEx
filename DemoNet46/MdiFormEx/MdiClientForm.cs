using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoNet46.MdiFormEx
{


    /// <summary>
    /// A wrapper class for any form wanting to be an MDI child of an MDI Panel
    /// </summary>
    public class MdiClientForm : Form
    {
        /// <summary>
        /// My parent MDI container
        /// </summary>
        public MdiClientPanel MyMdiContainer { get; set; }

        /// <summary>
        /// Standard Constructor
        /// </summary>
        public MdiClientForm()
        {
            Activated += OnFormActivated;
            FormClosed += OnFormClosed;
        }

        /// <summary>
        /// Reports back to the container when we close
        /// </summary>
        void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            MyMdiContainer.ChildClosed(this);
        }

        /// <summary>
        /// Reports back to the parent container when we are activated
        /// </summary>
        private void OnFormActivated(object sender, EventArgs e)
        {
            MyMdiContainer.ChildActivated(this);
        }


    }
}
