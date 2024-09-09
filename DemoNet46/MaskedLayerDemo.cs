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
    public partial class MaskedLayerDemo : Form
    {
        public MaskedLayerDemo()
        {
            InitializeComponent(); 
            Shown += MaskedLayerDemo_Shown;
            
        }

        private void MaskedLayerDemo_Shown(object sender, EventArgs e)
        {
            MaskedLayer mask = new MaskedLayer(this);
            mask.LayerColor = Color.Black;
            mask.Show();

            DrawerControl drawer = new DrawerControl(this);
            drawer.Show();
        }
         
    }
}
