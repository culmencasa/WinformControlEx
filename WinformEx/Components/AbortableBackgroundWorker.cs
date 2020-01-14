using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;

namespace System.ComponentModel
{
    public class AbortableBackgroundWorker : BackgroundWorker
    {

        private Thread workerThread;

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            workerThread = Thread.CurrentThread;
            try
            {
                base.OnDoWork(e);
            }
            catch (ThreadAbortException)
            {
                e.Cancel = true; 
                Thread.ResetAbort();
            }
        }


        public void Abort()
        {
            if (workerThread != null)
            {
                workerThread.Abort();
                workerThread = null;
            }
        }
    }
}
