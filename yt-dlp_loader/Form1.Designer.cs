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
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(57, 41);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(891, 27);
            textBox1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(55, 17);
            label1.Name = "label1";
            label1.Size = new Size(75, 20);
            label1.TabIndex = 1;
            label1.Text = "yt-dlp.exe";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(55, 110);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(893, 27);
            textBox2.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(57, 87);
            label2.Name = "label2";
            label2.Size = new Size(53, 20);
            label2.TabIndex = 3;
            label2.Text = "url File";
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 20;
            listBox1.Location = new Point(12, 209);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(1142, 284);
            listBox1.TabIndex = 4;
            // 
            // button1
            // 
            button1.Location = new Point(991, 41);
            button1.Name = "button1";
            button1.Size = new Size(140, 29);
            button1.TabIndex = 5;
            button1.Text = "Start";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(55, 161);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(190, 24);
            checkBox1.TabIndex = 6;
            checkBox1.Text = "URLを既定のブラウザで開く";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(368, 159);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(108, 27);
            textBox3.TabIndex = 7;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(251, 162);
            label3.Name = "label3";
            label3.Size = new Size(111, 20);
            label3.TabIndex = 8;
            label3.Text = "閉じるまでの秒数";
            // 
            // button2
            // 
            button2.Location = new Point(694, 158);
            button2.Name = "button2";
            button2.Size = new Size(94, 29);
            button2.TabIndex = 9;
            button2.Text = "Save";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(588, 160);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(63, 27);
            numericUpDown1.TabIndex = 10;
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(488, 162);
            label4.Name = "label4";
            label4.Size = new Size(85, 20);
            label4.TabIndex = 11;
            label4.Text = "DLスレッド数";
            // 
            // button3
            // 
            button3.Location = new Point(808, 158);
            button3.Name = "button3";
            button3.Size = new Size(140, 29);
            button3.TabIndex = 12;
            button3.Text = "Show Folder";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // Form1
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1166, 518);
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
    }
}