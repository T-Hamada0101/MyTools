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

        //�R���t�B�O�t�@�C���́A�s�x������������
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
            checkBox6.Checked = Properties.Settings.Default.LimitSize720p;
            textBox4.Text = Properties.Settings.Default.AddText1;
            textBox5.Text = Properties.Settings.Default.AddText2;
        }
        public void SaveSetting()
        {
            Properties.Settings.Default.ExePath = textBox1.Text;
            Properties.Settings.Default.UrlFilePath = textBox2.Text;
            Properties.Settings.Default.IsOpenUrl = checkBox1.Checked;
            Properties.Settings.Default.AddDownloaderName = checkBox3.Checked;
            Properties.Settings.Default.AddVideoId = checkBox2.Checked;
            Properties.Settings.Default.LimitSize720p = checkBox6.Checked;
            Properties.Settings.Default.AddText1 = textBox4.Text;
            Properties.Settings.Default.AddText2 = textBox5.Text;

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
            //------------D&D�C�x���g�o�^------------
            this.AllowDrop = true;//�h���b�v���󂯓����
            this.DragEnter += new DragEventHandler(ProcDragEnter);
            this.DragDrop += new DragEventHandler(ProcDragDrop);

        }
        public void ProcDragEnter(object? sender, DragEventArgs e)//D&D�C�x���g
        {
            if (e.Data is null) return;
            ////�h���b�O���ꂽ�f�[�^�`���𒲂� URL�̂ݎ󂯓����
            if (e.Data.GetDataPresent("UniformResourceLocator"))
            {
                e.Effect = DragDropEffects.Link;
            }
        }
        private void ProcDragDrop(object? sender, DragEventArgs e)//D&D�C�x���g
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
            //�h���b�v���ꂽ�̂��t�@�C���̏ꍇ
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var _st = e.Data.GetData(DataFormats.FileDrop);
                string[] strList = (string[])_st;

                url = strList[0];
            }
            else//�e�L�X�g(�����N���܂�)�̏ꍇ
            {
                //�h���b�v���ꂽ�����N��URL���擾����
                url = e.Data.GetData(DataFormats.Text).ToString();
            }
            return url;
        }
        /*
         
         ListView�̃A�C�e���̃e�L�X�g�����[�U�[���ҏW�ł���悤�ɂ���
         https://dobon.net/vb/dotnet/control/lvlabeledit.html
         
         
         */
        private void AddListboxEditEvents()
        {
            //------------�C�x���g�o�^------------
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
                    textBox1.Location = new Point(bounds.X, bounds.Y);
                    textBox1.Size = new Size(bounds.Width, bounds.Height);

                    // Set the TextBox's text to the clicked item's text
                    textBox1.Text = listBox1.Items[index].ToString();

                    // Make the TextBox visible and give it focus
                    textBox1.Visible = true;
                    textBox1.Focus();
                }

            };
        }
        /// <summary>
        /// �񓯊��ŊO���R�}���h���s
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
        private async void button1_Click(object sender, EventArgs e)
        {
            string exePath = textBox1.Text;
            string urlFilePath = textBox2.Text;
            SaveSetting();
            // ListBox1 �� Items �� List<string> �ɕϊ�����
            List<string> urls = listBox1.Items.Cast<string>().ToList();
            WriteUrlFile(urlFilePath, urls);

            int thred = Properties.Settings.Default.DLThreads;
            WriteConfigFile(thred);
            //await RunCommandAsync("cmd.exe",exePath);
            //RunCommand("cmd.exe", exePath);
#if DEBUG
            RunCommand(exePath, null);
            //RunCommandPause(exePath, null);
#else

            RunCommand(exePath, null);
#endif
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
            string OptionsForOutFileName = @"H:\_Youtube\%(title)s";
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
            
            string profileDir = @"dkj5wpar.Suteyo";

            OptionsForOutFileName = @"-o """ + OptionsForOutFileName + @".%(ext)s" + @"""";
            AppendTextToFile(configFilePath, OptionsForOutFileName);
            //AppendTextToFile(configFilePath, @"- N " + thred.ToString());
            AppendTextToFile(configFilePath, @"--no-mtime");
            AppendTextToFile(configFilePath, @"--console-title");
            AppendTextToFile(configFilePath, $@"--cookies-from-browser firefox:{profileDir}");//firefox//chrome
            AppendTextToFile(configFilePath, @"-a ""H:\_yt-dlp_url.txt""");
            if (checkBox6.Checked)
            {
                AppendTextToFile(configFilePath, @"--format-sort res:720");
            }
            AppendTextToFile(configFilePath, @"--remux-video mkv");//���܂������Ȃ�
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
            System.Diagnostics.Process.Start("EXPLORER.EXE", @"H:\_Youtube");
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}