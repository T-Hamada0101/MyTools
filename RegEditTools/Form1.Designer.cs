namespace RegEditTools
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.grpActions = new System.Windows.Forms.GroupBox();
            this.btnOpenRegedit = new System.Windows.Forms.Button();
            this.btnRestore = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCheck = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.infoLabel = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.grpActions2 = new System.Windows.Forms.GroupBox();
            this.btnOpenRegedit2 = new System.Windows.Forms.Button();
            this.btnRestore2 = new System.Windows.Forms.Button();
            this.btnApply2 = new System.Windows.Forms.Button();
            this.btnCheck2 = new System.Windows.Forms.Button();
            this.txtLog2 = new System.Windows.Forms.TextBox();
            this.infoLabel2 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.grpActions.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.grpActions2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(800, 450);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.grpActions);
            this.tabPage1.Controls.Add(this.txtLog);
            this.tabPage1.Controls.Add(this.infoLabel);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(792, 417);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Win11 フォルダ種類";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // grpActions
            // 
            this.grpActions.Controls.Add(this.btnOpenRegedit);
            this.grpActions.Controls.Add(this.btnRestore);
            this.grpActions.Controls.Add(this.btnApply);
            this.grpActions.Controls.Add(this.btnCheck);
            this.grpActions.Location = new System.Drawing.Point(8, 260);
            this.grpActions.Name = "grpActions";
            this.grpActions.Size = new System.Drawing.Size(776, 100);
            this.grpActions.TabIndex = 2;
            this.grpActions.TabStop = false;
            this.grpActions.Text = "操作";
            // 
            // btnOpenRegedit
            // 
            this.btnOpenRegedit.Location = new System.Drawing.Point(582, 35);
            this.btnOpenRegedit.Name = "btnOpenRegedit";
            this.btnOpenRegedit.Size = new System.Drawing.Size(150, 40);
            this.btnOpenRegedit.TabIndex = 3;
            this.btnOpenRegedit.Text = "RegEditで開く";
            this.btnOpenRegedit.UseVisualStyleBackColor = true;
            this.btnOpenRegedit.Click += new System.EventHandler(this.btnOpenRegedit_Click);
            // 
            // btnRestore
            // 
            this.btnRestore.Location = new System.Drawing.Point(400, 35);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(150, 40);
            this.btnRestore.TabIndex = 2;
            this.btnRestore.Text = "元に戻す";
            this.btnRestore.UseVisualStyleBackColor = true;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(220, 35);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(150, 40);
            this.btnApply.TabIndex = 1;
            this.btnApply.Text = "修正を適用";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCheck
            // 
            this.btnCheck.Location = new System.Drawing.Point(45, 35);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(150, 40);
            this.btnCheck.TabIndex = 0;
            this.btnCheck.Text = "状態確認";
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(8, 60);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(776, 180);
            this.txtLog.TabIndex = 1;
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.Location = new System.Drawing.Point(8, 15);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(550, 20);
            this.infoLabel.TabIndex = 0;
            this.infoLabel.Text = "Windows 11でフォルダの種類が自動判定されるのを防ぐため、レジストリを設定します。\r\n";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.grpActions2);
            this.tabPage2.Controls.Add(this.txtLog2);
            this.tabPage2.Controls.Add(this.infoLabel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(792, 417);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "AppXSVC停止";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // grpActions2
            // 
            this.grpActions2.Controls.Add(this.btnOpenRegedit2);
            this.grpActions2.Controls.Add(this.btnRestore2);
            this.grpActions2.Controls.Add(this.btnApply2);
            this.grpActions2.Controls.Add(this.btnCheck2);
            this.grpActions2.Location = new System.Drawing.Point(8, 260);
            this.grpActions2.Name = "grpActions2";
            this.grpActions2.Size = new System.Drawing.Size(776, 100);
            this.grpActions2.TabIndex = 2;
            this.grpActions2.TabStop = false;
            this.grpActions2.Text = "操作";
            // 
            // btnOpenRegedit2
            // 
            this.btnOpenRegedit2.Location = new System.Drawing.Point(582, 35);
            this.btnOpenRegedit2.Name = "btnOpenRegedit2";
            this.btnOpenRegedit2.Size = new System.Drawing.Size(150, 40);
            this.btnOpenRegedit2.TabIndex = 3;
            this.btnOpenRegedit2.Text = "RegEditで開く";
            this.btnOpenRegedit2.UseVisualStyleBackColor = true;
            this.btnOpenRegedit2.Click += new System.EventHandler(this.btnOpenRegedit2_Click);
            // 
            // btnRestore2
            // 
            this.btnRestore2.Location = new System.Drawing.Point(400, 35);
            this.btnRestore2.Name = "btnRestore2";
            this.btnRestore2.Size = new System.Drawing.Size(150, 40);
            this.btnRestore2.TabIndex = 2;
            this.btnRestore2.Text = "元に戻す";
            this.btnRestore2.UseVisualStyleBackColor = true;
            this.btnRestore2.Click += new System.EventHandler(this.btnRestore2_Click);
            // 
            // btnApply2
            // 
            this.btnApply2.Location = new System.Drawing.Point(220, 35);
            this.btnApply2.Name = "btnApply2";
            this.btnApply2.Size = new System.Drawing.Size(150, 40);
            this.btnApply2.TabIndex = 1;
            this.btnApply2.Text = "修正を適用";
            this.btnApply2.UseVisualStyleBackColor = true;
            this.btnApply2.Click += new System.EventHandler(this.btnApply2_Click);
            // 
            // btnCheck2
            // 
            this.btnCheck2.Location = new System.Drawing.Point(45, 35);
            this.btnCheck2.Name = "btnCheck2";
            this.btnCheck2.Size = new System.Drawing.Size(150, 40);
            this.btnCheck2.TabIndex = 0;
            this.btnCheck2.Text = "状態確認";
            this.btnCheck2.UseVisualStyleBackColor = true;
            this.btnCheck2.Click += new System.EventHandler(this.btnCheck2_Click);
            // 
            // txtLog2
            // 
            this.txtLog2.Location = new System.Drawing.Point(8, 60);
            this.txtLog2.Multiline = true;
            this.txtLog2.Name = "txtLog2";
            this.txtLog2.ReadOnly = true;
            this.txtLog2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog2.Size = new System.Drawing.Size(776, 180);
            this.txtLog2.TabIndex = 1;
            // 
            // infoLabel2
            // 
            this.infoLabel2.AutoSize = true;
            this.infoLabel2.Location = new System.Drawing.Point(8, 15);
            this.infoLabel2.Name = "infoLabel2";
            this.infoLabel2.Size = new System.Drawing.Size(550, 20);
            this.infoLabel2.TabIndex = 0;
            this.infoLabel2.Text = "AppXSvc (AppX Deployment Service) を無効化(Start=4)します。\r\n";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "RegEdit Tool";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.grpActions.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.grpActions2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox grpActions;
        private System.Windows.Forms.Button btnOpenRegedit;
        private System.Windows.Forms.Button btnRestore;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCheck;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label infoLabel;
        // Tab2 controls
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox grpActions2;
        private System.Windows.Forms.Button btnOpenRegedit2;
        private System.Windows.Forms.Button btnRestore2;
        private System.Windows.Forms.Button btnApply2;
        private System.Windows.Forms.Button btnCheck2;
        private System.Windows.Forms.TextBox txtLog2;
        private System.Windows.Forms.Label infoLabel2;
    }
}
