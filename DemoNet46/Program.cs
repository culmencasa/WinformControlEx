using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoNet46
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            var splashForm = new SplashForm(Properties.Resources.Se7enBoot);

            var mainForm = new MainForm();
            mainForm.Show();

            Thread th = new Thread(() =>
            {
                Thread.Sleep(1000);
            });
            th.Start();
            th.Join();
            splashForm.Close();

            Application.Run(mainForm);
        }
    }
}
