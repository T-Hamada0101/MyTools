using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace EncodeAuto
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
            AddDropEvents();
            //this.TopMost = true;
            Properties.Settings.Default.Reload();
            textBox1.Text = Properties.Settings.Default.ExePath;
            textBox2.Text = Properties.Settings.Default.Arguments;
            textBox3.Text = Properties.Settings.Default.Safix;
            textBox4.Text = Properties.Settings.Default.OutputDir;
            checkBox1.Checked = Properties.Settings.Default.MoveComp;
        }

        #region D&Dイベント
        private void AddDropEvents()
        {
            //------------D&Dイベント登録------------
            this.AllowDrop = true;//ドロップを受け入れる
            this.DragEnter += new DragEventHandler(ProcDragEnter);
            this.DragDrop += new DragEventHandler(ProcDragDrop);

        }
        public void ProcDragEnter(object? sender, DragEventArgs e)//D&Dイベント
        {
            if (e.Data is null) return;
            ////ドラッグされたデータ形式を調べ ファイルのみ受け入れる
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
        }
        private void ProcDragDrop(object? sender, DragEventArgs e)//D&Dイベント
        {
            string[] files = ExtractDragDropPath(e);
            AddListboxItem(files);
        }
        /// <summary>
        /// ドロップされたファイルのパス一覧を作成
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private string[] ExtractDragDropPath(DragEventArgs e)
        {
            string[] pass = new string[1];

            if (e.Data is null) return pass;
            //ドロップされたのがファイルの場合
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                return files;
            }
            else//テキスト(リンクを含む)の場合
            {
                //ドロップされたリンクのURLを取得する
                pass[0] = e.Data.GetData(DataFormats.Text).ToString();
            }
            return pass;
        }
        #endregion

        /// <summary>
        /// パスをListboxに重複を許さずに追加（フォルダ内も検索）
        /// </summary>
        /// <param name="files"></param>
        private void AddListboxItem(string[] files)
        {
            // ListBox1 の 既存Items を HashSet に変換する
            List<string> listbox = listBox1.Items.Cast<string>().ToList();
            HashSet<string> hashSet = new HashSet<string>(listbox);

            //引数分を追加
            List<string> lists = FileUtils.GetAllFilePath(files);
            foreach (string _file in lists) 
            {
                hashSet.Add(_file);
            }

            if (hashSet.Count > 0)
            {
                listBox1.Items.Clear();
                listBox1.Items.AddRange(hashSet.ToArray());
                //listBox1.Items.AddRange(lists.ToArray());
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            //string exePath = textBox1.Text;

            Properties.Settings.Default.ExePath = textBox1.Text;
            Properties.Settings.Default.Arguments = textBox2.Text;
            Properties.Settings.Default.Safix = textBox3.Text;
            Properties.Settings.Default.OutputDir = textBox4.Text;
            Properties.Settings.Default.MoveComp = checkBox1.Checked;
            Properties.Settings.Default.Save();

            // ListBox1 の Items を List<string> に変換する
            List<string> list = listBox1.Items.Cast<string>().ToList();

            EncodeDeta deta = new EncodeDeta(list);
            Task.Run(() => new Encoder(deta));
            //ProcessUtils.RunCommand(batPath, null);
            listBox1.Items.Clear();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
        /// <summary>
        /// Add Input Dir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            string outDir = textBox4.Text;
            Properties.Settings.Default.OutputDir = outDir;
            Properties.Settings.Default.Save();
            AddListboxItem(new string[] { outDir + @"\Input" });
        }
    }
}