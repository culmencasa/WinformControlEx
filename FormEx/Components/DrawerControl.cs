using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace System.Windows.Forms
{
    public class DrawerControl : CustomPanel
    {

        private Thread popupThread;
        private Thread popoffThread;
        private Form owner;

        public DrawerControl(Form form)
        {
            this.RoundBorderRadius = 50;
            this.

            owner = form;
            owner.Controls.Add(this);
            this.Width = owner.Width;
            this.Height = owner.Height - 100;

            owner.BringToFront();
            this.Location = new Point(this.Left, owner.Height);
        }



        public enum StartMenuStates
        {
            Closed = 0,
            Opening = 1,
            Opened = 2,
            Closing = 3
        }


        private StartMenuStates startMenuState = StartMenuStates.Closed;

        public new void Show()
        {
            if (startMenuState == StartMenuStates.Closed)
            {
                if (popupThread == null || popupThread.ThreadState != ThreadState.Running)
                {
                    popupThread = new Thread(PopUpThreadWork);
                    popupThread.Start();
                }
            }
            else if (startMenuState == StartMenuStates.Opened)
            {
                //if (popoffThread == null || popoffThread.ThreadState != ThreadState.Running)
                //{
                //    popoffThread = new Thread(PopOffThreadWork);
                //    popoffThread.Start();
                //}
            }

        }

        private void PopUpThreadWork()
        {
            if (startMenuState == StartMenuStates.Closing || startMenuState == StartMenuStates.Opening)
            {
                return;
            }

            Thread.Sleep(500);

            var pnlStart = this;
            var bottom = owner.Height;
            while (startMenuState != StartMenuStates.Opened)
            {
                startMenuState = StartMenuStates.Opening;

                this.Invoke((Action)delegate
                {
                    if (pnlStart.Visible == false && pnlStart.Location.Y <= bottom)
                    {
                        pnlStart.Visible = true;
                        pnlStart.BringToFront();
                        return;
                    }


                    if (pnlStart.Location.Y <= bottom - pnlStart.Height)
                    {
                        pnlStart.Location = new Point(pnlStart.Left, bottom - pnlStart.Height);

                        //ActiveControl = pnlStart;
                        pnlStart.BringToFront();

                        startMenuState = StartMenuStates.Opened;
                    }
                    else
                    {
                        pnlStart.Location = new Point(
                            pnlStart.Left,
                            pnlStart.Top - 10
                        );
                        pnlStart.BringToFront();
                        //ActiveControl = pnlStart;
                    }
                });

                Thread.Sleep(10);
            }

        }

    }
}
