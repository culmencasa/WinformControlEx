using Ocean;
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

            //new OcnForm().ShowDialog();
            //new OcnMessageBox().ShowDialog();

            var splashForm = new SplashForm(Properties.Resources.Se7enBoot);

            Thread th = new Thread(() =>
            {
                Thread.Sleep(1000);
            });
            th.Start();
            th.Join();
            splashForm.Close();
            var mainForm = FormManager.Single<MainForm>();
            mainForm.Show();


            Application.Run(mainForm);
        }
    }
}
