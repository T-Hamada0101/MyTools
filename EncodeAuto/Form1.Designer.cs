﻿namespace EncodeAuto
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
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // BatPath
            // 
            BatPath.Location = new Point(113, 50);
            BatPath.Name = "BatPath";
            BatPath.Size = new Size(794, 27);
            BatPath.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(14, 60);
            label1.Name = "label1";
            label1.Size = new Size(53, 20);
            label1.TabIndex = 1;
            label1.Text = "＊.BAT";
            // 
            // Arguments
            // 
            Arguments.Location = new Point(170, 95);
            Arguments.Name = "Arguments";
            Arguments.Size = new Size(737, 27);
            Arguments.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 98);
            label2.Name = "label2";
            label2.Size = new Size(154, 20);
            label2.TabIndex = 3;
            label2.Text = "Command Arguments";
            // 
            // listBox1
            // 
            listBox1.Dock = DockStyle.Bottom;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 20;
            listBox1.Location = new Point(0, 251);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(1338, 164);
            listBox1.TabIndex = 4;
            // 
            // BT_start
            // 
            BT_start.Location = new Point(937, 199);
            BT_start.Name = "BT_start";
            BT_start.Size = new Size(256, 29);
            BT_start.TabIndex = 5;
            BT_start.Text = "Start";
            BT_start.UseVisualStyleBackColor = true;
            BT_start.Click += BT_start_Click;
            // 
            // Safix
            // 
            Safix.Location = new Point(113, 137);
            Safix.Name = "Safix";
            Safix.Size = new Size(121, 27);
            Safix.TabIndex = 6;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(18, 140);
            label3.Name = "label3";
            label3.Size = new Size(40, 20);
            label3.TabIndex = 7;
            label3.Text = "Safix";
            // 
            // Dir
            // 
            Dir.Location = new Point(513, 14);
            Dir.Name = "Dir";
            Dir.Size = new Size(261, 27);
            Dir.TabIndex = 8;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(403, 19);
            label4.Name = "label4";
            label4.Size = new Size(104, 20);
            label4.TabIndex = 9;
            label4.Text = "EndodeOutDir";
            // 
            // CK_MoveComp
            // 
            CK_MoveComp.AutoSize = true;
            CK_MoveComp.Location = new Point(259, 136);
            CK_MoveComp.Name = "CK_MoveComp";
            CK_MoveComp.Size = new Size(209, 24);
            CK_MoveComp.TabIndex = 10;
            CK_MoveComp.Text = "処理後に元ファイルを移動する";
            CK_MoveComp.UseVisualStyleBackColor = true;
            CK_MoveComp.CheckedChanged += CK_MoveComp_CheckedChanged;
            // 
            // BT_addInputDir
            // 
            BT_addInputDir.Location = new Point(12, 200);
            BT_addInputDir.Name = "BT_addInputDir";
            BT_addInputDir.Size = new Size(115, 29);
            BT_addInputDir.TabIndex = 13;
            BT_addInputDir.Text = "AddInputDir";
            BT_addInputDir.UseVisualStyleBackColor = true;
            BT_addInputDir.Click += BT_addInputDir_Click;
            // 
            // BT_open
            // 
            BT_open.Location = new Point(787, 14);
            BT_open.Name = "BT_open";
            BT_open.Size = new Size(115, 29);
            BT_open.TabIndex = 14;
            BT_open.Text = "Open";
            BT_open.UseVisualStyleBackColor = true;
            BT_open.Click += BT_open_Click;
            // 
            // BT_Save
            // 
            BT_Save.Location = new Point(787, 167);
            BT_Save.Name = "BT_Save";
            BT_Save.Size = new Size(115, 29);
            BT_Save.TabIndex = 15;
            BT_Save.Text = "Save";
            BT_Save.UseVisualStyleBackColor = true;
            BT_Save.Click += BT_Save_Click;
            // 
            // CK_PauseCMD
            // 
            CK_PauseCMD.AutoSize = true;
            CK_PauseCMD.Location = new Point(700, 137);
            CK_PauseCMD.Name = "CK_PauseCMD";
            CK_PauseCMD.Size = new Size(202, 24);
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
            groupBox1.Location = new Point(937, 16);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(390, 160);
            groupBox1.TabIndex = 17;
            groupBox1.TabStop = false;
            groupBox1.Text = "Preset";
            // 
            // radioButton9
            // 
            radioButton9.AutoSize = true;
            radioButton9.Location = new Point(257, 37);
            radioButton9.Name = "radioButton9";
            radioButton9.Size = new Size(38, 24);
            radioButton9.TabIndex = 27;
            radioButton9.TabStop = true;
            radioButton9.Text = "9";
            radioButton9.UseVisualStyleBackColor = true;
            radioButton9.CheckedChanged += radioButton9_CheckedChanged;
            // 
            // radioButton10
            // 
            radioButton10.AutoSize = true;
            radioButton10.Location = new Point(257, 66);
            radioButton10.Name = "radioButton10";
            radioButton10.Size = new Size(46, 24);
            radioButton10.TabIndex = 26;
            radioButton10.TabStop = true;
            radioButton10.Text = "10";
            radioButton10.UseVisualStyleBackColor = true;
            radioButton10.CheckedChanged += radioButton10_CheckedChanged;
            // 
            // radioButton11
            // 
            radioButton11.AutoSize = true;
            radioButton11.Location = new Point(257, 94);
            radioButton11.Name = "radioButton11";
            radioButton11.Size = new Size(46, 24);
            radioButton11.TabIndex = 25;
            radioButton11.TabStop = true;
            radioButton11.Text = "11";
            radioButton11.UseVisualStyleBackColor = true;
            radioButton11.CheckedChanged += radioButton11_CheckedChanged;
            // 
            // radioButton12
            // 
            radioButton12.AutoSize = true;
            radioButton12.Location = new Point(257, 124);
            radioButton12.Name = "radioButton12";
            radioButton12.Size = new Size(46, 24);
            radioButton12.TabIndex = 24;
            radioButton12.TabStop = true;
            radioButton12.Text = "12";
            radioButton12.UseVisualStyleBackColor = true;
            radioButton12.CheckedChanged += radioButton12_CheckedChanged;
            // 
            // radioButton5
            // 
            radioButton5.AutoSize = true;
            radioButton5.Location = new Point(141, 34);
            radioButton5.Name = "radioButton5";
            radioButton5.Size = new Size(38, 24);
            radioButton5.TabIndex = 23;
            radioButton5.TabStop = true;
            radioButton5.Text = "5";
            radioButton5.UseVisualStyleBackColor = true;
            radioButton5.CheckedChanged += radioButton5_CheckedChanged;
            // 
            // radioButton6
            // 
            radioButton6.AutoSize = true;
            radioButton6.Location = new Point(141, 63);
            radioButton6.Name = "radioButton6";
            radioButton6.Size = new Size(38, 24);
            radioButton6.TabIndex = 22;
            radioButton6.TabStop = true;
            radioButton6.Text = "6";
            radioButton6.UseVisualStyleBackColor = true;
            radioButton6.CheckedChanged += radioButton6_CheckedChanged;
            // 
            // radioButton7
            // 
            radioButton7.AutoSize = true;
            radioButton7.Location = new Point(141, 91);
            radioButton7.Name = "radioButton7";
            radioButton7.Size = new Size(38, 24);
            radioButton7.TabIndex = 21;
            radioButton7.TabStop = true;
            radioButton7.Text = "7";
            radioButton7.UseVisualStyleBackColor = true;
            radioButton7.CheckedChanged += radioButton7_CheckedChanged;
            // 
            // radioButton8
            // 
            radioButton8.AutoSize = true;
            radioButton8.Location = new Point(141, 121);
            radioButton8.Name = "radioButton8";
            radioButton8.Size = new Size(38, 24);
            radioButton8.TabIndex = 20;
            radioButton8.TabStop = true;
            radioButton8.Text = "8";
            radioButton8.UseVisualStyleBackColor = true;
            radioButton8.CheckedChanged += radioButton8_CheckedChanged;
            // 
            // radioButton4
            // 
            radioButton4.AutoSize = true;
            radioButton4.Location = new Point(23, 123);
            radioButton4.Name = "radioButton4";
            radioButton4.Size = new Size(38, 24);
            radioButton4.TabIndex = 19;
            radioButton4.TabStop = true;
            radioButton4.Text = "4";
            radioButton4.UseVisualStyleBackColor = true;
            radioButton4.CheckedChanged += radioButton4_CheckedChanged;
            // 
            // radioButton3
            // 
            radioButton3.AutoSize = true;
            radioButton3.Location = new Point(23, 93);
            radioButton3.Name = "radioButton3";
            radioButton3.Size = new Size(38, 24);
            radioButton3.TabIndex = 18;
            radioButton3.TabStop = true;
            radioButton3.Text = "3";
            radioButton3.UseVisualStyleBackColor = true;
            radioButton3.CheckedChanged += radioButton3_CheckedChanged;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new Point(23, 63);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(38, 24);
            radioButton2.TabIndex = 17;
            radioButton2.TabStop = true;
            radioButton2.Text = "2";
            radioButton2.UseVisualStyleBackColor = true;
            radioButton2.CheckedChanged += radioButton2_CheckedChanged;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new Point(23, 34);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(38, 24);
            radioButton1.TabIndex = 16;
            radioButton1.TabStop = true;
            radioButton1.Text = "1";
            radioButton1.UseVisualStyleBackColor = true;
            radioButton1.CheckedChanged += radioButton1_CheckedChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(18, 16);
            label5.Name = "label5";
            label5.Size = new Size(89, 20);
            label5.TabIndex = 18;
            label5.Text = "PresetName";
            // 
            // PresetName
            // 
            PresetName.Location = new Point(113, 12);
            PresetName.Name = "PresetName";
            PresetName.Size = new Size(158, 27);
            PresetName.TabIndex = 19;
            // 
            // CK_OutSameDir
            // 
            CK_OutSameDir.AutoSize = true;
            CK_OutSameDir.Location = new Point(475, 136);
            CK_OutSameDir.Name = "CK_OutSameDir";
            CK_OutSameDir.Size = new Size(176, 24);
            CK_OutSameDir.TabIndex = 20;
            CK_OutSameDir.Text = "元ファイルと同Dirに出力";
            CK_OutSameDir.UseVisualStyleBackColor = true;
            CK_OutSameDir.CheckedChanged += CK_OutSameDir_CheckedChanged;
            // 
            // Form1
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1338, 415);
            Controls.Add(CK_OutSameDir);
            Controls.Add(PresetName);
            Controls.Add(label5);
            Controls.Add(groupBox1);
            Controls.Add(CK_PauseCMD);
            Controls.Add(BT_open);
            Controls.Add(BT_Save);
            Controls.Add(BT_addInputDir);
            Controls.Add(CK_MoveComp);
            Controls.Add(label4);
            Controls.Add(Dir);
            Controls.Add(label3);
            Controls.Add(Safix);
            Controls.Add(BT_start);
            Controls.Add(listBox1);
            Controls.Add(label2);
            Controls.Add(Arguments);
            Controls.Add(label1);
            Controls.Add(BatPath);
            Name = "Form1";
            Text = "Form1";
            FormClosing += Form1_FormClosing;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
    }
}