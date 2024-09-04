using FormExCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DemoNet46.Pages
{
    public partial class HomePage : UserControl
    {
        public HomePage()
        {
            InitializeComponent();
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
