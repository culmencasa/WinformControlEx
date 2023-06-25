using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Utils.UI
{

    internal class MessageLoopUtil
    {
        private const int WM_QUIT = 0x0012;

        [DllImport("user32.dll")]
        private static extern bool GetMessage(out Message lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport("user32.dll")]
        private static extern bool TranslateMessage(ref Message lpMsg);

        [DllImport("user32.dll")]
        private static extern IntPtr DispatchMessage(ref Message lpMsg);

        [DllImport("user32.dll")]
        private static extern void PostQuitMessage(int nExitCode);


        private static bool isMessageLoopRunning = false;

        private static IntPtr mainFormHandle;


        public static void Start(IntPtr contextHandle)
        {
            if (isMessageLoopRunning)
            {
                return;
            }

            isMessageLoopRunning = true;

            if (contextHandle != IntPtr.Zero)
            {
                mainFormHandle = contextHandle;
            }
            else
            {
                if (Application.OpenForms.Count > 0)
                {
                    mainFormHandle = Application.OpenForms[0].Handle;
                }
            } 

            MessageLoop();
        }

        public static void Stop()
        {
            if (!isMessageLoopRunning)
            {
                return;
            }

            isMessageLoopRunning = false;
        }


        private static void MessageLoop()
        {
            Message message;
            while (isMessageLoopRunning)
            {
                if (GetMessage(out message, IntPtr.Zero, 0, 0))
                {
                    TranslateMessage(ref message);
                    DispatchMessage(ref message);
                }
                else
                {
                    ErrorOnGetMessage();

                }

                if (ShouldExitMessageLoop(message))
                {
                    isMessageLoopRunning = false;
                }
            }
        }

        private static void ErrorOnGetMessage()
        {

        }

        private static bool ShouldExitMessageLoop(Message message)
        {
            return (message.Msg == WM_QUIT && message.HWnd == mainFormHandle);
        }

    }
}
