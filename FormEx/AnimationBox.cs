using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace System.Windows.Forms
{
    public class AnimationBox : NonFlickerUserControl
    {
        private System.Windows.Forms.Timer refreshTimer;
        private Image _gifImage;

        //public Image Image
        //{
        //    get => _gifImage;
        //    set
        //    {
        //        if (_gifImage != null)
        //        {
        //            ImageAnimator.StopAnimate(_gifImage, OnFrameChanged);
        //            _gifImage = null;
        //        }

        //        _gifImage = value;
        //        if (_gifImage != null)
        //        {
        //            ImageAnimator.Animate(_gifImage, OnFrameChanged);
        //            refreshTimer.Start();
        //        }
        //        else
        //        {
        //            refreshTimer.Stop();
        //        }
        //    }
        //}


        public Image Image
        {
            get => _gifImage;
            set
            {
                if (_gifImage != null)
                {
                    ImageAnimator.StopAnimate(_gifImage, OnFrameChanged);
                    _gifImage = null;
                }

                _gifImage = value;
                if (_gifImage != null)
                {
                    // 根据帧延迟动态调整定时器间隔
                    var frameDelay = GetFrameDelay(_gifImage);
                    refreshTimer.Interval = Math.Max(10, frameDelay);

                    ImageAnimator.Animate(_gifImage, OnFrameChanged);
                    refreshTimer.Start();
                }
            }
        }

        public AnimationBox()
        {
            refreshTimer = new System.Windows.Forms.Timer { Interval = 50 };
            refreshTimer.Tick += (s, e) => this.Invalidate();
        }


        private int GetFrameDelay(Image image)
        {
            const int PropertyTagFrameDelay = 0x5100;
            var propItem = image.GetPropertyItem(PropertyTagFrameDelay);
            return propItem != null ? BitConverter.ToInt32(propItem.Value, 0) * 10 : 100;
        }

        private void OnFrameChanged(object sender, EventArgs e)
        {
            BeginInvoke((MethodInvoker)Invalidate);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_gifImage != null)
            {
                ImageAnimator.UpdateFrames(_gifImage);
                e.Graphics.DrawImage(_gifImage, ClientRectangle);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                refreshTimer?.Dispose();
                if (_gifImage != null)
                    ImageAnimator.StopAnimate(_gifImage, OnFrameChanged);
            }
            base.Dispose(disposing);
        }
    }
}
