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
            button1 = new Button();
            Safix = new TextBox();
            label3 = new Label();
            Dir = new TextBox();
            label4 = new Label();
            MoveComp = new CheckBox();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            PauseCMD = new CheckBox();
            groupBox1 = new GroupBox();
            radioButton4 = new RadioButton();
            radioButton3 = new RadioButton();
            radioButton2 = new RadioButton();
            radioButton1 = new RadioButton();
            label5 = new Label();
            PresetName = new TextBox();
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
            listBox1.Location = new Point(0, 237);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(1208, 164);
            listBox1.TabIndex = 4;
            // 
            // button1
            // 
            button1.Location = new Point(937, 199);
            button1.Name = "button1";
            button1.Size = new Size(256, 29);
            button1.TabIndex = 5;
            button1.Text = "Start";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
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
            // MoveComp
            // 
            MoveComp.AutoSize = true;
            MoveComp.Location = new Point(466, 139);
            MoveComp.Name = "MoveComp";
            MoveComp.Size = new Size(210, 24);
            MoveComp.TabIndex = 10;
            MoveComp.Text = "処理済み元ファイルを移動する";
            MoveComp.UseVisualStyleBackColor = true;
            MoveComp.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // button2
            // 
            button2.Location = new Point(12, 200);
            button2.Name = "button2";
            button2.Size = new Size(115, 29);
            button2.TabIndex = 13;
            button2.Text = "AddInputDir";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(787, 14);
            button3.Name = "button3";
            button3.Size = new Size(115, 29);
            button3.TabIndex = 14;
            button3.Text = "Open";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(787, 167);
            button4.Name = "button4";
            button4.Size = new Size(115, 29);
            button4.TabIndex = 15;
            button4.Text = "Save";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // PauseCMD
            // 
            PauseCMD.AutoSize = true;
            PauseCMD.Location = new Point(700, 137);
            PauseCMD.Name = "PauseCMD";
            PauseCMD.Size = new Size(202, 24);
            PauseCMD.TabIndex = 16;
            PauseCMD.Text = "コマンドを処理後に一時停止";
            PauseCMD.UseVisualStyleBackColor = true;
            PauseCMD.CheckedChanged += checkBox2_CheckedChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(radioButton4);
            groupBox1.Controls.Add(radioButton3);
            groupBox1.Controls.Add(radioButton2);
            groupBox1.Controls.Add(radioButton1);
            groupBox1.Location = new Point(937, 16);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(256, 160);
            groupBox1.TabIndex = 17;
            groupBox1.TabStop = false;
            groupBox1.Text = "Preset";
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
            // Form1
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1208, 401);
            Controls.Add(PresetName);
            Controls.Add(label5);
            Controls.Add(groupBox1);
            Controls.Add(PauseCMD);
            Controls.Add(button3);
            Controls.Add(button4);
            Controls.Add(button2);
            Controls.Add(MoveComp);
            Controls.Add(label4);
            Controls.Add(Dir);
            Controls.Add(label3);
            Controls.Add(Safix);
            Controls.Add(button1);
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
        private Button button1;
        private TextBox Safix;
        private Label label3;
        private TextBox Dir;
        private Label label4;
        private CheckBox MoveComp;
        private Button button2;
        private Button button3;
        private Button button4;
        private CheckBox PauseCMD;
        private GroupBox groupBox1;
        private RadioButton radioButton4;
        private RadioButton radioButton3;
        private RadioButton radioButton2;
        private RadioButton radioButton1;
        private Label label5;
        private TextBox PresetName;
    }
}