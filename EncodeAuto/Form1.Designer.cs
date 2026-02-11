namespace EncodeAuto
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
            BatPath = new TextBox();
            label1 = new Label();
            Arguments = new TextBox();
            label2 = new Label();
            listBox1 = new ListBox();
            BT_start = new Button();
            Safix = new TextBox();
            label3 = new Label();
            Dir = new TextBox();
            label4 = new Label();
            CK_MoveComp = new CheckBox();
            BT_addInputDir = new Button();
            BT_open = new Button();
            BT_Save = new Button();
            CK_PauseCMD = new CheckBox();
            groupBox1 = new GroupBox();
            radioButton9 = new RadioButton();
            radioButton10 = new RadioButton();
            radioButton11 = new RadioButton();
            radioButton12 = new RadioButton();
            radioButton5 = new RadioButton();
            radioButton6 = new RadioButton();
            radioButton7 = new RadioButton();
            radioButton8 = new RadioButton();
            radioButton4 = new RadioButton();
            radioButton3 = new RadioButton();
            radioButton2 = new RadioButton();
            radioButton1 = new RadioButton();
            label5 = new Label();
            PresetName = new TextBox();
            CK_OutSameDir = new CheckBox();
            numericUpDown1 = new NumericUpDown();
            label6 = new Label();
            panel1 = new Panel();
            listBoxMerge = new ListBox();
            button2 = new Button();
            CK_ShortFileName = new CheckBox();
            button1 = new Button();
            textBox1 = new TextBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            textBoxPrefix = new TextBox();
            label7 = new Label();
            groupBox2 = new GroupBox();
            label8 = new Label();
            textBoxSafix = new TextBox();
            buttonFileName = new Button();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            panel1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // BatPath
            // 
            BatPath.Location = new Point(130, 52);
            BatPath.Margin = new Padding(4, 4, 4, 4);
            BatPath.Name = "BatPath";
            BatPath.Size = new Size(992, 31);
            BatPath.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 65);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(64, 25);
            label1.TabIndex = 1;
            label1.Text = "＊.BAT";
            // 
            // Arguments
            // 
            Arguments.Location = new Point(201, 109);
            Arguments.Margin = new Padding(4, 4, 4, 4);
            Arguments.Name = "Arguments";
            Arguments.Size = new Size(920, 31);
            Arguments.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 112);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(189, 25);
            label2.TabIndex = 3;
            label2.Text = "Command Arguments";
            // 
            // listBox1
            // 
            listBox1.Dock = DockStyle.Fill;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 25;
            listBox1.Location = new Point(4, 316);
            listBox1.Margin = new Padding(4, 4, 4, 4);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(2220, 494);
            listBox1.TabIndex = 4;
            // 
            // BT_start
            // 
            BT_start.Location = new Point(1418, 262);
            BT_start.Margin = new Padding(4, 4, 4, 4);
            BT_start.Name = "BT_start";
            BT_start.Size = new Size(230, 36);
            BT_start.TabIndex = 5;
            BT_start.Text = "Start";
            BT_start.UseVisualStyleBackColor = true;
            BT_start.Click += BT_start_Click;
            // 
            // Safix
            // 
            Safix.Location = new Point(69, 162);
            Safix.Margin = new Padding(4, 4, 4, 4);
            Safix.Name = "Safix";
            Safix.Size = new Size(150, 31);
            Safix.TabIndex = 6;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(11, 165);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(49, 25);
            label3.TabIndex = 7;
            label3.Text = "Safix";
            // 
            // Dir
            // 
            Dir.Location = new Point(630, 8);
            Dir.Margin = new Padding(4, 4, 4, 4);
            Dir.Name = "Dir";
            Dir.Size = new Size(325, 31);
            Dir.TabIndex = 8;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(492, 14);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(126, 25);
            label4.TabIndex = 9;
            label4.Text = "EndodeOutDir";
            // 
            // CK_MoveComp
            // 
            CK_MoveComp.AutoSize = true;
            CK_MoveComp.Location = new Point(241, 166);
            CK_MoveComp.Margin = new Padding(4, 4, 4, 4);
            CK_MoveComp.Name = "CK_MoveComp";
            CK_MoveComp.Size = new Size(254, 29);
            CK_MoveComp.TabIndex = 10;
            CK_MoveComp.Text = "処理後に元ファイルを移動する";
            CK_MoveComp.UseVisualStyleBackColor = true;
            CK_MoveComp.CheckedChanged += CK_MoveComp_CheckedChanged;
            // 
            // BT_addInputDir
            // 
            BT_addInputDir.Location = new Point(4, 262);
            BT_addInputDir.Margin = new Padding(4, 4, 4, 4);
            BT_addInputDir.Name = "BT_addInputDir";
            BT_addInputDir.Size = new Size(144, 36);
            BT_addInputDir.TabIndex = 13;
            BT_addInputDir.Text = "AddInputDir";
            BT_addInputDir.UseVisualStyleBackColor = true;
            BT_addInputDir.Click += BT_addInputDir_Click;
            // 
            // BT_open
            // 
            BT_open.Location = new Point(972, 8);
            BT_open.Margin = new Padding(4, 4, 4, 4);
            BT_open.Name = "BT_open";
            BT_open.Size = new Size(144, 36);
            BT_open.TabIndex = 14;
            BT_open.Text = "Open";
            BT_open.UseVisualStyleBackColor = true;
            BT_open.Click += BT_open_Click;
            // 
            // BT_Save
            // 
            BT_Save.Location = new Point(1009, 162);
            BT_Save.Margin = new Padding(4, 4, 4, 4);
            BT_Save.Name = "BT_Save";
            BT_Save.Size = new Size(144, 36);
            BT_Save.TabIndex = 15;
            BT_Save.Text = "Save";
            BT_Save.UseVisualStyleBackColor = true;
            BT_Save.Click += BT_Save_Click;
            // 
            // CK_PauseCMD
            // 
            CK_PauseCMD.AutoSize = true;
            CK_PauseCMD.Location = new Point(739, 166);
            CK_PauseCMD.Margin = new Padding(4, 4, 4, 4);
            CK_PauseCMD.Name = "CK_PauseCMD";
            CK_PauseCMD.Size = new Size(245, 29);
            CK_PauseCMD.TabIndex = 16;
            CK_PauseCMD.Text = "コマンドを処理後に一時停止";
            CK_PauseCMD.UseVisualStyleBackColor = true;
            CK_PauseCMD.CheckedChanged += CK_PauseCMD_CheckedChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(radioButton9);
            groupBox1.Controls.Add(radioButton10);
            groupBox1.Controls.Add(radioButton11);
            groupBox1.Controls.Add(radioButton12);
            groupBox1.Controls.Add(radioButton5);
            groupBox1.Controls.Add(radioButton6);
            groupBox1.Controls.Add(radioButton7);
            groupBox1.Controls.Add(radioButton8);
            groupBox1.Controls.Add(radioButton4);
            groupBox1.Controls.Add(radioButton3);
            groupBox1.Controls.Add(radioButton2);
            groupBox1.Controls.Add(radioButton1);
            groupBox1.Location = new Point(1160, 10);
            groupBox1.Margin = new Padding(4, 4, 4, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4, 4, 4, 4);
            groupBox1.Size = new Size(488, 200);
            groupBox1.TabIndex = 17;
            groupBox1.TabStop = false;
            groupBox1.Text = "Preset";
            // 
            // radioButton9
            // 
            radioButton9.AutoSize = true;
            radioButton9.Location = new Point(321, 46);
            radioButton9.Margin = new Padding(4, 4, 4, 4);
            radioButton9.Name = "radioButton9";
            radioButton9.Size = new Size(47, 29);
            radioButton9.TabIndex = 27;
            radioButton9.TabStop = true;
            radioButton9.Text = "9";
            radioButton9.UseVisualStyleBackColor = true;
            radioButton9.CheckedChanged += radioButton9_CheckedChanged;
            // 
            // radioButton10
            // 
            radioButton10.AutoSize = true;
            radioButton10.Location = new Point(321, 82);
            radioButton10.Margin = new Padding(4, 4, 4, 4);
            radioButton10.Name = "radioButton10";
            radioButton10.Size = new Size(57, 29);
            radioButton10.TabIndex = 26;
            radioButton10.TabStop = true;
            radioButton10.Text = "10";
            radioButton10.UseVisualStyleBackColor = true;
            radioButton10.CheckedChanged += radioButton10_CheckedChanged;
            // 
            // radioButton11
            // 
            radioButton11.AutoSize = true;
            radioButton11.Location = new Point(321, 118);
            radioButton11.Margin = new Padding(4, 4, 4, 4);
            radioButton11.Name = "radioButton11";
            radioButton11.Size = new Size(57, 29);
            radioButton11.TabIndex = 25;
            radioButton11.TabStop = true;
            radioButton11.Text = "11";
            radioButton11.UseVisualStyleBackColor = true;
            radioButton11.CheckedChanged += radioButton11_CheckedChanged;
            // 
            // radioButton12
            // 
            radioButton12.AutoSize = true;
            radioButton12.Location = new Point(321, 155);
            radioButton12.Margin = new Padding(4, 4, 4, 4);
            radioButton12.Name = "radioButton12";
            radioButton12.Size = new Size(57, 29);
            radioButton12.TabIndex = 24;
            radioButton12.TabStop = true;
            radioButton12.Text = "12";
            radioButton12.UseVisualStyleBackColor = true;
            radioButton12.CheckedChanged += radioButton12_CheckedChanged;
            // 
            // radioButton5
            // 
            radioButton5.AutoSize = true;
            radioButton5.Location = new Point(176, 42);
            radioButton5.Margin = new Padding(4, 4, 4, 4);
            radioButton5.Name = "radioButton5";
            radioButton5.Size = new Size(47, 29);
            radioButton5.TabIndex = 23;
            radioButton5.TabStop = true;
            radioButton5.Text = "5";
            radioButton5.UseVisualStyleBackColor = true;
            radioButton5.CheckedChanged += radioButton5_CheckedChanged;
            // 
            // radioButton6
            // 
            radioButton6.AutoSize = true;
            radioButton6.Location = new Point(176, 79);
            radioButton6.Margin = new Padding(4, 4, 4, 4);
            radioButton6.Name = "radioButton6";
            radioButton6.Size = new Size(47, 29);
            radioButton6.TabIndex = 22;
            radioButton6.TabStop = true;
            radioButton6.Text = "6";
            radioButton6.UseVisualStyleBackColor = true;
            radioButton6.CheckedChanged += radioButton6_CheckedChanged;
            // 
            // radioButton7
            // 
            radioButton7.AutoSize = true;
            radioButton7.Location = new Point(176, 114);
            radioButton7.Margin = new Padding(4, 4, 4, 4);
            radioButton7.Name = "radioButton7";
            radioButton7.Size = new Size(47, 29);
            radioButton7.TabIndex = 21;
            radioButton7.TabStop = true;
            radioButton7.Text = "7";
            radioButton7.UseVisualStyleBackColor = true;
            radioButton7.CheckedChanged += radioButton7_CheckedChanged;
            // 
            // radioButton8
            // 
            radioButton8.AutoSize = true;
            radioButton8.Location = new Point(176, 151);
            radioButton8.Margin = new Padding(4, 4, 4, 4);
            radioButton8.Name = "radioButton8";
            radioButton8.Size = new Size(47, 29);
            radioButton8.TabIndex = 20;
            radioButton8.TabStop = true;
            radioButton8.Text = "8";
            radioButton8.UseVisualStyleBackColor = true;
            radioButton8.CheckedChanged += radioButton8_CheckedChanged;
            // 
            // radioButton4
            // 
            radioButton4.AutoSize = true;
            radioButton4.Location = new Point(29, 154);
            radioButton4.Margin = new Padding(4, 4, 4, 4);
            radioButton4.Name = "radioButton4";
            radioButton4.Size = new Size(47, 29);
            radioButton4.TabIndex = 19;
            radioButton4.TabStop = true;
            radioButton4.Text = "4";
            radioButton4.UseVisualStyleBackColor = true;
            radioButton4.CheckedChanged += radioButton4_CheckedChanged;
            // 
            // radioButton3
            // 
            radioButton3.AutoSize = true;
            radioButton3.Location = new Point(29, 116);
            radioButton3.Margin = new Padding(4, 4, 4, 4);
            radioButton3.Name = "radioButton3";
            radioButton3.Size = new Size(47, 29);
            radioButton3.TabIndex = 18;
            radioButton3.TabStop = true;
            radioButton3.Text = "3";
            radioButton3.UseVisualStyleBackColor = true;
            radioButton3.CheckedChanged += radioButton3_CheckedChanged;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new Point(29, 79);
            radioButton2.Margin = new Padding(4, 4, 4, 4);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(47, 29);
            radioButton2.TabIndex = 17;
            radioButton2.TabStop = true;
            radioButton2.Text = "2";
            radioButton2.UseVisualStyleBackColor = true;
            radioButton2.CheckedChanged += radioButton2_CheckedChanged;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new Point(29, 42);
            radioButton1.Margin = new Padding(4, 4, 4, 4);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(47, 29);
            radioButton1.TabIndex = 16;
            radioButton1.TabStop = true;
            radioButton1.Text = "1";
            radioButton1.UseVisualStyleBackColor = true;
            radioButton1.CheckedChanged += radioButton1_CheckedChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(11, 10);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(107, 25);
            label5.TabIndex = 18;
            label5.Text = "PresetName";
            // 
            // PresetName
            // 
            PresetName.Location = new Point(130, 5);
            PresetName.Margin = new Padding(4, 4, 4, 4);
            PresetName.Name = "PresetName";
            PresetName.Size = new Size(196, 31);
            PresetName.TabIndex = 19;
            // 
            // CK_OutSameDir
            // 
            CK_OutSameDir.AutoSize = true;
            CK_OutSameDir.Location = new Point(511, 166);
            CK_OutSameDir.Margin = new Padding(4, 4, 4, 4);
            CK_OutSameDir.Name = "CK_OutSameDir";
            CK_OutSameDir.Size = new Size(211, 29);
            CK_OutSameDir.TabIndex = 20;
            CK_OutSameDir.Text = "元ファイルと同Dirに出力";
            CK_OutSameDir.UseVisualStyleBackColor = true;
            CK_OutSameDir.CheckedChanged += CK_OutSameDir_CheckedChanged;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(1299, 265);
            numericUpDown1.Margin = new Padding(4, 4, 4, 4);
            numericUpDown1.Maximum = new decimal(new int[] { 5, 0, 0, 0 });
            numericUpDown1.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(85, 31);
            numericUpDown1.TabIndex = 21;
            numericUpDown1.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(1176, 270);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(113, 25);
            label6.TabIndex = 22;
            label6.Text = "MaxSessions";
            // 
            // panel1
            // 
            panel1.Controls.Add(groupBox2);
            panel1.Controls.Add(listBoxMerge);
            panel1.Controls.Add(button2);
            panel1.Controls.Add(CK_ShortFileName);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(textBox1);
            panel1.Controls.Add(PresetName);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(BatPath);
            panel1.Controls.Add(numericUpDown1);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(CK_OutSameDir);
            panel1.Controls.Add(Arguments);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(BT_start);
            panel1.Controls.Add(groupBox1);
            panel1.Controls.Add(Safix);
            panel1.Controls.Add(CK_PauseCMD);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(BT_open);
            panel1.Controls.Add(Dir);
            panel1.Controls.Add(BT_Save);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(BT_addInputDir);
            panel1.Controls.Add(CK_MoveComp);
            panel1.Location = new Point(4, 4);
            panel1.Margin = new Padding(4, 4, 4, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(2220, 304);
            panel1.TabIndex = 23;
            // 
            // listBoxMerge
            // 
            listBoxMerge.FormattingEnabled = true;
            listBoxMerge.HorizontalScrollbar = true;
            listBoxMerge.ItemHeight = 25;
            listBoxMerge.Location = new Point(2124, 8);
            listBoxMerge.Margin = new Padding(4, 4, 4, 4);
            listBoxMerge.Name = "listBoxMerge";
            listBoxMerge.Size = new Size(78, 204);
            listBoxMerge.TabIndex = 28;
            // 
            // button2
            // 
            button2.Location = new Point(2061, 242);
            button2.Margin = new Padding(4, 4, 4, 4);
            button2.Name = "button2";
            button2.Size = new Size(118, 36);
            button2.TabIndex = 27;
            button2.Text = "merge";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // CK_ShortFileName
            // 
            CK_ShortFileName.AutoSize = true;
            CK_ShortFileName.Location = new Point(241, 220);
            CK_ShortFileName.Margin = new Padding(4, 4, 4, 4);
            CK_ShortFileName.Name = "CK_ShortFileName";
            CK_ShortFileName.Size = new Size(219, 29);
            CK_ShortFileName.TabIndex = 25;
            CK_ShortFileName.Text = "長いファイル名を省略する";
            CK_ShortFileName.UseVisualStyleBackColor = true;
            CK_ShortFileName.CheckedChanged += CK_ShortFileName_CheckedChanged;
            // 
            // button1
            // 
            button1.Location = new Point(1916, 242);
            button1.Margin = new Padding(4, 4, 4, 4);
            button1.Name = "button1";
            button1.Size = new Size(118, 36);
            button1.TabIndex = 24;
            button1.Text = "cut";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Yu Gothic UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            textBox1.Location = new Point(1997, 14);
            textBox1.Margin = new Padding(4, 4, 4, 4);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(119, 206);
            textBox1.TabIndex = 23;
            textBox1.Text = "02:36:20\r\n05:08:35\r\n07:42:50\r\n00:00:00";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(listBox1, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(4, 4, 4, 4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 312F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 476F));
            tableLayoutPanel1.Size = new Size(2228, 814);
            tableLayoutPanel1.TabIndex = 25;
            // 
            // textBoxPrefix
            // 
            textBoxPrefix.Location = new Point(94, 42);
            textBoxPrefix.Margin = new Padding(4);
            textBoxPrefix.Name = "textBoxPrefix";
            textBoxPrefix.Size = new Size(150, 31);
            textBoxPrefix.TabIndex = 29;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(36, 45);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(56, 25);
            label7.TabIndex = 30;
            label7.Text = "prefix";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(buttonFileName);
            groupBox2.Controls.Add(label8);
            groupBox2.Controls.Add(textBoxSafix);
            groupBox2.Controls.Add(label7);
            groupBox2.Controls.Add(textBoxPrefix);
            groupBox2.Location = new Point(1662, 20);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(300, 215);
            groupBox2.TabIndex = 31;
            groupBox2.TabStop = false;
            groupBox2.Text = "ファイル名変更のみ";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(36, 92);
            label8.Margin = new Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new Size(49, 25);
            label8.TabIndex = 32;
            label8.Text = "Safix";
            // 
            // textBoxSafix
            // 
            textBoxSafix.Location = new Point(94, 89);
            textBoxSafix.Margin = new Padding(4);
            textBoxSafix.Name = "textBoxSafix";
            textBoxSafix.Size = new Size(150, 31);
            textBoxSafix.TabIndex = 31;
            // 
            // buttonFileName
            // 
            buttonFileName.Location = new Point(36, 146);
            buttonFileName.Margin = new Padding(4);
            buttonFileName.Name = "buttonFileName";
            buttonFileName.Size = new Size(230, 36);
            buttonFileName.TabIndex = 32;
            buttonFileName.Text = "Start";
            buttonFileName.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(2228, 814);
            Controls.Add(tableLayoutPanel1);
            Margin = new Padding(4, 4, 4, 4);
            Name = "Form1";
            Text = "Form1";
            FormClosing += Form1_FormClosing;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TextBox BatPath;
        private Label label1;
        private TextBox Arguments;
        private Label label2;
        private ListBox listBox1;
        private Button BT_start;
        private TextBox Safix;
        private Label label3;
        private TextBox Dir;
        private Label label4;
        private CheckBox CK_MoveComp;
        private Button BT_addInputDir;
        private Button BT_open;
        private Button BT_Save;
        private CheckBox CK_PauseCMD;
        private GroupBox groupBox1;
        private RadioButton radioButton4;
        private RadioButton radioButton3;
        private RadioButton radioButton2;
        private RadioButton radioButton1;
        private Label label5;
        private TextBox PresetName;
        private RadioButton radioButton5;
        private RadioButton radioButton6;
        private RadioButton radioButton7;
        private RadioButton radioButton8;
        private CheckBox CK_OutSameDir;
        private RadioButton radioButton9;
        private RadioButton radioButton10;
        private RadioButton radioButton11;
        private RadioButton radioButton12;
        private NumericUpDown numericUpDown1;
        private Label label6;
        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel1;
        private Button button1;
        private TextBox textBox1;
        private CheckBox CK_ShortFileName;
        private Button button2;
        private ListBox listBoxMerge;
        private GroupBox groupBox2;
        private Button buttonFileName;
        private Label label8;
        private TextBox textBoxSafix;
        private Label label7;
        private TextBox textBoxPrefix;
    }
}