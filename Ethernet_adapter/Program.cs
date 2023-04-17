using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Ethernet_adapter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SelectQuery wmiQuery = new SelectQuery("SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionId != NULL");
            ManagementObjectSearcher searchProcedure = new ManagementObjectSearcher(wmiQuery);
            foreach (ManagementObject item in searchProcedure.Get())
            {
                string name = (string)item["NetConnectionId"];
                if (name == "Ethernet")
                {
                    int status = Convert.ToInt16(item["NetConnectionStatus"]);
                    if (status == 0)
                        item.InvokeMethod("Enable", null);
                    else
                        item.InvokeMethod("Disable", null);
                }
            }
        }
    }
}
