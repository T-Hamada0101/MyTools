using System;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;
using System.Threading;
using System.Threading.Tasks;
using yt_dlp_loader.Properties;
using static System.Net.Mime.MediaTypeNames;

namespace yt_dlp_loader
{
    public partial class Form1 : Form
    {
        int waiteSec = 0;
        string downloadFolder = @"E:\_Youtube";

        // プロファイルディレクトリの設定
        string profileDir = @"dkj5wpar.Suteyo"; // Firefoxのプロファイル名
                                                // string profileDir = @"Profile 2"; // Chromeのプロファイル名

        string configFilePath = "";
        string cookiesOption = "";
        //コンフィグファイルは、都度自動生成する
        public Form1()
        {
            InitializeComponent();
            AddDropEvents();

            //comboBox1にブラウザ名を追加
            comboBox1.Items.Add("Firefox PPP");
            comboBox1.Items.Add("Firefox");
            comboBox1.Items.Add("Chrome");

            //this.TopMost = true;
            LoadProperties();
        }
        private void LoadProperties()
        {
            Properties.Settings.Default.Reload();
            textBoxExePath.Text = Properties.Settings.Default.ExePath;
            textBoxUrlFile.Text = Properties.Settings.Default.UrlFilePath;
            textBoxDLFolder.Text = Properties.Settings.Default.DownloadDirectory;
            checkBox1.Checked = Properties.Settings.Default.IsOpenUrl;
            textBox3.Text = Properties.Settings.Default.BrowserOpenTime.ToString();
            numericUpDown1.Value = Properties.Settings.Default.DLThreads;
            checkBox3.Checked = Properties.Settings.Default.AddDownloaderName;
            checkBox2.Checked = Properties.Settings.Default.AddVideoId;
            checkBox6.Checked = Properties.Settings.Default.LimitSize720p;
            textBox4.Text = Properties.Settings.Default.AddText1;
            textBox5.Text = Properties.Settings.Default.AddText2;
            comboBox1.SelectedText = Properties.Settings.Default.SelectBrowserProfile;
            configFilePath = Properties.Settings.Default.ExePath.Replace("yt-dlp.exe", "yt-dlp.conf");
            if (File.Exists(configFilePath))
            {
                ShowConfigFile(configFilePath);
            }

        }
        public void SaveSetting()
        {
            Properties.Settings.Default.ExePath = textBoxExePath.Text;
            Properties.Settings.Default.UrlFilePath = textBoxUrlFile.Text;
            Properties.Settings.Default.DownloadDirectory = textBoxDLFolder.Text;
            Properties.Settings.Default.IsOpenUrl = checkBox1.Checked;
            Properties.Settings.Default.AddDownloaderName = checkBox3.Checked;
            Properties.Settings.Default.AddVideoId = checkBox2.Checked;
            Properties.Settings.Default.LimitSize720p = checkBox6.Checked;
            Properties.Settings.Default.AddText1 = textBox4.Text;
            Properties.Settings.Default.AddText2 = textBox5.Text;
            Properties.Settings.Default.SelectBrowserProfile = comboBox1.SelectedItem.ToString() ?? "Firefox";
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

            //string configFilePath = @"D:\yt-dlp.conf";
            string configFilePath = Properties.Settings.Default.ExePath.Replace("yt-dlp.exe", "yt-dlp.conf");
            if (!File.Exists(configFilePath))
            {
                WriteConfigFile(configFilePath);
                ShowConfigFile(configFilePath);
            }
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
        /*
         
         ListViewのアイテムのテキストをユーザーが編集できるようにする
         https://dobon.net/vb/dotnet/control/lvlabeledit.html
         
         
         */
        private void AddListboxEditEvents()
        {
            //------------イベント登録------------
            // Add an event handler for the ListBox's MouseDoubleClick event
            listBox1.MouseDoubleClick += (sender, e) =>
            {
                // Get the index of the clicked item
                int index = listBox1.IndexFromPoint(e.Location);

                // If the index is valid, enter edit mode
                if (index != ListBox.NoMatches)
                {
                    // Get the bounds of the clicked item
                    Rectangle bounds = listBox1.GetItemRectangle(index);

                    // Set the TextBox's location and size to match the clicked item
                    textBoxExePath.Location = new Point(bounds.X, bounds.Y);
                    textBoxExePath.Size = new Size(bounds.Width, bounds.Height);

                    // Set the TextBox's text to the clicked item's text
                    textBoxExePath.Text = listBox1.Items[index].ToString();

                    // Make the TextBox visible and give it focus
                    textBoxExePath.Visible = true;
                    textBoxExePath.Focus();
                }

            };
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

        /// <summary>
        /// 通常の起動 //string configFilePath = @"D:\yt-dlp.conf";を指定して起動する場合
        /// </summary>
        /// <param name="_command"></param>
        /// <param name="_arguments"></param>
        public void RunCommand(string _command, string? _arguments)
        {
            if (string.IsNullOrEmpty(_arguments))
            {
                Process.Start(_command);
            }
            else
            {
                Process.Start(_command, _arguments);
            }
        }
        public void RunCommandPause(string command, string? arguments)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                UseShellExecute = false,
                CreateNoWindow = false
            };

            using (Process process = new Process())
            {
                process.StartInfo = startInfo;
                process.Start();

                using (StreamWriter sw = process.StandardInput)
                {
                    if (sw.BaseStream.CanWrite)
                    {
                        if (!string.IsNullOrEmpty(arguments))
                        {
                            sw.WriteLine($"{command} {arguments}");
                        }
                        else
                        {
                            sw.WriteLine(command);
                        }
                        sw.WriteLine("pause"); // This will keep the command prompt open
                    }
                }

                process.WaitForExit();
            }
        }
        private async void buttonStart_Click(object sender, EventArgs e)
        {
            string exePath = textBoxExePath.Text;
            string urlFilePath = textBoxUrlFile.Text;
            //UIで指定した設定とconfigFileを保存する
            SaveSetting();
            // ListBox1 の Items を List<string> に変換する
            List<string> urls = listBox1.Items.Cast<string>().ToList();
            WriteUrlFile(urlFilePath, urls);

            //DLスレッド数
            int thred = Properties.Settings.Default.DLThreads;


            //await RunCommandAsync("cmd.exe",exePath);
            //RunCommand("cmd.exe", exePath);
#if DEBUG
            //RunCommand(exePath, @"-f bestaudio+bestvideo");
            //RunCommandPause(exePath, null);
            RunCommand(exePath, null);
#else

            RunCommand(exePath, null);
#endif
            //視聴済みにするためにブラウザを開く
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

        //brwser cookie の設定をdropdownで選択できるようにする
        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            GetCookiesOption();
            WriteConfigFile(configFilePath);
            ShowConfigFile(configFilePath);
        }

        private string GetCookiesOption()
        {
            string selectedBrowser = comboBox1.SelectedItem.ToString();
            string browser;
            switch (selectedBrowser)
            {
                case "Firefox":
                    browser = "firefox";
                    profileDir = @"19cp24ya.default-release-1"; // Firefoxのプロファイル名
                    break;
                case "Firefox PPP":
                    browser = "firefox";
                    profileDir = @"dkj5wpar.Suteyo"; // Firefoxのプロファイル名
                    break;
                case "Chrome":
                    browser = "chrome";
                    profileDir = @"Profile 2"; // Chromeのプロファイル名
                    break;
                default:
                    browser = "firefox";
                    profileDir = @"dkj5wpar.Suteyo"; // デフォルトはFirefox
                    break;
            }
            string cookiesOption = $@"--cookies-from-browser {browser}:{profileDir}"; // クッキーを取得;
            return cookiesOption;

        }

        /// <summary>
        ///  コンフィグファイルの内容をtextboxに表示する
        /// </summary>
        /// <param name="configFilePath"></param>
        private void ShowConfigFile(string configFilePath)
        {
            if (File.Exists(configFilePath))
            {
                string[] lines = File.ReadAllLines(configFilePath);
                textBoxConfigFile.Lines = lines;
            }
        }

        /// <summary>
        /// コンフィグファイルの書き込み(ファイルが無ければ作成する)
        /// </summary>
        /// <param name="configFilePath"></param>
        private void WriteConfigFile(string configFilePath)
        {
            string[] driveletters = Environment.GetLogicalDrives();

            string driveLetter = driveletters.FirstOrDefault(d => d.Length == 3 && char.IsLetter(d[0]) && d[1] == ':' && d[2] == '\\');
            if (string.IsNullOrEmpty(driveLetter)) driveLetter = @"C:\"; //デフォルトドライブ


            ClearFile(configFilePath);
            //write urlFile

            downloadFolder = Properties.Settings.Default.DownloadDirectory;

            string OptionsForOutFileName = $@"{downloadFolder}\%(title)s";
            //string OptionsForOutFileName = @"{driveLetter}_Youtube\%(title)s";
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



            // 出力ファイル名のオプションを設定
            OptionsForOutFileName = @"-o """ + OptionsForOutFileName + @".%(ext)s" + @"""";
            AppendTextToFile(configFilePath, OptionsForOutFileName); // 出力ファイル名を設定

            // その他のオプションを設定
            AppendTextToFile(configFilePath, @"--no-mtime"); // ファイルの最終更新時刻を変更しない
            AppendTextToFile(configFilePath, @"--console-title"); // コンソールのタイトルを設定
            AppendTextToFile(configFilePath, GetCookiesOption()); // Firefoxからクッキーを取得
            // AppendTextToFile(configFilePath, $@"--cookies-from-browser chrome:{"Profile 2"}"); // Chromeからクッキーを取得（動作NG）

            // URLファイルの設定
            AppendTextToFile(configFilePath, $@"-a ""{Settings.Default.UrlFilePath}"""); // URLファイルを指定

            // 720p制限オプションの設定
            if (checkBox6.Checked)
            {
                AppendTextToFile(configFilePath, @"--format-sort res:720"); // 720pに制限
            }
            else
            {
                // AppendTextToFile(configFilePath, @"-f bestaudio+bestvideo"); // 最良の音声と映像を取得するオプション（コメントアウト）
            }
            //AppendTextToFile(configFilePath, @"--remux-video mkv");//うまく動かない
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
            //引数指定：-P "profile_name"
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }
            ProcessStartInfo pi = new ProcessStartInfo();

            string selectedBrowser = comboBox1.SelectedText;
            switch (selectedBrowser)
            {
                case "Firefox":
                case "Firefox PPP":
                    pi.FileName = @"C:\Program Files\Mozilla Firefox\firefox.exe";
                    //pi.Arguments = $"-P \"{profileDir}\" \"{url}\"";
                    break;
                case "Chrome":
                    pi.FileName = "chrome.exe";
                    pi.Arguments = $"--profile-directory=\"{profileDir}\" \"{url}\"";
                    break;
                default:
                    pi.FileName = "firefox.exe";
                    pi.Arguments = $"-P \"{profileDir}\" \"{url}\"";
                    break;
            }
            pi.UseShellExecute = false;
            //pi.RedirectStandardOutput = true;
            //pi.CreateNoWindow = true;
            //pi.RedirectStandardError = true;
            //pi.RedirectStandardInput = true;


            //Verb = "runas",           // これで管理者権限になる

            //ProcessStartInfo pi = new ProcessStartInfo()
            //{
            //    FileName = url,
            //    UseShellExecute = true,
            //};

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
            System.Diagnostics.Process.Start("EXPLORER.EXE", Settings.Default.DownloadDirectory);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Openブラウザボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            OprnUrl("https://www.youtube.com/");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // ListBox1 の Items を List<string> に変換する
            List<string> urls = listBox1.Items.Cast<string>().ToList();
            string _command = textBoxExePath.Text;
            string _arguments = $" -f bestvideo+bestaudio {urls[0]}";

            Process.Start(_command, _arguments);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedText = Properties.Settings.Default.SelectBrowserProfile;
        }

    }
}