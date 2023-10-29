using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using yt_dlp_loader.Properties;
using static System.Net.Mime.MediaTypeNames;

namespace yt_dlp_loader
{
    public partial class Form1 : Form
    {
        int waiteSec = 0;


        public Form1()
        {
            InitializeComponent();
            AddDropEvents();
            //this.TopMost = true;
            LoadProperties();
        }
        private void LoadProperties()
        {
            Properties.Settings.Default.Reload();
            textBox1.Text = Properties.Settings.Default.ExePath;
            textBox2.Text = Properties.Settings.Default.UrlFilePath;
            checkBox1.Checked = Properties.Settings.Default.IsOpenUrl;
            textBox3.Text = Properties.Settings.Default.BrowserOpenTime.ToString();
            numericUpDown1.Value = Properties.Settings.Default.DLThreads;
            checkBox3.Checked = Properties.Settings.Default.AddDownloaderName;
            checkBox2.Checked = Properties.Settings.Default.AddVideoId;
        }
        public void SaveSetting()
        {
            Properties.Settings.Default.ExePath = textBox1.Text;
            Properties.Settings.Default.UrlFilePath = textBox2.Text;
            Properties.Settings.Default.IsOpenUrl = checkBox1.Checked;
            Properties.Settings.Default.AddDownloaderName = checkBox3.Checked;
            Properties.Settings.Default.AddVideoId = checkBox2.Checked;

            int threads = (int)numericUpDown1.Value;
            if (threads < 0) threads = 1;
            Properties.Settings.Default.DLThreads = threads;

            if (!int.TryParse(textBox3.Text, out int w))
            {
                w = 0;
            }
            waiteSec = w;

            Properties.Settings.Default.BrowserOpenTime = waiteSec;
            Properties.Settings.Default.Save();
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
            ////ドラッグされたデータ形式を調べ URLのみ受け入れる
            if (e.Data.GetDataPresent("UniformResourceLocator"))
            {
                e.Effect = DragDropEffects.Link;
            }
        }
        private void ProcDragDrop(object? sender, DragEventArgs e)//D&Dイベント
        {
            string url = makeUrlDD(e);
            if (!listBox1.Items.Contains(url) && url.Length > 10)
            {
                listBox1.Items.Add(url);
            }

        }

        private string makeUrlDD(DragEventArgs e)
        {
            if (e.Data is null) return "";
            string url = "";
            //ドロップされたのがファイルの場合
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var _st = e.Data.GetData(DataFormats.FileDrop);
                string[] strList = (string[])_st;

                url = strList[0];
            }
            else//テキスト(リンクを含む)の場合
            {
                //ドロップされたリンクのURLを取得する
                url = e.Data.GetData(DataFormats.Text).ToString();
            }
            return url;
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
            string exePath = textBox1.Text;
            string urlFilePath = textBox2.Text;
            SaveSetting();
            // ListBox1 の Items を List<string> に変換する
            List<string> urls = listBox1.Items.Cast<string>().ToList();
            WriteUrlFile(urlFilePath, urls);

            int thred = Properties.Settings.Default.DLThreads;
            WriteConfigFile(thred);
            //await RunCommandAsync("cmd.exe",exePath);
            //RunCommand("cmd.exe", exePath);
            RunCommand(exePath, null);
            RunBrowser(urls);
            listBox1.Items.Clear();
        }

        private void WriteUrlFile(string urlFilePath, List<string> urls)
        {
            ClearFile(urlFilePath);
            //write urlFile
            foreach (string item in urls)
            {
                AppendTextToFile(urlFilePath, item);
            }
        }
        private void WriteConfigFile(int thred)
        {
            string configFilePath = Properties.Settings.Default.ExePath.Replace("yt-dlp.exe", "yt-dlp.conf");
            ClearFile(configFilePath);
            //write urlFile
            //string OptionsForOutFileName = @"-o ""D:\WD12share\_Youtube\%(title)s.%(ext)s""";
            string OptionsForOutFileName = @"D:\WD12share\_Youtube\%(title)s";
            if (checkBox3.Checked)
            {
                OptionsForOutFileName += @"_%(uploader)s";
            }
            if (checkBox2.Checked)
            {
                OptionsForOutFileName += @"_%(id)s";
            }
            if (checkBox4.Checked)
            {
                OptionsForOutFileName += textBox4.Text.Trim();
            }

            if (checkBox5.Checked)
            {
                OptionsForOutFileName += textBox5.Text.Trim();
            }


            OptionsForOutFileName = @"-o """ + OptionsForOutFileName + @".%(ext)s" + @"""";
            AppendTextToFile(configFilePath, OptionsForOutFileName);
            AppendTextToFile(configFilePath, @"- N " + thred.ToString());
            AppendTextToFile(configFilePath, @"--no-mtime");
            AppendTextToFile(configFilePath, @"--console-title");
            AppendTextToFile(configFilePath, @"--cookies-from-browser firefox");
            AppendTextToFile(configFilePath, @"-a ""D:\WD12share\_yt-dlp_url.txt""");
        }
        private void RunBrowser(List<string> urls)
        {
            int waiteSec = Properties.Settings.Default.BrowserOpenTime;
            if (!checkBox1.Checked) return;
            foreach (var url in urls)
            {
                Process p = OprnUrl(url);
                Thread.Sleep(waiteSec * 1000);
            }

        }
        private Process OprnUrl(string url)
        {
            ProcessStartInfo pi = new ProcessStartInfo()
            {
                FileName = url,
                UseShellExecute = true,
            };

            return Process.Start(pi);
        }

        public void AppendTextToFile(string filePath, string text)
        {
            using (StreamWriter writer = File.AppendText(filePath))
            {
                writer.WriteLine(text);
            }
        }

        public void ClearFile(string filePath)
        {
            File.WriteAllText(filePath, string.Empty);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveSetting();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("EXPLORER.EXE", @"D:\WD12share\_Youtube");
        }
    }
}