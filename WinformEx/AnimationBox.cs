using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace System.Windows.Forms
{
    public class AnimationBox : NonFlickerUserControl
    {
        public Image Image { get; set; }

        protected override void OnHandleCreated(EventArgs e)
        {
            if (DesignMode)
                return;

            if (this.Image != null)
            {
                ImageAnimator.Animate(this.Image, new EventHandler(this.OnFrameChanged));
            }
            base.OnHandleCreated(e);
        }

        private void OnFrameChanged(object o, EventArgs e)
        {
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.Image != null)
            {
                ImageAnimator.UpdateFrames();
                e.Graphics.DrawImage(this.Image, this.ClientRectangle);
            }
        }
    }
}
