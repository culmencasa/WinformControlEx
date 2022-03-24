using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

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
            catch (TaskCanceledException)
            {
                e.Cancel = true;
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
                // 异常不会被workerThread捕捉
                //throw new TaskCanceledException();

                workerThread.Abort();
                workerThread = null;
            }
        }

    }
}
