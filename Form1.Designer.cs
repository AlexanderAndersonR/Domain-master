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
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 27);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(293, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // input_domain
            // 
            this.input_domain.Location = new System.Drawing.Point(363, 25);
            this.input_domain.Name = "input_domain";
            this.input_domain.Size = new System.Drawing.Size(136, 23);
            this.input_domain.TabIndex = 1;
            this.input_domain.Text = "Ввести в домен";
            this.input_domain.UseVisualStyleBackColor = true;
            this.input_domain.Click += new System.EventHandler(this.input_domain_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Имя компьютера";
            // 
            // reset
            // 
            this.reset.Location = new System.Drawing.Point(363, 54);
            this.reset.Name = "reset";
            this.reset.Size = new System.Drawing.Size(136, 23);
            this.reset.TabIndex = 3;
            this.reset.Text = "Перезагрузка";
            this.reset.UseVisualStyleBackColor = true;
            this.reset.Click += new System.EventHandler(this.reset_Click);
            // 
            // shutdown
            // 
            this.shutdown.Location = new System.Drawing.Point(363, 83);
            this.shutdown.Name = "shutdown";
            this.shutdown.Size = new System.Drawing.Size(136, 23);
            this.shutdown.TabIndex = 4;
            this.shutdown.Text = "Выключение";
            this.shutdown.UseVisualStyleBackColor = true;
            this.shutdown.Click += new System.EventHandler(this.shutdown_Click);
            // 
            // PowerShell
            // 
            this.PowerShell.Location = new System.Drawing.Point(363, 113);
            this.PowerShell.Name = "PowerShell";
            this.PowerShell.Size = new System.Drawing.Size(136, 23);
            this.PowerShell.TabIndex = 5;
            this.PowerShell.Text = "Проверка на доверие";
            this.PowerShell.UseVisualStyleBackColor = true;
            this.PowerShell.Click += new System.EventHandler(this.PowerShell_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Enabled = false;
            this.checkBox1.Location = new System.Drawing.Point(16, 60);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(119, 17);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "Наличие в домене";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Enabled = false;
            this.checkBox2.Location = new System.Drawing.Point(16, 83);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(74, 17);
            this.checkBox2.TabIndex = 8;
            this.checkBox2.Text = "Доверие ";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // button_exit_domain
            // 
            this.button_exit_domain.Location = new System.Drawing.Point(363, 142);
            this.button_exit_domain.Name = "button_exit_domain";
            this.button_exit_domain.Size = new System.Drawing.Size(136, 23);
            this.button_exit_domain.TabIndex = 9;
            this.button_exit_domain.Text = "Выйти из домена";
            this.button_exit_domain.UseVisualStyleBackColor = true;
            this.button_exit_domain.Click += new System.EventHandler(this.button_exit_domain_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 176);
            this.Controls.Add(this.button_exit_domain);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.PowerShell);
            this.Controls.Add(this.shutdown);
            this.Controls.Add(this.reset);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.input_domain);
            this.Controls.Add(this.textBox1);
            this.MaximumSize = new System.Drawing.Size(549, 215);
            this.MinimumSize = new System.Drawing.Size(549, 215);
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
    }
}

