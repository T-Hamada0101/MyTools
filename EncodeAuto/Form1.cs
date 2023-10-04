using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace EncodeAuto
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 絵文字ファイル名と一時変更ファイル名をを保存
        /// </summary>
        private Dictionary<string, string> EmojiFileDic = new Dictionary<string, string>();
        /// <summary>
        /// 対象ファイルlist
        /// </summary>
        private List<string> AllFiles = new List<string>();
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
            string[] files = makeUrlDD(e);
            foreach (string file in files)
            {
                string[] _fs = new string[] { file };
                if (IsDirectory(file))
                {
                    _fs = GetAllFilesInDir(file);
                }

                foreach (string f in _fs)
                {
                    if (File.Exists(f)) { listBox1.Items.Add(f); }
                }
            }
        }

        private string[] GetAllFilesInDir(string dir)
        {
            return Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories);
        }

        private bool IsDirectory(string path)
        {
            if (string.IsNullOrEmpty(path)) return false;
            return File.GetAttributes(path).HasFlag(FileAttributes.Directory);
        }
        private string[] EnumerateFileNamesInFolder(string path)
        {
            return Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
        }

        private string[] makeUrlDD(DragEventArgs e)
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

        /// <summary>
        /// 非同期で外部コマンド実行
        /// </summary>
        /// <param name="command"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public async Task RunCommandAsync(string _command, string _arguments)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _command,
                    //Arguments = _arguments,
                    UseShellExecute = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            process.Start();
            await process.WaitForExitAsync();
        }

        public void RunCommand(string _command, string? _arguments)
        {
            if (!string.IsNullOrEmpty(_arguments))
            {
                Process.Start(_command);
            }
            else
            {
                Process.Start(_command, _arguments);
            }
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            //string exePath = textBox1.Text;
            string batPath = textBox1.Text;
            string _arguments = textBox2.Text;
            string _safix = textBox3.Text;
            string _outDir = textBox4.Text;
            Properties.Settings.Default.ExePath = textBox1.Text;
            Properties.Settings.Default.Arguments = textBox2.Text;
            Properties.Settings.Default.Safix = textBox3.Text;
            Properties.Settings.Default.OutputDir = textBox4.Text;
            Properties.Settings.Default.MoveComp = checkBox1.Checked;
            Properties.Settings.Default.Save();
            CleateDir(_outDir);
            ClearFile(batPath);
            AppendTextToFile(batPath, @"cd /d %~dp0");
            foreach (string pass in listBox1.Items)
            {
                AllFiles.Add(pass);

                //絵文字除去
                string after = RenameEmojiFile(pass);

                //拡張子なしのファイル名の取得:
                string _noExt = Path.GetFileNameWithoutExtension(after);
                string _out = _outDir + @"\" + _noExt + _safix;


                string arg = _arguments.Replace(@"%input", @"""" + after + @"""");
                arg = arg.Replace(@"%out", @"""" + _out + @"""");
                AppendTextToFile(batPath, arg);
            }
            AppendTextToFile(batPath, "pause");
            //await RunCommandAsync("cmd.exe",exePath);
            //RunCommand("cmd.exe", exePath);
            RunCommand(batPath, null);
            listBox1.Items.Clear();
        }

        private string RenameEmojiFile(string filePath)
        {
            string after = Regexs.InputValueValidate(filePath);
            if (!after.Equals(filePath))
            {
                EmojiFileDic.Add(filePath, after);
                File.Move(filePath, after);
            }
            return after;
        }
        private void ComebackEmojiFile()
        {
            foreach (var item in EmojiFileDic)
            {
                try
                {
                    File.Move(item.Value, item.Key);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                finally
                {
                    EmojiFileDic.Remove(item.Key);
                }
            }
        }
        private void MoveCompleatedFile()
        {
            if (!Properties.Settings.Default.MoveComp)
            {
                return;
            }
            string ourDir = Properties.Settings.Default.OutputDir + @"\completed";
            CleateDir(ourDir);
            foreach (var item in AllFiles)
            {
                try
                {
                    string fName = Path.GetFileName(item);
                    File.Move(item, ourDir + @"\" + fName);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        public void AppendTextToFile(string filePath, string text)
        {
            EncodingProvider provider = System.Text.CodePagesEncodingProvider.Instance;
            var encoding = provider.GetEncoding("shift-jis");
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(filePath,true, encoding))
            // System.Text.Encoding.GetEncoding("Shift-JIS"))
            //TextBox1.Textの内容を書き込む
            {
                sw.WriteLine(text);
            }
        }
        private void CleateDir(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    //Console.WriteLine($"Directory created: {path}");
                }
                else
                {
                    //Console.WriteLine($"Directory already exists: {path}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating directory: {ex.Message}");
            }
        }
        public void ClearFile(string filePath)
        {
            File.WriteAllText(filePath, string.Empty);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ComebackEmojiFile();
            MoveCompleatedFile();
        }
    }
}