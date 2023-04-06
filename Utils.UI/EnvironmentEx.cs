using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Utils;

namespace System.Windows.Forms
{
    public static class EnvironmentEx
    {
        /// <summary>
        /// 获取当前Windows名称
        /// </summary>
        /// <returns></returns>
        public static WindowsNames GetCurrentOSName()
        {
            WindowsNames osname = WindowsNames.Unknown;
            OperatingSystem os = Environment.OSVersion;
            switch (os.Platform)
            {
                case PlatformID.Win32Windows:
                    switch (os.Version.Minor)
                    {
                        case 0:
                            osname = WindowsNames.Windows95;
                            break;
                        case 10:
                            if (os.Version.Revision.ToString() == "2222A ")
                                osname = WindowsNames.Windows98SE;
                            else
                                osname = WindowsNames.Windows98;
                            break;
                        case 90:
                            osname = WindowsNames.WindowsMe;
                            break;
                    }
                    break;
                case PlatformID.Win32NT:
                    switch (os.Version.Major) // 主要版本号
                    {
                        /* Minor规则: (不一定准确)
                         * Windows 11 build number 起始于 20000(也有可能是Windows Server 2022)
                         * Windows 10 build number 起始于 10000
                         * Windows 8.1 build numbers 起始于 9000
                         * Windows XP build numbers 起始于 3000
                         * 
                         * 注册表
                         * HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\CurrentBuildNumber
                         * 
                         * 参考: https://www.anoopcnair.com/windows-10-build-numbers-version-numbers/
                         */
                        case 3:
                            osname = WindowsNames.WindowsNT35;
                            break;
                        case 4:
                            osname = WindowsNames.WindowsNT40;
                            break;
                        case 5:
                            switch (os.Version.Minor) // 次要版本号
                            {
                                case 0:
                                    osname = WindowsNames.Windows2000;
                                    break;
                                case 1:
                                    osname = WindowsNames.WindowsXP;
                                    break;
                                case 2:
                                    osname = WindowsNames.Windows2003;
                                    break;
                            }
                            break;
                        case 6:
                            switch (os.Version.Minor) // 次要版本号
                            {
                                case 0:
                                    osname = WindowsNames.WindowsVista;
                                    break;
                                case 1:
                                    osname = WindowsNames.Windows7;
                                    break;
                                case 2:
                                    osname = WindowsNames.Windows8;
                                    break;
                                case 3:
                                    osname = WindowsNames.Windows81;
                                    break;
                            }
                            break;
                        case 10:
                            /* 需要在app.manifest打开注解, os.Version.Major才能识别为10 */
                            if (os.Version.Build > 20000)
                            {
                                osname = WindowsNames.Windows11;
                            }
                            else if (os.Version.Build > 10000)
                            {
                                osname = WindowsNames.Windows10;
                            }
                            break;
                    }
                    break;
            }

            return osname;
        }

        /// <summary>
        /// 获取本地IE版本
        /// </summary>
        /// <returns></returns>
        public static int GetLocalIEVersion()
        {
            int result = 0;
            using (RegistryKey versionKey = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Internet Explorer"))
            {
                if (versionKey == null)
                    return result;

                string version = string.Empty;
                //判是否存在 svcVersion 属性
                if (!string.IsNullOrEmpty(Conv.NS(versionKey.GetValue("svcVersion"))))
                {
                    version = Conv.NS(versionKey.GetValue("svcVersion"));
                }

                if (string.IsNullOrEmpty(version))
                {
                    //判断是否存在 Version 属性
                    if (!string.IsNullOrEmpty(Conv.NS(versionKey.GetValue("Version"))))
                    {
                        version = Conv.NS(versionKey.GetValue("Version"));
                    }
                }

                if (!string.IsNullOrEmpty(version))
                {
                    result = Convert.ToInt32(version.Substring(0, version.IndexOf('.')));
                }
            }
            return result;
        }


        /// <summary>
        /// 是否以管理员权限运行
        /// </summary>
        /// <returns></returns>
        public static bool IsRunningAsAdministrator()
        {
            System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);

            return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);

        }


        /// <summary>
        /// 查询是否存在有与当前程序相同的进程
        /// </summary>
        /// <returns></returns>
        public static Process GetSameProcessWithCurrent()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);

            foreach (Process process in processes)
            {
                if (process.Id != current.Id)
                {
                    if (current.MainModule.FileName == Assembly.GetCallingAssembly().Location.Replace("/", "\\"))
                    {
                        return process;
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// 清除进程
        /// </summary>
        /// <param name="processname">带.exe后缀的进程 </param>
        public static void KillProcess(string processname)
        {
            Process[] allProcess = Process.GetProcesses(); //获取所有进程
            foreach (Process p in allProcess)
            {
                if (p.ProcessName.ToLower() + ".exe" == processname.ToLower()) //是否是当前操作的进程
                {
                    for (int i = 0; i < p.Threads.Count - 1; i++)
                        p.Threads[i].Dispose(); //释放当前进程的线程
                    p.Kill(); //关闭进程

                    break;
                }
            }
        }


        /// <summary>
        /// 获取当前缩放大小(1-4)
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static float GetCurrentScaleFactor(Form form = null)
        {
            float factor = GetScalingFactor();
            // 再次检查
            if (factor == 1)
            {
                // workaround: 异常来自 HRESULT:0x8001010D (RPC_E_CANTCALLOUT_ININPUTSYNCCALL)
                Thread thread = new Thread(() =>
                {
                    factor = GetScalingFactorUseMC();
                });
                thread.Start();
                thread.Join();
            }
            // 再次检查
            if (factor == 1 && form != null)
            {
                factor = GetScaleFactorUseGDI(form);
            }

            return factor;
        }

        private static float GetScalingFactor()
        {
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                IntPtr desktop = g.GetHdc();
                int LogicalScreenHeight = Win32.GetDeviceCaps(desktop, (int)DeviceCap.VERTRES);
                int PhysicalScreenHeight = Win32.GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES);


                float ScreenScalingFactor = (float)PhysicalScreenHeight / (float)LogicalScreenHeight;


                return ScreenScalingFactor;
            }
        }

        private static float GetScalingFactorUseMC()
        {
            int PixelsPerXLogicalInch = 0;
            int PixelsPerYLogicalInch = 0;
            using (ManagementClass mc = new ManagementClass("Win32_DesktopMonitor"))
            {
                using (ManagementObjectCollection moc = mc.GetInstances())
                {
                    foreach (ManagementObject item in moc)
                    {
                        PixelsPerXLogicalInch = int.Parse((item.Properties["PixelsPerXLogicalInch"].Value.ToString()));
                        PixelsPerYLogicalInch = int.Parse((item.Properties["PixelsPerYLogicalInch"].Value.ToString()));
                        break;
                    }
                }
            }

            if (PixelsPerXLogicalInch <= 0)
            {
                return 1;
            }

            float factor = PixelsPerXLogicalInch * 1f / 96;

            return factor;
        }

        private static float GetScaleFactorUseGDI(Form form)
        {
            float factor = 1;
            if (form.IsHandleCreated)
            {
                using (Graphics currentGraphics = Graphics.FromHwnd(form.Handle))
                {
                    float dpixRatio = currentGraphics.DpiX / 96;
                    float dpiyRatio = currentGraphics.DpiY / 96;

                    factor = dpixRatio;
                }
            }
            else
            {
                using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
                {
                    float dpiX = graphics.DpiX;
                    float dpiY = graphics.DpiY;

                    factor = dpiY / 96f;
                }
            }

            return factor;
        }

        /// <summary>
        /// 是否启用ClearType
        /// </summary>
        /// <returns></returns>
        public static bool IsClearTypeEnabled()
        {
            uint SPI_GETFONTSMOOTHING = 0x004A;
            uint SPI_GETFONTSMOOTHINGCONTRAST = 0x200C;

            uint fontSmoothing = 0;
            uint fontSmoothingContrast = 0;
            Win32.SystemParametersInfo(SPI_GETFONTSMOOTHING, 0, ref fontSmoothing, 0);
            Win32.SystemParametersInfo(SPI_GETFONTSMOOTHINGCONTRAST, 0, ref fontSmoothingContrast, 0);
            return (fontSmoothing == 1 && fontSmoothingContrast >= 200);
        }


    }


    /// <summary>
    /// Windows系统名称
    /// </summary>
    public enum WindowsNames
    {
        Unknown,
        Windows95,
        Windows98,
        Windows98SE,
        WindowsMe,
        WindowsNT35,
        WindowsNT40,
        Windows2000,
        WindowsXP,
        Windows2003,
        WindowsVista,
        Windows7,
        Windows8,
        Windows81,
        Windows10,
        Windows11
    }
}