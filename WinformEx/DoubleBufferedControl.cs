using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace System.Windows.Forms
{
    public class DoubleBufferedControl : Control
    {
        const BufferedGraphics NO_MANAGED_BACK_BUFFER = null;

        protected BufferedGraphicsContext _graphicManager;
        protected BufferedGraphics _managedBackBuffer;

        public DoubleBufferedControl()
        {
            Application.ApplicationExit +=
                   new EventHandler(MemoryCleanup);

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            _graphicManager = BufferedGraphicsManager.Current;
            _graphicManager.MaximumBuffer =
                   new Size(this.Width + 1, this.Height + 1);
            _managedBackBuffer =
                _graphicManager.Allocate(this.CreateGraphics(), this.ClientRectangle);
        }

        private void MemoryCleanup(object sender, EventArgs e)
        {
            // clean up the memory
            if (_managedBackBuffer != NO_MANAGED_BACK_BUFFER)
                _managedBackBuffer.Dispose();
        }
        protected override void OnPaint(PaintEventArgs pe)
        {
            // we draw the progressbar into the image in the memory
            //DrawProgressBar(ManagedBackBuffer.Graphics);

            // now we draw the image into the screen
            _managedBackBuffer.Render(pe.Graphics);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            MemoryCleanup(this, e);

            _graphicManager.MaximumBuffer =
                  new Size(this.Width + 1, this.Height + 1);

            _managedBackBuffer =
                _graphicManager.Allocate(this.CreateGraphics(),
                                                ClientRectangle);

            this.Refresh();
        }

    }
}
