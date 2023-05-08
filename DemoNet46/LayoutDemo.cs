using FormExCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace DemoNet46
{
    public partial class LayoutDemo : NonFrameForm
    {
        public LayoutDemo()
        {
            InitializeComponent();
        }

        private void btnHome_SingleClick(object sender, MouseEventArgs e)
        {

        }

        private void btnFavorite_SingleClick(object sender, MouseEventArgs e)
        {

        }

        private void btnHelp_SingleClick(object sender, MouseEventArgs e)
        {

        }

        private void btnSwitchSidebar_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                int maxWidth = pnlSidebar.MaximumSize.Width;
                int minWidth = pnlSidebar.MinimumSize.Width;
                Thread t1 = new Thread(() => {

                    if (Conv.NS(pnlSidebar.Tag) == "Expand")
                    {
                        while (pnlSidebar.Width < maxWidth)
                        {
                            if (pnlSidebar.Width + 20 >= maxWidth)
                            {
                                pnlSidebar.Invoke((Action)delegate
                                { 
                                    pnlSidebar.Width = maxWidth;
                                    pnlSidebar.Tag = "Collapse";
                                });

                                return;
                            }
                            else
                            {
                                pnlSidebar.Invoke((Action)delegate
                                {
                                    pnlSidebar.Width += 20;
                                    foreach (Control control in pnlSidebar.Controls)
                                    {
                                        control.Refresh();
                                    }
                                });
                            }
                        }
                    }
                    else
                    {

                        while (pnlSidebar.Width > minWidth)
                        {
                            if (pnlSidebar.Width - 20 <= minWidth)
                            {
                                pnlSidebar.Invoke((Action)delegate
                                {
                                    pnlSidebar.Width = minWidth;
                                    pnlSidebar.Tag = "Expand";
                                });
                                return;
                            }
                            else
                            {
                                pnlSidebar.Invoke((Action)delegate
                                {
                                    pnlSidebar.Width -= 20;
                                });
                            }
                        }
                    }

                });

                t1.Start();
            }
        }



        private void btnAnimate_Click(object sender, EventArgs e)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            int frameCount = 0;

            //btnAnimate.Enabled = false;

            var originalSize = new Size(150, 150);
            var loopControls = new List<OcnTile>();
            loopControls.AddRange(customPanel1.Controls.OfType<OcnTile>().ToArray());

            loopControls.ForEach((item) =>
            {
                item.Opacity = 0;
                item.Visible = false;
                item.Width = (int)(originalSize.Width * 0.5f);
                item.Height = (int)(originalSize.Height * 0.3f);
            });

            //loopControls.Reverse();


            int interval = 50;
            var timer = new System.Windows.Forms.Timer();
            timer.Tick += (a, b) =>
            {
                if (loopControls.Count > 0)
                {
                    for (var i = loopControls.Count - 1; i >= 0; i--)
                    {
                        var control = loopControls[i] as OcnTile;

                        // 仅在一定条件下才开始下一个控件的动画
                        if (i + 1 < (loopControls.Count - 1) && loopControls[i + 1].Opacity <= 0.3f)
                        {
                            continue;
                        }
                        else if (control.Opacity >= 1 && control.Width >= originalSize.Width && control.Height >= originalSize.Height)
                        {
                            control.Opacity = 1;
                            control.Width = originalSize.Width;
                            control.Height = originalSize.Height;

                            // 直到0, Tick结束
                            loopControls.Remove(control);
                            continue;
                        }

                        control.Visible = true;

                        var increment = (int)(0.2 * originalSize.Width);
                        if (control.Width + increment <= originalSize.Width)
                        {
                            control.Width += increment;
                        }
                        else
                        {
                            control.Width = originalSize.Width;
                        }
                        increment = (int)(0.2 * originalSize.Height);
                        if (control.Height + increment <= originalSize.Height)
                        {
                            control.Height += increment;
                        }
                        else
                        {
                            control.Height = originalSize.Height;
                        }

                        control.Opacity += 0.1f;
                        control.Refresh();

                        // 计算fps
                        frameCount++;
                        double elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
                        double fps = frameCount / elapsedSeconds;
                        //customLabel1.Text = $"FPS: {fps:N0}";
                    }
                }
                else
                {
                    timer.Stop();
                    timer.Dispose();
                    timer = null;
                    //btnAnimate.Enabled = true;
                    return;
                }

                timer.Interval = interval;
            };


            timer.Start();
        }

        private void btnColorize_Click(object sender, EventArgs e)
        {
            foreach (OcnTile item in customPanel1.Controls)
            {
                item.DrawingBackColor = item.MouseOverBackColor;
                item.Refresh();
            }
        }
    }
}
