using System;
using System.Diagnostics;
using System.Threading.Tasks;
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
            textBox2.Text = Properties.Settings.Default.UrlFilePath;
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
        private async void button1_Click(object sender, EventArgs e)
        {
            string exePath = textBox1.Text;
            string urlFilePath = textBox2.Text;
            Properties.Settings.Default.ExePath = textBox1.Text;
            Properties.Settings.Default.UrlFilePath = textBox2.Text;
            Properties.Settings.Default.Save();
            ClearFile(urlFilePath);

            foreach (string item in listBox1.Items)
            {
                AppendTextToFile(urlFilePath, item);
            }

            //await RunCommandAsync("cmd.exe",exePath);
            //RunCommand("cmd.exe", exePath);
            RunCommand(exePath,null);
            listBox1.Items.Clear();
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
    }
}