using EncodeAuto;
using MovieCut.ffmpeg;
namespace MovieCut
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // load ffmpeg path
            label1.Text = Properties.Settings.Default.ffmpegPath;
            AddDropEvents();

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
                if (!FileUtils.IsMovieFile(_file)) { continue; }
                hashSet.Add(_file);
            }

            if (hashSet.Count > 0)
            {
                listBox1.Items.Clear();
                listBox1.Items.AddRange(hashSet.ToArray());
                //listBox1.Items.AddRange(lists.ToArray());
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            // ListBox1 の Items を List<string> に変換する
            List<string> allList = listBox1.Items.Cast<string>().ToList();

            silencedetect.GetSilentReport(allList[0]);

            ////　Create ffmpeg process
            //System.Diagnostics.Process p = new System.Diagnostics.Process();
            ////　Set ffmpeg path
            //p.StartInfo.FileName = Properties.Settings.Default.ffmpegPath;
            ////　Set ffmpeg arguments
            //p.StartInfo.Arguments = "-ss 00:00:00 -t 00:00:10 -i " + allList[0] + " -vcodec copy -acodec copy " + allList[0] + "_cut.mp4";
            ////　Start ffmpeg
            //p.Start();
            ////　Wait for ffmpeg to finish
            //p.WaitForExit();
            ////　Close ffmpeg process
            //p.Close();
        }

        /// <summary>
        /// ffmpegのパスを指定するボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            //　Open file dialog
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "ffmpeg file(*.exe)|*.exe|All files (*.*)|*.*";
            ofd.Title = "Open ffmpeg file";
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //　Get file path
                string filePath = ofd.FileName;
                //　Get file name
                string fileName = System.IO.Path.GetFileName(filePath);
                //　Get file extension
                string fileExtension = System.IO.Path.GetExtension(filePath);
                //　Get file directory
                string fileDirectory = System.IO.Path.GetDirectoryName(filePath);

                //　Create output file path
                string outputFilePath = fileDirectory + "\\" + fileName + "_cut" + fileExtension;

                // save ffmpeg path
                Properties.Settings.Default.ffmpegPath = filePath;
                Properties.Settings.Default.Save();
                label1.Text = filePath;

            }
        }
    }
}
