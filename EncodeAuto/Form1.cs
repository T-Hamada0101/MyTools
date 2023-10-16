using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using RadioButton = System.Windows.Forms.RadioButton;

namespace EncodeAuto
{
    public partial class Form1 : Form
    {

        PresetClass preset;
        List<RadioButton> radioButtons = new List<RadioButton>();

        public Form1()
        {
            InitializeComponent();
            AddDropEvents();

            radioButtons.Add(radioButton1);
            radioButtons.Add(radioButton2);
            radioButtons.Add(radioButton3);
            radioButtons.Add(radioButton4);
            radioButtons.Add(radioButton5);
            radioButtons.Add(radioButton6);
            radioButtons.Add(radioButton7);
            radioButtons.Add(radioButton8);

            preset = XmlSerialize.Load();
            if (preset is null)
            {
                preset = new PresetClass();
                Properties.Settings.Default.Reload();
                BatPath.Text = Properties.Settings.Default.ExePath;
                Arguments.Text = Properties.Settings.Default.Arguments;
                Safix.Text = Properties.Settings.Default.Safix;
                Dir.Text = Properties.Settings.Default.OutputDir;
                MoveComp.Checked = Properties.Settings.Default.MoveComp;
                PauseCMD.Checked = Properties.Settings.Default.Pause;
                PresetName.Text = "default";
                SetPreset(0);
            }

            for (int i = 0; i < radioButtons.Count; i++)
            {
                try
                {
                    string? name = preset.presetName[i];
                    if (name != null)
                    {
                        radioButtons[i].Text = preset.presetName[i].ToString();
                    }
                }
                catch (Exception)
                {
                }
                
            }
            radioButtons[Properties.Settings.Default.ActiveRedioBox].Checked = true;
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
        public void SaveSetting()
        {
            SetPropeties();

            Properties.Settings.Default.Save();
            SetPreset(SelectedRadioButton());
            XmlSerialize.Save(preset);
        }
        internal void SetPropeties()
        {
            Properties.Settings.Default.ExePath = BatPath.Text;
            Properties.Settings.Default.Arguments = Arguments.Text;
            Properties.Settings.Default.Safix = Safix.Text;
            Properties.Settings.Default.OutputDir = Dir.Text;
            Properties.Settings.Default.MoveComp = MoveComp.Checked;
            Properties.Settings.Default.Pause = PauseCMD.Checked;
            Properties.Settings.Default.ActiveRedioBox = SelectedRadioButton();
        }
        internal void SetPreset(int index)
        {
            try
            {
                radioButtons[index].Text = PresetName.Text;
                preset.presetName[index] = PresetName.Text;
                preset.batPath[index] = BatPath.Text;
                preset.argment[index] = Arguments.Text;
                preset.safix[index] = Safix.Text;
                preset.outDir[index] = Dir.Text;
                preset.afterMove[index] = MoveComp.Checked;
                preset.pauseCMD[index] = PauseCMD.Checked;

            }
            catch (Exception)
            {

            }
        }
        internal void LoadPreset(int index)
        {
            if (preset.presetName.Length < index)
            {
                return;
            }
            try
            {
                radioButtons[index].Text = preset.presetName[index];
                PresetName.Text = preset.presetName[index];
                BatPath.Text = preset.batPath[index];
                Arguments.Text = preset.argment[index];
                Safix.Text = preset.safix[index];
                Dir.Text = preset.outDir[index];
                MoveComp.Checked = preset.afterMove[index];
                PauseCMD.Checked = preset.pauseCMD[index];
            }
            catch
            {

            }
            
        }

        private int SelectedRadioButton()
        {
            int index = 0;
            for (int i = 0; i < radioButtons.Count; i++)
            {
                RadioButton rb = radioButtons[i];
                if (rb.Checked)
                {
                    index = i; break;
                }
            }
            return index;
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            //string exePath = textBox1.Text;
            //SaveSetting();
            SetPropeties();
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
            string outDir = Dir.Text;
            Properties.Settings.Default.OutputDir = outDir;
            Properties.Settings.Default.Save();
            AddListboxItem(new string[] { outDir + @"\Input" });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string outDir = Dir.Text;
            Properties.Settings.Default.OutputDir = outDir;
            Properties.Settings.Default.Save();
            FileUtils.CleateDir(outDir);
            if (Directory.Exists(outDir))
            {
                System.Diagnostics.Process.Start("EXPLORER.EXE", outDir);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveSetting();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Pause = PauseCMD.Checked;
            int index = SelectedRadioButton();
            preset.pauseCMD[index] = PauseCMD.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.MoveComp = MoveComp.Checked;
            int index = SelectedRadioButton();
            preset.afterMove[index] = MoveComp.Checked;
        }
        /// <summary>
        /// ラジオボタン排他制御
        /// </summary>
        /// <param name="index"></param>
        private void radioButtonChecked(int index)
        {
            if (radioButtons[index].Checked)
            {
                LoadPreset(index);
            }
            else
            {
                SetPreset(index);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonChecked(0);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonChecked(1);
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonChecked(2);
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonChecked(3);
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonChecked(4);
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonChecked(5);
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonChecked(6);
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonChecked(7);
        }
    }
}