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
            radioButtons.Add(radioButton9);
            radioButtons.Add(radioButton10);
            radioButtons.Add(radioButton11);
            radioButtons.Add(radioButton12);

            preset = XmlSerialize.Load();


            if (preset != null)
            {
                //フォームのプリセットを増やした直後に起動した場合に配列の長さが足りないので増やす
                if (radioButtons.Count > preset.presetName.Length)
                {
                    PresetClass tmp = new PresetClass();
                    tmp.presetName = new string[radioButtons.Count];
                    tmp.batPath = new string[radioButtons.Count];
                    tmp.argment = new string[radioButtons.Count];
                    tmp.safix = new string[radioButtons.Count];
                    tmp.outDir = new string[radioButtons.Count];
                    tmp.afterOriginMove = new bool[radioButtons.Count];
                    tmp.pauseCMD = new bool[radioButtons.Count];
                    tmp.sameDirOutput = new bool[radioButtons.Count];
                    for (int i = 0; i < preset.presetName.Length; i++)
                    {
                        tmp.presetName[i] = preset.presetName[i];
                        tmp.batPath[i] = preset.batPath[i];
                        tmp.argment[i] = preset.argment[i];
                        tmp.safix[i] = preset.safix[i];
                        tmp.outDir[i] = preset.outDir[i];
                        tmp.afterOriginMove[i] = preset.afterOriginMove[i];
                        tmp.pauseCMD[i] = preset.pauseCMD[i];
                        tmp.sameDirOutput[i] = preset.sameDirOutput[i];
                    }
                    preset = tmp;
                }
            }
            else
            {   //初回起動時
                preset = new PresetClass();
                Properties.Settings.Default.Reload();
                BatPath.Text = Properties.Settings.Default.ExePath;
                Arguments.Text = Properties.Settings.Default.Arguments;
                Safix.Text = Properties.Settings.Default.Safix;
                Dir.Text = Properties.Settings.Default.OutputDir;
                CK_MoveComp.Checked = Properties.Settings.Default.MoveComp;
                CK_PauseCMD.Checked = Properties.Settings.Default.Pause;
                CK_OutSameDir.Checked = Properties.Settings.Default.SameDirOutput;
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
            Properties.Settings.Default.MoveComp = CK_MoveComp.Checked;
            Properties.Settings.Default.Pause = CK_PauseCMD.Checked;
            Properties.Settings.Default.SameDirOutput = CK_OutSameDir.Checked;
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
                preset.afterOriginMove[index] = CK_MoveComp.Checked;
                preset.pauseCMD[index] = CK_PauseCMD.Checked;
                preset.sameDirOutput[index] = CK_OutSameDir.Checked;
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
                CK_MoveComp.Checked = preset.afterOriginMove[index];
                CK_PauseCMD.Checked = preset.pauseCMD[index];
                CK_OutSameDir.Checked = preset.sameDirOutput[index];
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
        private async void BT_start_Click(object sender, EventArgs e)
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
        private void BT_addInputDir_Click(object sender, EventArgs e)
        {
            string outDir = Dir.Text;
            Properties.Settings.Default.OutputDir = outDir;
            Properties.Settings.Default.Save();
            AddListboxItem(new string[] { outDir + @"\Input" });
        }

        private void BT_open_Click(object sender, EventArgs e)
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

        private void BT_Save_Click(object sender, EventArgs e)
        {
            SaveSetting();
        }

        private void CK_PauseCMD_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Pause = CK_PauseCMD.Checked;
            int index = SelectedRadioButton();
            preset.pauseCMD[index] = CK_PauseCMD.Checked;
        }

        private void CK_MoveComp_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.MoveComp = CK_MoveComp.Checked;
            int index = SelectedRadioButton();
            preset.afterOriginMove[index] = CK_MoveComp.Checked;
        }
        private void CK_OutSameDir_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.SameDirOutput = CK_OutSameDir.Checked;
            int index = SelectedRadioButton();
            preset.sameDirOutput[index] = CK_OutSameDir.Checked;
        }

        #region ラジオボタン
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

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonChecked(8);
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonChecked(9);
        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonChecked(10);
        }

        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonChecked(11);
        }
        #endregion
    }
}