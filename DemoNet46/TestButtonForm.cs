using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoNet46
{
    public partial class TestButtonForm : NonFrameForm
    {
        public TestButtonForm()
        {
            InitializeComponent();
        }

        private void btnShowException_Click(object sender, EventArgs e)
        {
            Exception ex = new Exception("abc");
            ExceptionMessageBox box = new ExceptionMessageBox(ex);
            box.Show();
        }
    }
}
