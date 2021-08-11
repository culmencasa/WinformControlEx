using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Utils.UI
{

    /// <summary>
    /// 网络状态检查
    /// </summary>
    public class NetworkChecker
    {
        [Flags]
        enum InternetConnectionState : int
        {
            INTERNET_CONNECTION_MODEM = 0x01,
            INTERNET_CONNECTION_LAN = 0x02,
            INTERNET_CONNECTION_PROXY = 0x04,
            INTERNET_CONNECTION_MODEM_BUSY = 0x08, // 已弃用
            INTERNET_RAS_INSTALLED = 0x10,
            INTERNET_CONNECTION_OFFLINE = 0x20,
            INTERNET_CONNECTION_CONFIGURED = 0x40
        }



        /// <summary>
        /// 判断是否存在网络连接（Modem或LAN)
        /// </summary>
        /// <returns>返回False表示无网络连接。
        /// 返回True，表示存在网络连接，但并不保证能与某特定主机建立连接。</returns>
        public static bool ConnectionAvailable()
        {
            bool isConnected = false;
            try
            {
                int desc = 0;
                isConnected = Win32.InternetGetConnectedState(out desc, 0);
            }
            catch
            { }

            return isConnected;
        }

        static InternetConnectionState GetInternetState()
        {
            int desc = 0;
            bool result = Win32.InternetGetConnectedState(out desc, 0);

            return (InternetConnectionState)desc;
        }

        /// <summary>
        /// Local system has a valid connection to the Internet, but it might or might not be currently connected.
        /// </summary>
        public static bool IsConfigured
        {
            get
            {
                InternetConnectionState flag = GetInternetState();
                return (flag & InternetConnectionState.INTERNET_CONNECTION_CONFIGURED) != 0;
            }
        }
        /// <summary>
        /// Local system uses a local area network to connect to the Internet.
        /// </summary>
        public static bool IsConnectedUsingLAN
        {
            get
            {
                InternetConnectionState flag = GetInternetState();
                return (flag & InternetConnectionState.INTERNET_CONNECTION_LAN) != 0;
            }
        }
        /// <summary>
        /// Local system uses a modem to connect to the Internet.
        /// </summary>
        public static bool IsConnectedUsingModem
        {
            get
            {
                InternetConnectionState flag = GetInternetState();
                return (flag & InternetConnectionState.INTERNET_CONNECTION_MODEM) != 0;
            }
        }
        /// <summary>
        /// Local system is in offline mode.
        /// </summary>
        public static bool IsOffline
        {
            get
            {
                InternetConnectionState flag = GetInternetState();
                return (flag & InternetConnectionState.INTERNET_CONNECTION_OFFLINE) != 0;
            }
        }
        /// <summary>
        /// Local system uses a proxy server to connect to the Internet.
        /// </summary>
        public static bool IsProxyUsed
        {
            get
            {
                InternetConnectionState flag = GetInternetState();
                return (flag & InternetConnectionState.INTERNET_CONNECTION_PROXY) != 0;
            }
        }
        /// <summary>
        /// Local system has RAS installed.
        /// </summary>
        public static bool IsRasEnabled
        {
            get
            {
                InternetConnectionState flag = GetInternetState();
                return (flag & InternetConnectionState.INTERNET_RAS_INSTALLED) != 0;
            }
        }


        public static bool HasInternetConnectionWithPing()
        {
            Ping myPing = new Ping();
            String host = "dns.baidu.com";//"131.107.255.255"; //dns.msftncsi.com
            byte[] buffer = new byte[32];
            int timeout = 1000;
            PingOptions pingOptions = new PingOptions();
            try
            {
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                if (reply.Status == IPStatus.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }


        public static bool HasInternetConnectionWithStream()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead("http://www.baidu.com"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }



        /// <summary>
        /// Indicates whether any network connection is available
        /// Filter connections below a specified speed, as well as virtual network cards.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if a network connection is available; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNetworkAvailable()
        {
            return IsNetworkAvailable(0);
        }

        /// <summary>
        /// Indicates whether any network connection is available.
        /// Filter connections below a specified speed, as well as virtual network cards.
        /// </summary>
        /// <param name="minimumSpeed">The minimum speed required. Passing 0 will not filter connection using speed.</param>
        /// <returns>
        ///     <c>true</c> if a network connection is available; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNetworkAvailable(long minimumSpeed)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
                return false;

            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                // discard because of standard reasons
                if ((ni.OperationalStatus != OperationalStatus.Up) ||
                    (ni.NetworkInterfaceType == NetworkInterfaceType.Loopback) ||
                    (ni.NetworkInterfaceType == NetworkInterfaceType.Tunnel))
                    continue;

                // this allow to filter modems, serial, etc.
                // I use 10000000 as a minimum speed for most cases
                if (ni.Speed < minimumSpeed)
                    continue;

                // discard virtual cards (virtual box, virtual pc, etc.)
                if ((ni.Description.IndexOf("virtual", StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (ni.Name.IndexOf("virtual", StringComparison.OrdinalIgnoreCase) >= 0))
                    continue;

                // discard "Microsoft Loopback Adapter", it will not show as NetworkInterfaceType.Loopback but as Ethernet Card.
                if (ni.Description.Equals("Microsoft Loopback Adapter", StringComparison.OrdinalIgnoreCase))
                    continue;

                return true;
            }
            return false;
        }
    }


}
