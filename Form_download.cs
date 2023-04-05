using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace domain_setings_winforms
{
    public partial class Form_download : Form
    {
        BackgroundWorker worker = new BackgroundWorker();
        String path = @"\\Electronic\1\Domain master";
        List<string> filesDir1 = new List<string>();
        int count = 0;
        String exe { get; set; }
        public Form_download(String name)
        {
            InitializeComponent();
            exe = name;
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerAsync();
        }
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //CopyFile(@"C:\Users\Kaf\Desktop\VC_redist.x64.exe", @"C:\Users\Kaf\Desktop\Новая папка\VC_redist.x64.exe");
            filesDir1 = (from a in Directory.GetFiles(path) select Path.GetFileName(a)).ToList();
            //label1.Text = "Файл " + count.ToString() + " из " + filesDir1.Count.ToString();
            //progressBar1.Value = (count * 100) / filesDir1.Count;
            DirectoryInfo dirInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "\\update file");
            if (!dirInfo.Exists)
                dirInfo.Create();
            foreach (var item in filesDir1)
            {
                CopyFile(path + "\\" + item, dirInfo.FullName+"\\" + item);
                count++;
                //label1.Text = "Файл " + count.ToString() + " из " + filesDir1.Count.ToString();
                //progressBar1.Value = (count * 100) / filesDir1.Count;
            }
            DialogResult result = MessageBox.Show("Копирование новой версии успешно завершино"+"\r\nУстановить обновление сейчас?",
                "Внимание",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.ServiceNotification);
            if (result == DialogResult.Yes)
            {
                if(exe == "Update")
                {
                    filesDir1.Clear();
                    filesDir1 = (from a in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "\\update file") select Path.GetFileName(a)).ToList();
                    count = 0;
                    foreach (var item in filesDir1)
                    {
                        if(item.Contains("Update"))
                            CopyFile(AppDomain.CurrentDomain.BaseDirectory + "\\update file\\" + item, AppDomain.CurrentDomain.BaseDirectory + "\\" + item);
                        count++;
                        //label1.Text = "Файл " + count.ToString() + " из " + filesDir1.Count.ToString();
                        //progressBar1.Value = (count * 100) / filesDir1.Count;
                    }
                    MessageBox.Show("Установка новой версии программы обновления завершина",
                    "Внимание",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.ServiceNotification);
                    dirInfo.Delete(true);
                    BeginInvoke(new Deleg(Close));
                }
                else 
                {
                    Process.Start(AppDomain.CurrentDomain.BaseDirectory + "Update.exe");
                    Application.Exit();
                }
            }
            else if (result == DialogResult.No)
               BeginInvoke(new Deleg(Close));
        }
        private delegate void Deleg();
        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar2.Value = e.ProgressPercentage;
            label3.Text = progressBar2.Value.ToString() + "%";
            label1.Text = "Файл " + count.ToString() + " из " + filesDir1.Count.ToString();
            progressBar1.Value = (count * 100) / filesDir1.Count;
            label2.Text = progressBar1.Value.ToString() + "%";
        }
        private void CopyFile(string soure, string des)
        {
            FileStream fsOut = new FileStream(des, FileMode.Create);
            FileStream fsIn = new FileStream(soure, FileMode.Open);
            byte[] bt = new byte[1000000];
            int readByte;
            while ((readByte = fsIn.Read(bt, 0, bt.Length)) > 0)
            {
                fsOut.Write(bt, 0, readByte);
                worker.ReportProgress((int)(fsIn.Position * 100 / fsIn.Length));
            }
            fsIn.Close();
            fsOut.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            worker.RunWorkerAsync();
        }
    }
}
