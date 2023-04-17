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

namespace Update
{
    public partial class Form_update : Form
    {
        BackgroundWorker worker = new BackgroundWorker();
        List<string> filesDir1 = new List<string>();
        int count = 0;
        public Form_update()
        {
            InitializeComponent();
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerAsync();
        }
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            filesDir1 = (from a in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "\\update file") select Path.GetFileName(a)).ToList();
            foreach (var item in filesDir1)
            {
                if(!item.Contains("Update"))
                    CopyFile(AppDomain.CurrentDomain.BaseDirectory + "\\update file\\" + item, AppDomain.CurrentDomain.BaseDirectory + "\\" + item);
                count++;
            }
            DirectoryInfo dirInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "\\update file");
            dirInfo.Delete(true);
            DialogResult result = MessageBox.Show("Обновление успешно установленно" + "\r\nЗапустить программу?",
                "Внимание",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.ServiceNotification);
            if (result == DialogResult.Yes)
            {
                Process.Start(AppDomain.CurrentDomain.BaseDirectory + "domain_setings_winforms.exe");
                Application.Exit();
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

            //https://www.youtube.com/watch?v=c1f3KOpXgjQ
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
            //MessageBox.Show("complite");
        }
    }
}
