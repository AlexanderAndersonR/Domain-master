using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ethernet_adapter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Process process = Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = "c/ netsh interface set interface Ethernet disable",
                Verb = "runas",
                //UseShellExecute = false,
                //CreateNoWindow = true,
                //RedirectStandardOutput = true,
                //RedirectStandardError = true,

                //StandardErrorEncoding = Encoding.GetEncoding(866),
                //StandardOutputEncoding = Encoding.GetEncoding(866),
            });
        }
    }
}
