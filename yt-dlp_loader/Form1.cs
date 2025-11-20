using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using yt_dlp_loader.Properties;

namespace yt_dlp_loader
{
    public partial class Form1 : Form
    {
        private readonly YtDlpService ytDlpService = new();
        private string profileDir = @"dkj5wpar.Suteyo";
        private string configFilePath = string.Empty;

        public Form1()
        {
            InitializeComponent();
            AddDropEvents();

            // comboBox1 にブラウザ候補を追加
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
            textBox1.Text = Properties.Settings.Default.AddPrefix1;
            textBox4.Text = Properties.Settings.Default.AddText1;
            textBox5.Text = Properties.Settings.Default.AddText2;
            SelectBrowserProfile(Properties.Settings.Default.SelectBrowserProfile);
            
            // ユーザーAppDataフォルダの設定ファイルパスを取得
            var tempOptions = new YtDlpOptions { ExePath = Properties.Settings.Default.ExePath ?? string.Empty };
            configFilePath = tempOptions.EnsureConfigFilePath();
            if (File.Exists(configFilePath))
            {
                ShowConfigFile(configFilePath);
            }

        }
        public void SaveSetting()
        {
            var options = BuildOptionsFromControls();
            ytDlpService.SaveSettings(options);
            UpdateConfigFile(options);
        }

        private void AddDropEvents()
        {
            //------------D&D イベント登録------------
            this.AllowDrop = true;// ドラッグを受け入れる
            this.DragEnter += new DragEventHandler(ProcDragEnter);
            this.DragDrop += new DragEventHandler(ProcDragDrop);

        }
        public void ProcDragEnter(object? sender, DragEventArgs e)// D&D イベント
        {
            if (e.Data is null) return;
            //// ドラッグされたデータ形式を確認し URL のみ受け付ける
            if (e.Data.GetDataPresent("UniformResourceLocator"))
            {
                e.Effect = DragDropEffects.Link;
            }
        }
        private void ProcDragDrop(object? sender, DragEventArgs e)// D&D イベント
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
            // ドロップされたものがファイルの場合
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if (e.Data.GetData(DataFormats.FileDrop) is string[] strList && strList.Length > 0)
                {
                    url = strList[0];
                }
            }
            else// テキスト（URL など）の場合
            {
                // ドロップされたテキストから URL を取得
                if (e.Data.GetData(DataFormats.Text) is string text)
                {
                    url = text;
                }
            }
            return url;
        }
        /*
         
         ListView のアイテムのテキストをユーザーが編集できるようにする
         https://dobon.net/vb/dotnet/control/lvlabeledit.html
         
         
         */
        private void AddListboxEditEvents()
        {
            //------------ イベント登録 ------------
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
        private void buttonStart_Click(object sender, EventArgs e)
        {
            var options = BuildOptionsFromControls();
            ytDlpService.SaveSettings(options);
            UpdateConfigFile(options);

            List<string> urls = listBox1.Items.Cast<string>().ToList();
            ytDlpService.WriteUrlFile(options.UrlFilePath, urls);

            ytDlpService.RunYtDlp(options);
            RunBrowser(urls, options);
            listBox1.Items.Clear();
        }

        // ブラウザ cookie の設定をドロップダウンで切り替える
        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            UpdateConfigFile(BuildOptionsFromControls());
        }

        /// <summary>
        ///  コンフィグファイルの内容を TextBox に表示
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
        /// コンフィグファイルの更新（存在しない場合は作成）
        /// </summary>
        /// <param name="configFilePath"></param>
        private void RunBrowser(List<string> urls, YtDlpOptions options)
        {
            if (!options.IsOpenUrl) return;
            foreach (var url in urls)
            {
                OprnUrl(url);
                Thread.Sleep(Math.Max(0, options.BrowserWaitSeconds) * 1000);
            }

        }
        private Process? OprnUrl(string url)
        {
            // ブラウザ起動時に -P "profile_name" を付与する想定
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }
            ProcessStartInfo pi = new ProcessStartInfo();

            string selectedBrowser = comboBox1.SelectedItem as string ?? "Firefox";
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


            //Verb = "runas",           // 必要に応じて管理者実行

            //ProcessStartInfo pi = new ProcessStartInfo()
            //{
            //    FileName = url,
            //    UseShellExecute = true,
            //};

            return Process.Start(pi);
        }

        /// <summary>
        /// 設定ファイル保存ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            SaveSetting();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// フォルダを開くボタン
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("EXPLORER.EXE", Settings.Default.DownloadDirectory);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Open ブラウザボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            OprnUrl("https://www.youtube.com/");
        }

        /// <summary>
        /// Listの１番目を yt-dlp で実行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            // ListBox1 の Items を List<string> に変換
            List<string> urls = listBox1.Items.Cast<string>().ToList();
            string _command = textBoxExePath.Text;
            string _arguments = $" -f bestvideo+bestaudio {urls[0]}";
            try
            {
                Process.Start(_command, _arguments);
            }
            catch (Exception)
            {
                throw;
            }
            listBox1.Items.RemoveAt(0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SelectBrowserProfile(Properties.Settings.Default.SelectBrowserProfile);
        }

        private YtDlpOptions BuildOptionsFromControls()
        {
            int numericValue = (int)numericUpDown1.Value;
            int threads = Math.Max(1, numericValue);
            int waitSeconds = 0;
            if (!int.TryParse(textBox3.Text, out waitSeconds))
            {
                waitSeconds = 0;
            }

            return new YtDlpOptions
            {
                ExePath = textBoxExePath.Text,
                UrlFilePath = textBoxUrlFile.Text,
                DownloadDirectory = textBoxDLFolder.Text,
                IsOpenUrl = checkBox1.Checked,
                BrowserWaitSeconds = waitSeconds,
                DownloadThreads = threads,
                AddDownloaderName = checkBox3.Checked,
                AddVideoId = checkBox2.Checked,
                LimitSize720p = checkBox6.Checked,
                UseCustomPrefix1 = checkBox7.Checked,
                CustomPrefix1 = textBox1.Text,
                UseCustomSuffix1 = checkBox4.Checked,
                CustomSuffix1 = textBox4.Text,
                UseCustomSuffix2 = checkBox5.Checked,
                CustomSuffix2 = textBox5.Text,
                SelectedBrowserProfile = comboBox1.SelectedItem as string ?? "Firefox"
            };
        }

        private void UpdateConfigFile(YtDlpOptions options)
        {
            configFilePath = options.EnsureConfigFilePath();
            if (string.IsNullOrWhiteSpace(configFilePath))
            {
                return;
            }

            string cookiesOption = ytDlpService.GetCookiesOption(options.SelectedBrowserProfile, out profileDir);
            ytDlpService.WriteConfigFile(configFilePath, options, cookiesOption);
            ShowConfigFile(configFilePath);
        }

        private void SelectBrowserProfile(string? profileName)
        {
            if (string.IsNullOrEmpty(profileName))
            {
                comboBox1.SelectedIndex = 0;
                return;
            }

            int index = comboBox1.Items.IndexOf(profileName);
            comboBox1.SelectedIndex = index >= 0 ? index : 0;
        }

    }
}