using FormExCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoCore
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 此为.net5自动生成的代码
            //Application.SetHighDpiMode(HighDpiMode.SystemAware);
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);

            /* [记录及说明]
             * 虽然.netcore及.net5跨平台, 但窗体却只能在Windows系统下使用, 
             * 很多framework的库也将不会迁移到.net5上, 因此我想也没人会使用.net5作为Winform开发框架. 
             * 
             * 所以这个项目仅仅用于学习如何通过配置来做到一套代码生成多种平台DLL. 
             * Demo5不再更新.
             * 
             * 2022-06-14 update: 在测试得到win7下可正常运行.net6的结果后,决定尝试.net6-winform.
             * 1. 把Ocean项目去掉, 改成.net6运行环境下的类库. 
             * 2. 添加演示代码
             */

            ApplicationConfiguration.Initialize();

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(true);
            //Application.SetHighDpiMode(HighDpiMode.DpiUnawareGdiScaled); // SetHighDpiMode重复设置无效.


            //Application.Run(new Form1());
            //Application.Run(new OcnForm());
            Application.Run(new MainForm());
        }
    }
}
