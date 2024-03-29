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
using System.Security.Principal;
using System.IO;
using System.Threading;
using System.Security.Cryptography;
using System.Runtime.InteropServices.ComTypes;
using System.Management;

namespace domain_setings_winforms
{

    public partial class Form1 : Form
    {
        string admin_name = "администратор";
        string admin_password = "Flvby1djnnfrjq";
        string name_machine_in_domain = "";
        string name_machine = "";
        bool domain_status;
        bool trust_domain_bool;
        bool isAdministrator;
        public string proxy = "192.168.100.1:3128";
        double thisVersion;
        double thisVersion_Update;
        double newVersion;
        double newVersion_Update;
        [DllImport("wininet.dll")]
        public static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
        public const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
        public const int INTERNET_OPTION_REFRESH = 37;
        bool settingsReturn, refreshReturn;
        public ToolStripMenuItem MenuItem_notifyIcon_menu = new ToolStripMenuItem ();
        public ToolStripMenuItem MenuItem_notifyIcon_adapter = new ToolStripMenuItem();
        String way_update_program = @"\\Electronic\1\Domain master";
        BackgroundWorker worker = new BackgroundWorker();
        public Form1()
        {
            InitializeComponent();
            MenuItem_notifyIcon_menu.Click += Menu_Click;
            MenuItem_notifyIcon_adapter.Click += on_of_adapter;
            notifyIcon_menu();
            checkBox_auto_run.Checked = check_auto_run();
            isAdmin();
            checkBox3.Checked = Properties.Settings.Default.hide;
            if (!isAdministrator)
            {
                input_domain.Enabled = false;
                button_exit_domain.Enabled = false;
            }
            domain_name();
            PartOfDomain();
            this.Text = "Domain master " + Application.ProductVersion.ToString();
            thisVersion = Convert.ToDouble(ProductVersion.ToString().Replace(".",""));
            try
            {
                FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(AppDomain.CurrentDomain.BaseDirectory + "\\Update.exe");
                thisVersion_Update = Convert.ToDouble(myFileVersionInfo.ProductVersion.ToString().Replace(".", ""));
            }
            catch (Exception)
            {
                MessageBox.Show("Отсутствует файл обновления!",
                "Ошибка!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.ServiceNotification);
            }
            textBox1.Multiline = false;
            if (Properties.Settings.Default.name != null && Properties.Settings.Default.name != "")
            {
                textBox1.Text = Properties.Settings.Default.name;
                name_machine = Properties.Settings.Default.name;
            }
            if (check_proxy(proxy))
                button_proxy.Text = "Выключить прокси";
            MenuItem_notifyIcon_menu.Text = button_proxy.Text;
            if (Properties.Settings.Default.hide)
            {
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
            }
            checkUpdates();
            check_adapter_connect();
        }

        private void notifyIcon_menu()
        {
            notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            notifyIcon.ContextMenuStrip.Items.Add(MenuItem_notifyIcon_menu);
            notifyIcon.ContextMenuStrip.Items.Add(MenuItem_notifyIcon_adapter);

        }
        private void Menu_Click(object sender, EventArgs e)
        {
            if (MenuItem_notifyIcon_menu.Text == "Выключить прокси")
            {
                set_proxy(proxy, 0);
                MenuItem_notifyIcon_menu.Text = "Включить прокси";
                button_proxy.Text = "Включить прокси";
            }
            else
            {
                set_proxy(proxy, 1);
                MenuItem_notifyIcon_menu.Text = "Выключить прокси";
                button_proxy.Text = "Выключить прокси";
            }
        }
        private void isAdmin()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            isAdministrator = principal.IsInRole(WindowsBuiltInRole.Administrator);
            if (!isAdministrator && !checkBox_auto_run.Checked)
                MessageBox.Show("Приложение запущенно без прав администратора, ограничены действия с доменом", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                Arguments = "/c NETDOM JOIN " + newName + " /domain:radio3.aqua.sci-nnov.ru /userd:"+ admin_name + " /passwordd:"+ admin_password,
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
                textBox1.Text = result_Output;
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
                Verb = "runas",
                FileName = "cmd",
                StandardErrorEncoding = Encoding.GetEncoding(866),
                StandardOutputEncoding = Encoding.GetEncoding(866),
                Arguments = "/c netdom REMOVE " + name_machine_in_domain + " /d:radio1.aqua.sci-nnov.ru /ud:"+ admin_name+ " /pd:"+ admin_password /*+ " /Force"*/,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            });
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
                MenuItem_notifyIcon_menu.Text = "Включить прокси";
            }
            else
            {
                set_proxy(proxy, 1);
                button_proxy.Text = "Выключить прокси";
                MenuItem_notifyIcon_menu.Text = "Выключить прокси";
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

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon.Visible = false;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                notifyIcon.Visible = true;
                this.ShowInTaskbar = false;
            }
        }

        private void checkBox_auto_run_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_auto_run.Checked)
            {
                RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                rkApp.SetValue("domain_setings_winforms", Application.ExecutablePath.ToString());
            }
            else
            {
                RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                rkApp.DeleteValue("domain_setings_winforms", false);
            }
        }
        private bool check_auto_run()
        {
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (rkApp.GetValue("domain_setings_winforms") == null)
                return false;
            else
                return true;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.hide = checkBox3.Checked;
            Properties.Settings.Default.Save();
        }

        private bool check_proxy(string _proxy)
        {
            try
            {
                RegistryKey proxy_machine = Registry.CurrentUser;
                RegistryKey proxy_server = proxy_machine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Internet Settings");
                if (proxy_server.GetValue("ProxyServer").ToString() != _proxy)
                    return false;
                if (proxy_server.GetValue("ProxyEnable").ToString() == "0")
                    return false;
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return false;
            }
        }
        private async void checkUpdates()
        {
            await Task.Run(() =>
            {
                bool flag = false;
                while (true)
                {
                    try
                    {
                        FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(way_update_program +"\\"+ Application.ProductName + ".exe");
                        newVersion = Convert.ToDouble(myFileVersionInfo.ProductVersion.ToString().Replace(".", ""));
                        if (thisVersion < newVersion)
                        {
                            DialogResult result = MessageBox.Show("Обнаружена новая версия программы " + myFileVersionInfo.ProductVersion.ToString() + "\r\nУстановить сейчас?",
                            "Внимание!",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.ServiceNotification);
                            if (result == DialogResult.Yes)
                            {
                                if (isAdministrator)
                                {
                                    Form_download form_Download = new Form_download(Application.ProductName);
                                    form_Download.ShowDialog();
                                }
                                else
                                {
                                    restart();
                                }
                            }
                            else if (result == DialogResult.No)
                            {
                                Text += " доступно обновление до версии " + myFileVersionInfo.ProductVersion.ToString();
                                break;
                            }
                            Activate();
                        }
                        FileVersionInfo myFileVersionInfo_update = FileVersionInfo.GetVersionInfo(way_update_program + "\\Update.exe");
                        newVersion_Update = Convert.ToDouble(myFileVersionInfo_update.ProductVersion.ToString().Replace(".", ""));
                        if (thisVersion_Update < newVersion_Update)
                        {
                            DialogResult result = MessageBox.Show("Обнаружена новая версия программы обновления " + myFileVersionInfo_update.ProductVersion.ToString() + "\r\nУстановить сейчас?",
                            "Внимание!",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.ServiceNotification);
                            if (result == DialogResult.Yes)
                            {
                                if (isAdministrator)
                                {
                                    Form_download form_Download = new Form_download("Update");
                                    form_Download.ShowDialog();
                                }
                                else
                                {
                                    restart();
                                }
                            }
                            else if (result == DialogResult.No)
                            {
                                Text += " доступна новая версия программы обновления " + myFileVersionInfo_update.ProductVersion.ToString();
                                break;
                            }
                            //this.Activate();
                        }
                        Text = "Domain master " + Application.ProductVersion.ToString();
                        flag = false;
                    }
                    catch (Exception e )
                    {
                        if (!flag)
                        {
                            MessageBox.Show("Нет доступа к серверу обновления \r\n"+e,
                            "Ошибка!",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.ServiceNotification);
                            flag = true;
                        }
                        else
                        {
                            Text += " Нет доступа к серверу обновления";
                        }
                    }
                    Thread.Sleep(30000);
                }
            });
            
        }
        private void restart()
        {
            MessageBox.Show("Программа запущена не от имени администратора, сейчас она будет перезагружена",
            "Внимание!",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information,
            MessageBoxDefaultButton.Button1,
            MessageBoxOptions.ServiceNotification);
            Process proc = new Process();
            proc.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + Application.ProductName + ".exe";
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Verb = "runas";
            proc.Start();
            Application.Exit();
        }
        private void on_of_adapter(object sender, EventArgs e)
        {
            Process process = Process.Start(new ProcessStartInfo
            {
                FileName = "Ethernet_adapter.exe",
                Verb = "runas",
             }); 
        }
        private delegate void Deleg(string text);
        private void set_MenuItem_notifyIcon_adapter_text(string text)
        {
            MenuItem_notifyIcon_adapter.Text = text;
            button_settings_adapter.Text = text;
        }

        private void button_settings_adapter_Click(object sender, EventArgs e)
        {
            Process process = Process.Start(new ProcessStartInfo
            {
                FileName = "Ethernet_adapter.exe",
                Verb = "runas",
            });
        }

        private async void check_adapter_connect()
        {
            await Task.Run(() =>
            {
                while (true)
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
                                BeginInvoke(new Deleg(set_MenuItem_notifyIcon_adapter_text), "Ethernet адаптер отключен");
                            else
                                BeginInvoke(new Deleg(set_MenuItem_notifyIcon_adapter_text), "Ethernet адаптер подключен");
                        }
                    }
                    Thread.Sleep(3000);
                }
            });
        }
    }
}
