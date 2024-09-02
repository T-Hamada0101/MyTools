namespace yt_dlp_loader
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            textBox1 = new TextBox();
            label1 = new Label();
            textBox2 = new TextBox();
            label2 = new Label();
            listBox1 = new ListBox();
            button1 = new Button();
            checkBox1 = new CheckBox();
            textBox3 = new TextBox();
            label3 = new Label();
            button2 = new Button();
            numericUpDown1 = new NumericUpDown();
            label4 = new Label();
            button3 = new Button();
            checkBox2 = new CheckBox();
            checkBox3 = new CheckBox();
            textBox4 = new TextBox();
            checkBox4 = new CheckBox();
            label5 = new Label();
            checkBox5 = new CheckBox();
            textBox5 = new TextBox();
            checkBox6 = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(83, 17);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(891, 27);
            textBox1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 20);
            label1.Name = "label1";
            label1.Size = new Size(75, 20);
            label1.TabIndex = 1;
            label1.Text = "yt-dlp.exe";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(83, 57);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(668, 27);
            textBox2.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 64);
            label2.Name = "label2";
            label2.Size = new Size(53, 20);
            label2.TabIndex = 3;
            label2.Text = "url File";
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 20;
            listBox1.Location = new Point(12, 222);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(1142, 284);
            listBox1.TabIndex = 4;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // button1
            // 
            button1.Location = new Point(991, 133);
            button1.Name = "button1";
            button1.Size = new Size(163, 39);
            button1.TabIndex = 5;
            button1.Text = "Start";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(25, 112);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(190, 24);
            checkBox1.TabIndex = 6;
            checkBox1.Text = "URLを既定のブラウザで開く";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(142, 139);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(108, 27);
            textBox3.TabIndex = 7;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(25, 142);
            label3.Name = "label3";
            label3.Size = new Size(111, 20);
            label3.TabIndex = 8;
            label3.Text = "閉じるまでの秒数";
            // 
            // button2
            // 
            button2.Location = new Point(880, 142);
            button2.Name = "button2";
            button2.Size = new Size(94, 29);
            button2.TabIndex = 9;
            button2.Text = "Save";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(867, 58);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(63, 27);
            numericUpDown1.TabIndex = 10;
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(767, 60);
            label4.Name = "label4";
            label4.Size = new Size(85, 20);
            label4.TabIndex = 11;
            label4.Text = "DLスレッド数";
            // 
            // button3
            // 
            button3.Location = new Point(831, 105);
            button3.Name = "button3";
            button3.Size = new Size(143, 29);
            button3.TabIndex = 12;
            button3.Text = "Show DL Folder";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Location = new Point(511, 112);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(172, 24);
            checkBox2.TabIndex = 13;
            checkBox2.Text = "DLファイル名にidを付加";
            checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            checkBox3.AutoSize = true;
            checkBox3.Location = new Point(286, 110);
            checkBox3.Name = "checkBox3";
            checkBox3.Size = new Size(219, 24);
            checkBox3.TabIndex = 14;
            checkBox3.Text = "DLファイル名にuploaderを付加";
            checkBox3.UseVisualStyleBackColor = true;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(511, 147);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(163, 27);
            textBox4.TabIndex = 15;
            // 
            // checkBox4
            // 
            checkBox4.AutoSize = true;
            checkBox4.Location = new Point(487, 150);
            checkBox4.Name = "checkBox4";
            checkBox4.Size = new Size(18, 17);
            checkBox4.TabIndex = 17;
            checkBox4.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(286, 147);
            label5.Name = "label5";
            label5.Size = new Size(194, 20);
            label5.TabIndex = 18;
            label5.Text = "DLファイル名に次の文字を付加";
            // 
            // checkBox5
            // 
            checkBox5.AutoSize = true;
            checkBox5.Location = new Point(681, 150);
            checkBox5.Name = "checkBox5";
            checkBox5.Size = new Size(18, 17);
            checkBox5.TabIndex = 20;
            checkBox5.UseVisualStyleBackColor = true;
            // 
            // textBox5
            // 
            textBox5.Location = new Point(705, 147);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(147, 27);
            textBox5.TabIndex = 19;
            // 
            // checkBox6
            // 
            checkBox6.AutoSize = true;
            checkBox6.Location = new Point(25, 182);
            checkBox6.Name = "checkBox6";
            checkBox6.Size = new Size(136, 24);
            checkBox6.TabIndex = 21;
            checkBox6.Text = "720p以下に制限";
            checkBox6.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1166, 526);
            Controls.Add(checkBox6);
            Controls.Add(checkBox5);
            Controls.Add(textBox5);
            Controls.Add(label5);
            Controls.Add(checkBox4);
            Controls.Add(textBox4);
            Controls.Add(checkBox3);
            Controls.Add(checkBox2);
            Controls.Add(button3);
            Controls.Add(label4);
            Controls.Add(numericUpDown1);
            Controls.Add(button2);
            Controls.Add(label3);
            Controls.Add(textBox3);
            Controls.Add(checkBox1);
            Controls.Add(button1);
            Controls.Add(listBox1);
            Controls.Add(label2);
            Controls.Add(textBox2);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private Label label1;
        private TextBox textBox2;
        private Label label2;
        private ListBox listBox1;
        private Button button1;
        private CheckBox checkBox1;
        private TextBox textBox3;
        private Label label3;
        private Button button2;
        private NumericUpDown numericUpDown1;
        private Label label4;
        private Button button3;
        private CheckBox checkBox2;
        private CheckBox checkBox3;
        private TextBox textBox4;
        private CheckBox checkBox4;
        private Label label5;
        private CheckBox checkBox5;
        private TextBox textBox5;
        private CheckBox checkBox6;
    }
}