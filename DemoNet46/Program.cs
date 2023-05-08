using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoNet46
{
    static class Program
    {


        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            // 开启高HDPI支持(Win8以上)
            Win32.SetProcessDPIAware();

            //var screenSize = Screen.PrimaryScreen.WorkingArea.Size;
            var screenSize = new Size(1080, 720);
            var splashForm = new SplashForm(screenSize.Width, screenSize.Height)
            {
                ShowInTaskbar = true,
                Text = "Demo"
            };
            splashForm.Show(Properties.Resources.Se7enBoot);

            var mainForm = FormManager.Single<MainForm>();
            mainForm.Size = screenSize;
            mainForm.SetWindowSizeByDPI();
            mainForm.Show();


            Application.Run(mainForm);
        }

         
    }
}
