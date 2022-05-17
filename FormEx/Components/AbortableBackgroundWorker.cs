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

#if COMPILE_NET60
                e.Cancel = true;
#else
                e.Cancel = true;
                Thread.ResetAbort();
#endif
            }
        }


        public void Abort()
        {
            if (workerThread != null)
            {
                // 异常不会被workerThread捕捉
                //throw new TaskCanceledException();

#if COMPILE_NET60
                throw new TaskCanceledException();
#else
                workerThread.Abort();
                workerThread = null;
#endif
            }
        }

    }
}
