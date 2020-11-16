namespace Expert_system
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.создатьБЗToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label_question = new System.Windows.Forms.Label();
            this.comboBox_answer = new System.Windows.Forms.ComboBox();
            this.button_ok = new System.Windows.Forms.Button();
            this.button_restart = new System.Windows.Forms.Button();
            this.label_result = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.создатьБЗToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(370, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // создатьБЗToolStripMenuItem
            // 
            this.создатьБЗToolStripMenuItem.Name = "создатьБЗToolStripMenuItem";
            this.создатьБЗToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            this.создатьБЗToolStripMenuItem.Text = "Создать БЗ";
            this.создатьБЗToolStripMenuItem.Click += new System.EventHandler(this.создатьБЗToolStripMenuItem_Click);
            // 
            // label_question
            // 
            this.label_question.AutoSize = true;
            this.label_question.Location = new System.Drawing.Point(12, 37);
            this.label_question.Name = "label_question";
            this.label_question.Size = new System.Drawing.Size(44, 13);
            this.label_question.TabIndex = 1;
            this.label_question.Text = "Вопрос";
            // 
            // comboBox_answer
            // 
            this.comboBox_answer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_answer.FormattingEnabled = true;
            this.comboBox_answer.Location = new System.Drawing.Point(12, 53);
            this.comboBox_answer.Name = "comboBox_answer";
            this.comboBox_answer.Size = new System.Drawing.Size(346, 21);
            this.comboBox_answer.TabIndex = 2;
            // 
            // button_ok
            // 
            this.button_ok.Enabled = false;
            this.button_ok.Location = new System.Drawing.Point(12, 80);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 26);
            this.button_ok.TabIndex = 3;
            this.button_ok.Text = "OK";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_restart
            // 
            this.button_restart.Location = new System.Drawing.Point(283, 150);
            this.button_restart.Name = "button_restart";
            this.button_restart.Size = new System.Drawing.Size(75, 23);
            this.button_restart.TabIndex = 4;
            this.button_restart.Text = "RESTART";
            this.button_restart.UseVisualStyleBackColor = true;
            this.button_restart.Click += new System.EventHandler(this.button_restart_Click);
            // 
            // label_result
            // 
            this.label_result.AutoSize = true;
            this.label_result.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_result.Location = new System.Drawing.Point(12, 122);
            this.label_result.Name = "label_result";
            this.label_result.Size = new System.Drawing.Size(67, 20);
            this.label_result.TabIndex = 5;
            this.label_result.Text = "Result:  ";
            this.label_result.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 185);
            this.Controls.Add(this.label_result);
            this.Controls.Add(this.button_restart);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.comboBox_answer);
            this.Controls.Add(this.label_question);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Экспертная система";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem создатьБЗToolStripMenuItem;
        private System.Windows.Forms.Label label_question;
        private System.Windows.Forms.ComboBox comboBox_answer;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Button button_restart;
        private System.Windows.Forms.Label label_result;
    }
}

