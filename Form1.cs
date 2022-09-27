﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace domain_setings_winforms
{

    public partial class Form1 : Form
    {
        string name_machine_in_domain = "";
        string name_machine = "";
        bool domain_status;
        bool trust_domain_bool;

        public string proxy = "192.168.100.1:3128";

        [DllImport("wininet.dll")]
        public static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
        public const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
        public const int INTERNET_OPTION_REFRESH = 37;
        bool settingsReturn, refreshReturn;
        
        public Form1()
        {
            InitializeComponent();
            domain_name();
            PartOfDomain();
            this.Text = "Domain master " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            textBox1.Multiline = false;
            if (Properties.Settings.Default.name != null && Properties.Settings.Default.name != "")
            {
                textBox1.Text = Properties.Settings.Default.name;
                name_machine = Properties.Settings.Default.name;
            }
            if (check_proxy(proxy))
                button_proxy.Text = "Выключить прокси";
        }

        private void input_domain_Click(object sender, EventArgs e)
        {
            name_machine = textBox1.Text;
            changes_name(name_machine);
        }
        private void changes_name(string newName)
        {
            if (newName != null && newName !="")
            {
                RegistryKey key = Registry.LocalMachine;
                string activeComputerName = "SYSTEM\\CurrentControlSet\\Control\\ComputerName\\ActiveComputerName";
                RegistryKey activeCmpName = key.CreateSubKey(activeComputerName);
                activeCmpName.SetValue("ComputerName", newName);
                activeCmpName.Close();
                string computerName = "SYSTEM\\CurrentControlSet\\Control\\ComputerName\\ComputerName";
                RegistryKey cmpName = key.CreateSubKey(computerName);
                cmpName.SetValue("ComputerName", newName);
                cmpName.Close();
                string _hostName = "SYSTEM\\CurrentControlSet\\services\\Tcpip\\Parameters\\";
                RegistryKey hostName = key.CreateSubKey(_hostName);
                hostName.SetValue("Hostname", newName);
                hostName.SetValue("NV Hostname", newName);
                hostName.Close();
                new_domain(newName);
            }
            else
            {
                MessageBox.Show("Не задано имя компьютера!");
            }
        }
        private void new_domain(string newName)
        {
            Process process = Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                StandardErrorEncoding = Encoding.GetEncoding(866),
                StandardOutputEncoding = Encoding.GetEncoding(866),
                Arguments = "/c netdom join " + newName + " /domain:radio1.aqua.sci-nnov.ru /userd:1 /passwordd:362",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            });;
            string result_Output = process.StandardOutput.ReadToEnd().Replace("Active code page: 65001", "");
            string result_Error = process.StandardError.ReadToEnd();
            if (!String.IsNullOrWhiteSpace(result_Output) && !String.IsNullOrEmpty(result_Output))
            {
                MessageBox.Show(result_Output, "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (!String.IsNullOrWhiteSpace(result_Error) && !String.IsNullOrEmpty(result_Error))
            {
                MessageBox.Show(result_Error, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            PartOfDomain();
        }

        private void reset_Click(object sender, EventArgs e)
        {
            Process process = Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = "/c shutdown /r /t 0",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
            });
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.name = textBox1.Text;
            Properties.Settings.Default.Save();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                Properties.Settings.Default.name = textBox1.Text;
                Properties.Settings.Default.Save();
                input_domain_Click(sender, e);
            }
        }
        private void shutdown_Click(object sender, EventArgs e)
        {
            Process process = Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = "/c shutdown /s /t 0",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
            });
        }

        private void PowerShell_Click(object sender, EventArgs e)
        {
                try
                {
                    Process process = Process.Start(new ProcessStartInfo
                    {
                        FileName = "powershell",
                        Arguments = "/c Test-ComputerSecureChannel",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,

                        StandardErrorEncoding = Encoding.GetEncoding(866),
                        StandardOutputEncoding = Encoding.GetEncoding(866),
                    }); ;
                    string result_Outrut = process.StandardOutput.ReadToEnd();
                    string result_Error = process.StandardError.ReadToEnd();
                    if (!String.IsNullOrWhiteSpace(result_Outrut) && !String.IsNullOrEmpty(result_Outrut))
                    {
                        if (result_Outrut.Contains("True"))
                        {
                            MessageBox.Show("Доверительные отношения установленны", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            trust_domain_bool = true;
                        }
                        else if (result_Outrut.Contains("False"))
                        {
                            MessageBox.Show("Доверительные отношения ОТСУТСТВУЮТ", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            trust_domain_bool = false;
                        }
                        checkBox2.Checked = trust_domain_bool;
                    }
                    if (!String.IsNullOrWhiteSpace(result_Error) && !String.IsNullOrEmpty(result_Error))
                    {
                        MessageBox.Show(result_Error, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }
        private void PartOfDomain()
        {
            Process process = Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                StandardErrorEncoding = Encoding.GetEncoding(866),
                StandardOutputEncoding = Encoding.GetEncoding(866),
                Arguments = "/c wmic ComputerSystem get PartOfDomain",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            }); ;
            string result_Output = process.StandardOutput.ReadToEnd().Replace("PartOfDomain", "").Trim();
            string result_Error = process.StandardError.ReadToEnd();
            if (!String.IsNullOrWhiteSpace(result_Output) && !String.IsNullOrEmpty(result_Output))
            {
                if (result_Output=="TRUE")
                {
                    domain_status = true;
                }
                else if (result_Output=="FALSE")
                {
                    domain_status = false;
                }
                checkBox1.Checked = domain_status;
                if (domain_status)
                {
                    input_domain.Enabled = false;
                    PowerShell.Enabled = domain_status;
                    trust_domain();
                }
                else
                {
                    input_domain.Enabled=true;
                    PowerShell.Enabled = domain_status;
                    checkBox2.Checked = domain_status;
                }
            }
            if (!String.IsNullOrWhiteSpace(result_Error) && !String.IsNullOrEmpty(result_Error))
            {
                MessageBox.Show(result_Error, "Ошибка при получении информации о состоянии домена", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void domain_name()
        {
            Process process = Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                StandardErrorEncoding = Encoding.GetEncoding(866),
                StandardOutputEncoding = Encoding.GetEncoding(866),
                Arguments = "/c wmic ComputerSystem get Name",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            }); ;
            string result_Output = process.StandardOutput.ReadToEnd().Replace("Name", "").Trim();
            string result_Error = process.StandardError.ReadToEnd();
            if (!String.IsNullOrWhiteSpace(result_Output) && !String.IsNullOrEmpty(result_Output))
            {
                name_machine_in_domain=result_Output;
                label1.Text = "Имя компьютера: " + result_Output;
            }
            if (!String.IsNullOrWhiteSpace(result_Error) && !String.IsNullOrEmpty(result_Error))
            {
                MessageBox.Show(result_Error, "Ошибка при получении имени компьютера", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void trust_domain()
        {
            try
            {
                Process process = Process.Start(new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = "/c Test-ComputerSecureChannel",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                }); ;
                string result_Outrut = process.StandardOutput.ReadToEnd();
                string result_Error = process.StandardError.ReadToEnd();
                if (!String.IsNullOrWhiteSpace(result_Outrut) && !String.IsNullOrEmpty(result_Outrut))
                {
                    if (result_Outrut.Contains("True"))
                    {
                        trust_domain_bool = true;
                    }
                    else if (result_Outrut.Contains("False"))
                    {
                        trust_domain_bool = false;
                    }
                    checkBox2.Checked = trust_domain_bool;
                }
                if (!String.IsNullOrWhiteSpace(result_Error) && !String.IsNullOrEmpty(result_Error))
                {
                    MessageBox.Show(result_Error, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_exit_domain_Click(object sender, EventArgs e)
        {
            domain_name();
            Process process = Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                StandardErrorEncoding = Encoding.GetEncoding(866),
                StandardOutputEncoding = Encoding.GetEncoding(866),
                Arguments = "/c netdom remove " + name_machine_in_domain + " /Domain:radio1.aqua.sci-nnov.ru /UserD:1 /PasswordD:362 /Force",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            }); ;
            string result_Output = process.StandardOutput.ReadToEnd().Replace("Active code page: 65001", "");
            string result_Error = process.StandardError.ReadToEnd();
            if (!String.IsNullOrWhiteSpace(result_Output) && !String.IsNullOrEmpty(result_Output))
            {
                MessageBox.Show(result_Output, "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (!String.IsNullOrWhiteSpace(result_Error) && !String.IsNullOrEmpty(result_Error))
            {
                MessageBox.Show(result_Error, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            PartOfDomain();
        }

        private void button_proxy_Click(object sender, EventArgs e)
        {
            if(button_proxy.Text== "Выключить прокси")
            {
                set_proxy(proxy, 0);
                button_proxy.Text = "Включить прокси";
            }
            else
            {
                set_proxy(proxy, 1);
                button_proxy.Text = "Выключить прокси";
            }
        }

        private void set_proxy(string _proxy,int ProxyEnable)
        {
            RegistryKey proxy_machine = Registry.CurrentUser;
            RegistryKey proxy_server = proxy_machine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Internet Settings");
            proxy_server.SetValue("ProxyEnable", ProxyEnable);
            if(ProxyEnable==1) 
                proxy_server.SetValue("ProxyServer", _proxy);
            proxy_server.Close();
            proxy_machine.Close();

            settingsReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
            refreshReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
        }
        private bool check_proxy(string _proxy)
        {
            RegistryKey proxy_machine = Registry.CurrentUser;
            RegistryKey proxy_server = proxy_machine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Internet Settings");
            if (proxy_server.GetValue("ProxyServer").ToString()!=_proxy)
                return false;
            if(proxy_server.GetValue("ProxyEnable").ToString()=="0")
                return false;
            return true;
        }
    }
}
