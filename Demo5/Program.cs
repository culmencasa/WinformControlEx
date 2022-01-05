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

            /* [��¼��˵��]
             * ��Ȼ.netcore��.net5��ƽ̨, ������ȴֻ����Windowsϵͳ��ʹ��, 
             * �ܶ�framework�Ŀ�Ҳ������Ǩ�Ƶ�.net5��, �������Ҳû�˻�ʹ��.net5��ΪWinform�������. 
             * 
             * ���������Ŀ��������ѧϰ���ͨ������������һ�״������ɶ���ƽ̨DLL. 
             * Demo5���ٸ���.
             */
            Application.Run(new Form1());
        }
    }
}
