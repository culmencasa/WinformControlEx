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
    public partial class Form1 : NonFrameForm
    {
        public Form1()
        {
            InitializeComponent();

            this.CaptionShadowWidth = 5;
            this.Resizable = true;
            this.ShowCaptionShadow = true;

            pnlStart.Location = new Point(pnlStart.Left, pnlTaskbar.Top);
            pnlStart.LostFocus += PnlStart_LostFocus;
            

            popupTimer = new Timer();
            popupTimer.Interval = 1;
            popupTimer.Tick += new EventHandler(PopupTimer_Tick);

            popoffTimer = new Timer();
            popoffTimer.Interval = 1;
			popoffTimer.Tick += PopoffTimer_Tick;
        }

        private Timer popupTimer;
        private Timer popoffTimer;
        public enum StartMenuStates
        {
            Closed = 0,
            Opening = 1,
            Opened = 2,
            Closing = 3
        }
        private StartMenuStates startMenuState;

        private void PopoffTimer_Tick(object sender, EventArgs e)
        {
            if (startMenuState == StartMenuStates.Closed || startMenuState == StartMenuStates.Opening)
            {
                popoffTimer.Stop();
				pnlStart.GotFocus += PnlStart_GotFocus;
                pnlStart.LostFocus += PnlStart_LostFocus;
                return;
            }            


            startMenuState = StartMenuStates.Closing;
            if (pnlStart.Visible == true && pnlStart.Location.Y < pnlTaskbar.Top)
            {
                pnlStart.SendToBack();
            }
            if (pnlStart.Location.Y > pnlTaskbar.Top)
            {
                popoffTimer.Stop();
                pnlStart.Visible = false;
                pnlStart.Location = new Point(pnlStart.Left, pnlTaskbar.Top);
                startMenuState = StartMenuStates.Closed;
                pnlStart.LostFocus += PnlStart_LostFocus;
            }
            else
            {
                pnlStart.Location = new Point(
                    pnlStart.Left,
                    pnlStart.Top + 30
                );
            }

        }

		private void PnlStart_GotFocus(object sender, EventArgs e)
		{
            ActiveControl = pnlStart;
		}

		private void PnlStart_LostFocus(object sender, EventArgs e)
        {
            /* 麻烦...
			if (startMenuState != StartMenuStates.Opened)
			{
                return;
			}
            startMenuState = StartMenuStates.Closing;
            popoffTimer.Start();
            */
        }

		private void PopupTimer_Tick(object sender, EventArgs e)
        {
            if (startMenuState == StartMenuStates.Opened)
            {
                popupTimer.Stop();
                pnlStart.LostFocus += PnlStart_LostFocus;
                return;
            }

            while (startMenuState == StartMenuStates.Closing)
            {
                return;
            }

            startMenuState = StartMenuStates.Opening;

            if (pnlStart.Visible == false && pnlStart.Location.Y < pnlTaskbar.Top)
            {
                pnlStart.Visible = true;
            }
           
            if (pnlStart.Location.Y <= pnlTaskbar.Top - pnlStart.Height)
            {
                popupTimer.Stop();
                pnlStart.Location = new Point(pnlStart.Left, pnlTaskbar.Top - pnlStart.Height);

                ActiveControl = pnlStart;

                startMenuState = StartMenuStates.Opened;
                pnlStart.BringToFront();
                pnlStart.LostFocus += PnlStart_LostFocus;
            }
            else
            { 
                pnlStart.Location = new Point(
                    pnlStart.Left,
                    pnlStart.Top - 30
                );
            }

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (startMenuState == StartMenuStates.Closing || startMenuState == StartMenuStates.Closed)
            {
                pnlStart.LostFocus -= PnlStart_LostFocus;
                popupTimer.Start();                
            }
            else
            {
                pnlStart.LostFocus -= PnlStart_LostFocus;
                popoffTimer.Start();
            }
        }



        private void button1_Click(object sender, EventArgs e)
		{
            Form2 f = new Form2();
            f.Show();
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
            lblTime.Text =
                DateTime.Now.ToString("HH:mm") +
                Environment.NewLine +
                DateTime.Now.ToString("yyyy/MM/dd");
		}
	}


}
