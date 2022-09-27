namespace domain_setings_winforms
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.input_domain = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.reset = new System.Windows.Forms.Button();
            this.shutdown = new System.Windows.Forms.Button();
            this.PowerShell = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.button_exit_domain = new System.Windows.Forms.Button();
            this.button_proxy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(16, 33);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(389, 24);
            this.textBox1.TabIndex = 0;
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // input_domain
            // 
            this.input_domain.Location = new System.Drawing.Point(484, 31);
            this.input_domain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.input_domain.Name = "input_domain";
            this.input_domain.Size = new System.Drawing.Size(181, 28);
            this.input_domain.TabIndex = 1;
            this.input_domain.Text = "Ввести в домен";
            this.input_domain.UseVisualStyleBackColor = true;
            this.input_domain.Click += new System.EventHandler(this.input_domain_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Имя компьютера";
            // 
            // reset
            // 
            this.reset.Location = new System.Drawing.Point(484, 66);
            this.reset.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.reset.Name = "reset";
            this.reset.Size = new System.Drawing.Size(181, 28);
            this.reset.TabIndex = 3;
            this.reset.Text = "Перезагрузка";
            this.reset.UseVisualStyleBackColor = true;
            this.reset.Click += new System.EventHandler(this.reset_Click);
            // 
            // shutdown
            // 
            this.shutdown.Location = new System.Drawing.Point(484, 102);
            this.shutdown.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.shutdown.Name = "shutdown";
            this.shutdown.Size = new System.Drawing.Size(181, 28);
            this.shutdown.TabIndex = 4;
            this.shutdown.Text = "Выключение";
            this.shutdown.UseVisualStyleBackColor = true;
            this.shutdown.Click += new System.EventHandler(this.shutdown_Click);
            // 
            // PowerShell
            // 
            this.PowerShell.Location = new System.Drawing.Point(484, 139);
            this.PowerShell.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PowerShell.Name = "PowerShell";
            this.PowerShell.Size = new System.Drawing.Size(181, 28);
            this.PowerShell.TabIndex = 5;
            this.PowerShell.Text = "Проверка на доверие";
            this.PowerShell.UseVisualStyleBackColor = true;
            this.PowerShell.Click += new System.EventHandler(this.PowerShell_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Enabled = false;
            this.checkBox1.Location = new System.Drawing.Point(21, 74);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(150, 20);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "Наличие в домене";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Enabled = false;
            this.checkBox2.Location = new System.Drawing.Point(21, 102);
            this.checkBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(89, 20);
            this.checkBox2.TabIndex = 8;
            this.checkBox2.Text = "Доверие ";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // button_exit_domain
            // 
            this.button_exit_domain.Location = new System.Drawing.Point(484, 175);
            this.button_exit_domain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_exit_domain.Name = "button_exit_domain";
            this.button_exit_domain.Size = new System.Drawing.Size(181, 28);
            this.button_exit_domain.TabIndex = 9;
            this.button_exit_domain.Text = "Выйти из домена";
            this.button_exit_domain.UseVisualStyleBackColor = true;
            this.button_exit_domain.Click += new System.EventHandler(this.button_exit_domain_Click);
            // 
            // button_proxy
            // 
            this.button_proxy.Location = new System.Drawing.Point(484, 211);
            this.button_proxy.Margin = new System.Windows.Forms.Padding(4);
            this.button_proxy.Name = "button_proxy";
            this.button_proxy.Size = new System.Drawing.Size(181, 28);
            this.button_proxy.TabIndex = 10;
            this.button_proxy.Text = "Включить прокси ";
            this.button_proxy.UseVisualStyleBackColor = true;
            this.button_proxy.Click += new System.EventHandler(this.button_proxy_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(708, 247);
            this.Controls.Add(this.button_proxy);
            this.Controls.Add(this.button_exit_domain);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.PowerShell);
            this.Controls.Add(this.shutdown);
            this.Controls.Add(this.reset);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.input_domain);
            this.Controls.Add(this.textBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MinimumSize = new System.Drawing.Size(726, 254);
            this.Name = "Form1";
            this.Text = "Domain master ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button input_domain;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button reset;
        private System.Windows.Forms.Button shutdown;
        private System.Windows.Forms.Button PowerShell;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Button button_exit_domain;
        private System.Windows.Forms.Button button_proxy;
    }
}

