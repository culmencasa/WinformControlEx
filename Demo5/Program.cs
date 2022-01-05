using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            /* [记录及说明]
             * 虽然.netcore及.net5跨平台, 但窗体却只能在Windows系统下使用, 
             * 很多framework的库也将不会迁移到.net5上, 因此我想也没人会使用.net5作为Winform开发框架. 
             * 
             * 所以这个项目仅仅用于学习如何通过配置来做到一套代码生成多种平台DLL. 
             * Demo5不再更新.
             */
            Application.Run(new Form1());
        }
    }
}
