namespace MyExcelReader
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.button_ReadExcel = new System.Windows.Forms.Button();
            this.textBox_FilePath = new System.Windows.Forms.TextBox();
            this.textBox_FileName = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_ReadExcel
            // 
            this.button_ReadExcel.Location = new System.Drawing.Point(556, 3);
            this.button_ReadExcel.Name = "button_ReadExcel";
            this.button_ReadExcel.Size = new System.Drawing.Size(93, 23);
            this.button_ReadExcel.TabIndex = 0;
            this.button_ReadExcel.Text = "ReadExcel";
            this.button_ReadExcel.UseVisualStyleBackColor = true;
            this.button_ReadExcel.Click += new System.EventHandler(this.Button_ReadExcel_Click);
            // 
            // textBox_FilePath
            // 
            this.textBox_FilePath.Location = new System.Drawing.Point(3, 3);
            this.textBox_FilePath.Name = "textBox_FilePath";
            this.textBox_FilePath.Size = new System.Drawing.Size(547, 22);
            this.textBox_FilePath.TabIndex = 1;
            // 
            // textBox_FileName
            // 
            this.textBox_FileName.Location = new System.Drawing.Point(3, 36);
            this.textBox_FileName.Name = "textBox_FileName";
            this.textBox_FileName.Size = new System.Drawing.Size(547, 22);
            this.textBox_FileName.TabIndex = 2;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(253, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(240, 150);
            this.dataGridView1.TabIndex = 3;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 82.78443F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.21557F));
            this.tableLayoutPanel1.Controls.Add(this.textBox_FilePath, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBox_FileName, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.button_ReadExcel, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(16, 13);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(668, 67);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 93;
            this.splitContainer1.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_ReadExcel;
        private System.Windows.Forms.TextBox textBox_FilePath;
        private System.Windows.Forms.TextBox textBox_FileName;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}

