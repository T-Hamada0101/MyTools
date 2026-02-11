using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExcelDataReader; //install this package from NuGet
using System.IO;
using System.Reflection;

namespace MyExcelReader
{
    public partial class Form1 : Form
    {
        private string[] filePath = null;
        private string[] fileName = null;
        public Form1()
        {
            InitializeComponent();
            //splitContainer1をフォームにフィットさせる
            splitContainer1.Dock = DockStyle.Fill;

            //DetaGridViewの設定
            //dataGridView1.AllowUserToAddRows = false;
            //dataGridView1.AllowUserToDeleteRows = false;
            //dataGridView1.ReadOnly = true;
            //dataGridView1.RowHeadersVisible = false;
            //dataGridView1.ColumnHeadersVisible = true;
            //dataGridView1.AllowUserToResizeColumns = false;
            //dataGridView1.AllowUserToResizeRows = false;
            //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            //dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //dataGridView1.MultiSelect = false;
            //DetaGridViewのサイズをフォームに合わせる
            //dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            //DetaGridViewをフォームにフィットさせる
            dataGridView1.Dock = DockStyle.Fill;
            //DetaGridViewの描画を高速化するDoubleBuffered
            EnableDoubleBuffering(dataGridView1);
        }
        // <summary>
        /// コントロールのDoubleBufferedプロパティをTrueにする
        /// </summary>
        /// <param name="control">対象のコントロール</param>
        public static void EnableDoubleBuffering(Control control)
        {
            control.GetType().InvokeMember(
               "DoubleBuffered",
               BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
               null,
               control,
               new object[] { true });
        }

        private void Button_ReadExcel_Click(object sender, EventArgs e)
        {
            //openfiledialog
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //get the path of the file
                filePath = ofd.FileNames;
                //get the name of the file
                string[] safeFilePath = ofd.SafeFileNames;
                //display the file path in the textbox
                textBox_FilePath.Text = string.Join(",", filePath);
                //display the file name in the textbox
                textBox_FileName.Text = string.Join(",", safeFilePath);
            }
            if (filePath != null)
            {
                ExcelToDetaset();
            }
        }

        private void ExcelToDetaset()
        {
            //ExcelDataReaderでExcelファイルを読み込む
            FileStream stream = File.Open(filePath[0], FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            //DataSetにExcelの内容を読み込む
            DataSet result = excelReader.AsDataSet();
            //DataSetの内容をコンソールに出力する
            for (int i = 0; i < result.Tables.Count; i++)
            {
                Console.WriteLine("TableName:" + result.Tables[i].TableName);
                for (int j = 0; j < result.Tables[i].Rows.Count; j++)
                {
                    for (int k = 0; k < result.Tables[i].Columns.Count; k++)
                    {
                        Console.Write(result.Tables[i].Rows[j][k] + ",");
                    }
                    Console.WriteLine();
                }
            }
            // DataSetの内容をDataGridViewに表示する
            dataGridView1.DataSource = result.Tables[0];

            //ExcelDataReaderのインスタンスを解放する
            excelReader.Close();
        }


        //DetaGridViewのセルの変更を検知する
        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            //セルの変更前の値を取得する
            Console.WriteLine("変更前の値:" + dataGridView1[e.ColumnIndex, e.RowIndex].Value);
        }

        //DetaGridViewのセルの変更をDetaSetに反映させる
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //DataGridViewの内容をDataSetに反映させる
            DataSet ds = (DataSet)dataGridView1.DataSource;
            //DataSetの内容をコンソールに出力する
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                Console.WriteLine("TableName:" + ds.Tables[i].TableName);
                for (int j = 0; j < ds.Tables[i].Rows.Count; j++)
                {
                    for (int k = 0; k < ds.Tables[i].Columns.Count; k++)
                    {
                        Console.Write(ds.Tables[i].Rows[j][k] + ",");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
