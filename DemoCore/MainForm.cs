using FormExCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoCore
{
    public partial class MainForm : OcnForm
    {
        public MainForm()
        {
            InitializeComponent();

            // 无实现. 仅测试在不同.net框架编译条件下FormEx项目可用
        }

        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void btnClose_Click(object sender, EventArgs e)
        {

        }
    }
}
