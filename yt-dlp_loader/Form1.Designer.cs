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
            textBoxExePath = new TextBox();
            label1 = new Label();
            textBoxUrlFile = new TextBox();
            label2 = new Label();
            listBox1 = new ListBox();
            buttonStart = new Button();
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
            button4 = new Button();
            button5 = new Button();
            label6 = new Label();
            textBoxDLFolder = new TextBox();
            comboBox1 = new ComboBox();
            textBoxConfigFile = new TextBox();
            label7 = new Label();
            checkBox7 = new CheckBox();
            textBox1 = new TextBox();
            label8 = new Label();
            buttonUpdate = new Button();
            textBoxConsole = new TextBox();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            SuspendLayout();
            // 
            // textBoxExePath
            // 
            textBoxExePath.Location = new Point(104, 22);
            textBoxExePath.Margin = new Padding(4, 3, 4, 3);
            textBoxExePath.Name = "textBoxExePath";
            textBoxExePath.Size = new Size(1113, 31);
            textBoxExePath.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(14, 25);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(90, 25);
            label1.TabIndex = 1;
            label1.Text = "yt-dlp.exe";
            // 
            // textBoxUrlFile
            // 
            textBoxUrlFile.Location = new Point(104, 72);
            textBoxUrlFile.Margin = new Padding(4, 3, 4, 3);
            textBoxUrlFile.Name = "textBoxUrlFile";
            textBoxUrlFile.Size = new Size(834, 31);
            textBoxUrlFile.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 75);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(63, 25);
            label2.TabIndex = 3;
            label2.Text = "url File";
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 25;
            listBox1.Location = new Point(13, 357);
            listBox1.Margin = new Padding(4, 3, 4, 3);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(1427, 254);
            listBox1.TabIndex = 4;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // buttonStart
            // 
            buttonStart.Location = new Point(1236, 263);
            buttonStart.Margin = new Padding(4, 3, 4, 3);
            buttonStart.Name = "buttonStart";
            buttonStart.Size = new Size(204, 48);
            buttonStart.TabIndex = 5;
            buttonStart.Text = "Start";
            buttonStart.UseVisualStyleBackColor = true;
            buttonStart.Click += buttonStart_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(374, 184);
            checkBox1.Margin = new Padding(4, 3, 4, 3);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(230, 29);
            checkBox1.TabIndex = 6;
            checkBox1.Text = "URLを既定のブラウザで開く";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(761, 182);
            textBox3.Margin = new Padding(4, 3, 4, 3);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(134, 31);
            textBox3.TabIndex = 7;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(617, 183);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(136, 25);
            label3.TabIndex = 8;
            label3.Text = "閉じるまでの秒数";
            // 
            // button2
            // 
            button2.Location = new Point(1097, 273);
            button2.Margin = new Padding(4, 3, 4, 3);
            button2.Name = "button2";
            button2.Size = new Size(117, 37);
            button2.TabIndex = 9;
            button2.Text = "Save";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(1084, 73);
            numericUpDown1.Margin = new Padding(4, 3, 4, 3);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(79, 31);
            numericUpDown1.TabIndex = 10;
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(959, 75);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(103, 25);
            label4.TabIndex = 11;
            label4.Text = "DLスレッド数";
            // 
            // button3
            // 
            button3.Location = new Point(1248, 92);
            button3.Margin = new Padding(4, 3, 4, 3);
            button3.Name = "button3";
            button3.Size = new Size(179, 91);
            button3.TabIndex = 12;
            button3.Text = "Show DL Folder";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Location = new Point(506, 307);
            checkBox2.Margin = new Padding(4, 3, 4, 3);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(207, 29);
            checkBox2.TabIndex = 13;
            checkBox2.Text = "DLファイル名にidを付加";
            checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            checkBox3.AutoSize = true;
            checkBox3.Location = new Point(224, 304);
            checkBox3.Margin = new Padding(4, 3, 4, 3);
            checkBox3.Name = "checkBox3";
            checkBox3.Size = new Size(263, 29);
            checkBox3.TabIndex = 14;
            checkBox3.Text = "DLファイル名にuploaderを付加";
            checkBox3.UseVisualStyleBackColor = true;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(412, 267);
            textBox4.Margin = new Padding(4, 3, 4, 3);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(389, 31);
            textBox4.TabIndex = 15;
            // 
            // checkBox4
            // 
            checkBox4.AutoSize = true;
            checkBox4.Location = new Point(382, 271);
            checkBox4.Margin = new Padding(4, 3, 4, 3);
            checkBox4.Name = "checkBox4";
            checkBox4.Size = new Size(22, 21);
            checkBox4.TabIndex = 17;
            checkBox4.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(28, 267);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(286, 25);
            label5.TabIndex = 18;
            label5.Text = "DLファイル名の最後に次の文字を付加";
            // 
            // checkBox5
            // 
            checkBox5.AutoSize = true;
            checkBox5.Location = new Point(831, 271);
            checkBox5.Margin = new Padding(4, 3, 4, 3);
            checkBox5.Name = "checkBox5";
            checkBox5.Size = new Size(22, 21);
            checkBox5.TabIndex = 20;
            checkBox5.UseVisualStyleBackColor = true;
            // 
            // textBox5
            // 
            textBox5.Location = new Point(861, 267);
            textBox5.Margin = new Padding(4, 3, 4, 3);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(183, 31);
            textBox5.TabIndex = 19;
            // 
            // checkBox6
            // 
            checkBox6.AutoSize = true;
            checkBox6.Location = new Point(28, 309);
            checkBox6.Margin = new Padding(4, 3, 4, 3);
            checkBox6.Name = "checkBox6";
            checkBox6.Size = new Size(165, 29);
            checkBox6.TabIndex = 21;
            checkBox6.Text = "720p以下に制限";
            checkBox6.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.Location = new Point(240, 179);
            button4.Margin = new Padding(4, 3, 4, 3);
            button4.Name = "button4";
            button4.Size = new Size(87, 37);
            button4.TabIndex = 22;
            button4.Text = "Open";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Location = new Point(1286, 617);
            button5.Margin = new Padding(4, 3, 4, 3);
            button5.Name = "button5";
            button5.Size = new Size(154, 37);
            button5.TabIndex = 23;
            button5.Text = "1 movie DL";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(14, 126);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(87, 25);
            label6.TabIndex = 25;
            label6.Text = "DL Folder";
            // 
            // textBoxDLFolder
            // 
            textBoxDLFolder.Location = new Point(104, 123);
            textBoxDLFolder.Margin = new Padding(4, 3, 4, 3);
            textBoxDLFolder.Name = "textBoxDLFolder";
            textBoxDLFolder.Size = new Size(834, 31);
            textBoxDLFolder.TabIndex = 24;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(32, 182);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(182, 33);
            comboBox1.TabIndex = 26;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged_1;
            // 
            // textBoxConfigFile
            // 
            textBoxConfigFile.Location = new Point(14, 655);
            textBoxConfigFile.Multiline = true;
            textBoxConfigFile.Name = "textBoxConfigFile";
            textBoxConfigFile.Size = new Size(636, 278);
            textBoxConfigFile.TabIndex = 27;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(14, 627);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(96, 25);
            label7.TabIndex = 28;
            label7.Text = "yt-dlp設定";
            // 
            // checkBox7
            // 
            checkBox7.AutoSize = true;
            checkBox7.Location = new Point(382, 238);
            checkBox7.Margin = new Padding(4, 3, 4, 3);
            checkBox7.Name = "checkBox7";
            checkBox7.Size = new Size(22, 21);
            checkBox7.TabIndex = 30;
            checkBox7.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(412, 234);
            textBox1.Margin = new Padding(4, 3, 4, 3);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(389, 31);
            textBox1.TabIndex = 29;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(28, 234);
            label8.Margin = new Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new Size(286, 25);
            label8.TabIndex = 31;
            label8.Text = "DLファイル名の最初に次の文字を付加";
            // 
            // buttonUpdate
            // 
            buttonUpdate.Location = new Point(1224, 22);
            buttonUpdate.Name = "buttonUpdate";
            buttonUpdate.Size = new Size(124, 34);
            buttonUpdate.TabIndex = 32;
            buttonUpdate.Text = "Update";
            buttonUpdate.UseVisualStyleBackColor = true;
            buttonUpdate.Click += buttonUpdate_Click;
            // 
            // textBoxConsole
            // 
            textBoxConsole.Location = new Point(666, 655);
            textBoxConsole.Multiline = true;
            textBoxConsole.Name = "textBoxConsole";
            textBoxConsole.ScrollBars = ScrollBars.Vertical;
            textBoxConsole.Size = new Size(636, 278);
            textBoxConsole.TabIndex = 33;
            // 
            // Form1
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1457, 955);
            Controls.Add(textBoxConsole);
            Controls.Add(buttonUpdate);
            Controls.Add(label8);
            Controls.Add(checkBox7);
            Controls.Add(textBox1);
            Controls.Add(label7);
            Controls.Add(textBoxConfigFile);
            Controls.Add(comboBox1);
            Controls.Add(label6);
            Controls.Add(textBoxDLFolder);
            Controls.Add(button5);
            Controls.Add(button4);
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
            Controls.Add(buttonStart);
            Controls.Add(listBox1);
            Controls.Add(label2);
            Controls.Add(textBoxUrlFile);
            Controls.Add(label1);
            Controls.Add(textBoxExePath);
            Margin = new Padding(4, 3, 4, 3);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBoxExePath;
        private Label label1;
        private TextBox textBoxUrlFile;
        private Label label2;
        private ListBox listBox1;
        private Button buttonStart;
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
        private Button button4;
        private Button button5;
        private Label label6;
        private TextBox textBoxDLFolder;
        private ComboBox comboBox1;
        private TextBox textBoxConfigFile;
        private Label label7;
        private CheckBox checkBox7;
        private TextBox textBox1;
        private Label label8;
        private Button buttonUpdate;
        private TextBox textBoxConsole;
    }
}