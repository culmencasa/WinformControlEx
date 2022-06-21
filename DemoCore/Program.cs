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
            // ��Ϊ.net5�Զ����ɵĴ���
            //Application.SetHighDpiMode(HighDpiMode.SystemAware);
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);

            /* [��¼��˵��]
             * ��Ȼ.netcore��.net5��ƽ̨, ������ȴֻ����Windowsϵͳ��ʹ��, 
             * �ܶ�framework�Ŀ�Ҳ������Ǩ�Ƶ�.net5��, �������Ҳû�˻�ʹ��.net5��ΪWinform�������. 
             * 
             * ���������Ŀ��������ѧϰ���ͨ������������һ�״������ɶ���ƽ̨DLL. 
             * Demo5���ٸ���.
             * 
             * 2022-06-14 update: �ڲ��Եõ�win7�¿���������.net6�Ľ����,��������.net6-winform.
             * 1. ��Ocean��Ŀȥ��, �ĳ�.net6���л����µ����. 
             * 2. �����ʾ����
             */

            ApplicationConfiguration.Initialize();

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(true);
            //Application.SetHighDpiMode(HighDpiMode.DpiUnawareGdiScaled); // SetHighDpiMode�ظ�������Ч.


            //Application.Run(new Form1());
            //Application.Run(new OcnForm());
            Application.Run(new MainForm());
        }
    }
}
