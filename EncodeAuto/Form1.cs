using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Drawing;
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
            BindOptionalButtonEvents();
            InitializePresetButtons();

            PresetClass? loadedPreset = XmlSerialize.Load();


            if (loadedPreset != null)
            {
                preset = loadedPreset;
                EnsurePresetLength();
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
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        radioButtons[i].Text = name;
                    }
                }
                catch (Exception)
                {
                }

            }
            int activeIndex = Properties.Settings.Default.ActiveRedioBox;
            if (activeIndex < 0 || activeIndex >= radioButtons.Count)
            {
                activeIndex = 0;
            }
            radioButtons[activeIndex].Checked = true;
        }

        private void InitializePresetButtons()
        {
            groupBox1.SuspendLayout();
            groupBox1.Controls.Clear();
            groupBox1.Size = new Size(488, 224);
            radioButtons.Clear();

            // プリセット枠は数を一か所に寄せて、UIと保存データがずれないように作る
            for (int i = 0; i < PresetClass.PresetCount; i++)
            {
                int index = i;
                RadioButton radioButton = new RadioButton
                {
                    AutoSize = false,
                    Location = new Point(18 + (index / 6) * 112, 32 + (index % 6) * 30),
                    Margin = new Padding(4, 4, 4, 4),
                    Name = $"radioButton{index + 1}",
                    Size = new Size(104, 28),
                    TabIndex = 16 + index,
                    TabStop = true,
                    Text = (index + 1).ToString(),
                    UseVisualStyleBackColor = true,
                };
                radioButton.CheckedChanged += (sender, e) => radioButtonChecked(index);

                groupBox1.Controls.Add(radioButton);
                radioButtons.Add(radioButton);
            }

            groupBox1.ResumeLayout(false);
        }

        private void EnsurePresetLength()
        {
            if (preset == null)
            {
                return;
            }

            int length = Math.Max(PresetClass.PresetCount, radioButtons.Count);

            // 古いPreset.xmlを読み込んでも、既存値を残したまま不足分だけ増やす
            preset.presetName = ResizeStringArray(preset.presetName, length);
            preset.batPath = ResizeStringArray(preset.batPath, length);
            preset.argment = ResizeStringArray(preset.argment, length);
            preset.safix = ResizeStringArray(preset.safix, length);
            preset.outDir = ResizeStringArray(preset.outDir, length);
            preset.afterOriginMove = ResizeBoolArray(preset.afterOriginMove, length);
            preset.pauseCMD = ResizeBoolArray(preset.pauseCMD, length);
            preset.sameDirOutput = ResizeBoolArray(preset.sameDirOutput, length);
            preset.shortFileName = ResizeBoolArray(preset.shortFileName, length);
            preset.audioNormalize = ResizeBoolArray(preset.audioNormalize, length);
        }

        private string[] ResizeStringArray(string[]? values, int length)
        {
            string[] resized = new string[length];
            if (values != null)
            {
                Array.Copy(values, resized, Math.Min(values.Length, length));
            }

            return resized;
        }

        private bool[] ResizeBoolArray(bool[]? values, int length)
        {
            bool[] resized = new bool[length];
            if (values != null)
            {
                Array.Copy(values, resized, Math.Min(values.Length, length));
            }

            return resized;
        }

        private void BindOptionalButtonEvents()
        {
            Control[] controls = Controls.Find("buttonFileName", true);
            if (controls.Length > 0 && controls[0] is System.Windows.Forms.Button button)
            {
                button.Click -= buttonFileName_Click;
                button.Click += buttonFileName_Click;
            }
        }


        #region D&Dイベント
        private void AddDropEvents()
        {
            //------------D&Dイベント登録------------
            this.AllowDrop = true;//ドロップを受け入れる
            this.DragEnter += new DragEventHandler(ProcDragEnter);
            this.DragDrop += new DragEventHandler(ProcDragDrop);
            //------------D&Dイベント登録------------
            listBoxMerge.AllowDrop = true;//ドロップを受け入れる
            listBoxMerge.DragEnter += new DragEventHandler(ProcDragEnter);
            listBoxMerge.DragDrop += new DragEventHandler(ProcDragDrop);

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
            object? _sender = sender;
            if (_sender is ListBox)
            {
                AddListboxItem(_sender, files);
            }
            else//form1
            {
                AddListboxItem(listBox1, files);
            }
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
                pass[0] = e.Data.GetData(DataFormats.Text)?.ToString() ?? string.Empty;
            }
            return pass;
        }
        #endregion

        /// <summary>
        /// パスをListboxに重複を許さずに追加（フォルダ内も検索）
        /// </summary>
        /// <param name="files"></param>
        private void AddListboxItem(object sender, string[] files)
        {
            ListBox listBox = (ListBox)sender;
            // ListBox の 既存Items を HashSet に変換する
            List<string> list = listBox.Items.Cast<string>().ToList();
            HashSet<string> hashSet = new HashSet<string>(list);

            //引数分を追加
            List<string> lists = FileUtils.GetAllFilePath(files);
            foreach (string _file in lists)
            {
                if (!FileUtils.IsMovieFile(_file)) {
                    if (FileUtils.IsAudioFile(_file))
                    {
                        if (listBox.Name == "listBoxMerge")
                        {
                            hashSet.Add(_file);
                        }else if (MessageBox.Show("音声ファイルですが追加しますか？", "確認", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            hashSet.Add(_file);
                        }
                        else { continue; }
                    }
                    else { continue; }
                }else { hashSet.Add(_file); }
            }

            if (hashSet.Count > 0)
            {
                listBox.Items.Clear();
                listBox.Items.AddRange(hashSet.ToArray());
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
            if (!IsValidPresetIndex(index))
            {
                return;
            }

            string presetName = PresetName.Text;
            radioButtons[index].Text = string.IsNullOrWhiteSpace(presetName)
                ? (index + 1).ToString()
                : presetName;

            preset.presetName[index] = presetName;
            preset.batPath[index] = BatPath.Text;
            preset.argment[index] = Arguments.Text;
            preset.safix[index] = Safix.Text;
            preset.outDir[index] = Dir.Text;
            preset.afterOriginMove[index] = CK_MoveComp.Checked;
            preset.pauseCMD[index] = CK_PauseCMD.Checked;
            preset.sameDirOutput[index] = CK_OutSameDir.Checked;
            preset.shortFileName[index] = CK_ShortFileName.Checked;
            preset.audioNormalize[index] = CK_AudioNormalize.Checked;
        }

        private bool IsValidPresetIndex(int index)
        {
            return preset != null
                && index >= 0
                && index < radioButtons.Count
                && index < preset.presetName.Length;
        }

        private string GetPresetString(string[] values, int index)
        {
            if (index >= 0 && index < values.Length)
            {
                return values[index] ?? string.Empty;
            }

            return string.Empty;
        }

        internal void LoadPreset(int index)
        {
            if (!IsValidPresetIndex(index))
            {
                return;
            }

            string presetName = GetPresetString(preset.presetName, index);
            radioButtons[index].Text = string.IsNullOrWhiteSpace(presetName)
                ? (index + 1).ToString()
                : presetName;
            PresetName.Text = presetName;
            BatPath.Text = GetPresetString(preset.batPath, index);
            Arguments.Text = GetPresetString(preset.argment, index);
            Safix.Text = GetPresetString(preset.safix, index);
            Dir.Text = GetPresetString(preset.outDir, index);
            CK_MoveComp.Checked = preset.afterOriginMove[index];
            CK_PauseCMD.Checked = preset.pauseCMD[index];
            CK_OutSameDir.Checked = preset.sameDirOutput[index];
            CK_ShortFileName.Checked = preset.shortFileName[index];
            CK_AudioNormalize.Checked = preset.audioNormalize[index];
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
            List<string> allList = listBox1.Items.Cast<string>().ToList();

            //分割セッション数
            int threadNum = (int)numericUpDown1.Value;
            if (threadNum > allList.Count)
            {
                threadNum = allList.Count;
            }
            //セッション数分のリストを作成
            List<List<string>> sessions = new List<List<string>>();
            for (int i = 0; i < threadNum; i++)
            {
                sessions.Add(new List<string>());
            }
            //分割
            int count = 0;
            foreach (string _file in allList)
            {
                sessions[count].Add(_file);
                count++;
                if (count >= threadNum)
                {
                    count = 0;
                }
            }
            //listを実行
            for (int i = 0; i < sessions.Count; i++)
            {
                List<string> items = sessions[i];
                EncodeDeta deta = new EncodeDeta(i, items);
                Task.Run(() => new Encoder(deta));
                //listBolからitemsと同じ文字列を削除
                foreach (string _file in items)
                {
                    listBox1.Items.Remove(_file);
                }
            }


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
            AddListboxItem(listBox1,new string[] { outDir + @"\Input" });
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

        private void CK_ShortFileName_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ShortFileName = CK_ShortFileName.Checked;
            int index = SelectedRadioButton();
            preset.shortFileName[index] = CK_ShortFileName.Checked;
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

        /// <summary>
        /// CUT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //string exePath = textBox1.Text;
            //SaveSetting();
            SetPropeties();
            // ListBox1 の Items を List<string> に変換する
            List<string> allList = listBox1.Items.Cast<string>().ToList();

            //分割セッション数
            int threadNum = 1;
            //セッション数分のリストを作成
            List<List<string>> sessions = new List<List<string>>();
            for (int i = 0; i < threadNum; i++)
            {
                sessions.Add(new List<string>());
            }
            //分割
            int count = 0;
            foreach (string _file in allList)
            {
                sessions[count].Add(_file);
                count++;
                if (count >= threadNum)
                {
                    count = 0;
                }
            }
            //listを実行
            for (int i = 0; i < sessions.Count; i++)
            {
                List<string> items = sessions[i];
                List<string> cutTimes = new List<string>();
                string[] times = textBox1.Text.Split("\r\n");
                foreach (string time in times)
                {
                    cutTimes.Add(time);
                }
                EncodeDeta deta = new EncodeDeta(i, items, false,false, true, cutTimes);
                Task.Run(() => new Encoder(deta));
                //listBolからitemsと同じ文字列を削除
                foreach (string _file in items)
                {
                    listBox1.Items.Remove(_file);
                }
            }
        }

        /// <summary>
        /// marge 映像と音声をマージ（1setのみ処理）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            SetPropeties();
            // ListBox1 の Items を List<string> に変換する
            List<string> allList = listBoxMerge.Items.Cast<string>().ToList();

            //リストに映像ファイルsession[0]、音声ファイルsession[1]を1set追加
            List<string> session = new List<string>();
            //リストに映像ファイルを追加
            for (int i = 0; i < allList.Count; i++)
            {
                //映像ファイルなら
                if(FileUtils.IsMovieFile(allList[i])){
                    session.Add(allList[i]);
                    break;
                }
            }
            //リストに音声ファイルを追加
            for (int i = 0; i < allList.Count; i++)
            {
                //音声ファイルなら
                if (FileUtils.IsAudioFile(allList[i]))
                {
                    session.Add(allList[i]);
                    break;
                }
            }

            //"映像ファイル名:音声ファイル名"の形式でitemsに追加
            List<string> items = new List<string>();
            items.Add(session[0] + ":::" + session[1]);
            string tmp = items[0];
            Console.WriteLine(tmp);
            EncodeDeta deta = new EncodeDeta(0, items, true, true);
            Task.Run(() => new Encoder(deta));
            //listBolをクリア
            listBoxMerge.Items.Clear();
        }

        private void buttonFileName_Click(object? sender, EventArgs e)
        {
            string prefix = GetControlTextByName("textBoxPrefix");
            string safix = GetControlTextByName("textBoxSafix");

            List<string> files = listBox1.Items.Cast<object>()
                .Select(x => x?.ToString())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Cast<string>()
                .ToList();

            List<string> processed = new List<string>();
            List<string> errors = new List<string>();

            foreach (string filePath in files)
            {
                try
                {
                    if (!File.Exists(filePath))
                    {
                        errors.Add($"File not found: {filePath}");
                        continue;
                    }

                    string? dir = Path.GetDirectoryName(filePath);
                    if (string.IsNullOrWhiteSpace(dir))
                    {
                        errors.Add($"Directory not found: {filePath}");
                        continue;
                    }

                    string baseName = Path.GetFileNameWithoutExtension(filePath);
                    string ext = Path.GetExtension(filePath);
                    string newPath = Path.Combine(dir, $"{prefix}{baseName}{safix}{ext}");

                    if (!string.Equals(filePath, newPath, StringComparison.OrdinalIgnoreCase))
                    {
                        if (File.Exists(newPath))
                        {
                            errors.Add($"Already exists: {newPath}");
                            continue;
                        }
                        File.Move(filePath, newPath);
                    }

                    processed.Add(filePath);
                }
                catch (Exception ex)
                {
                    errors.Add($"{filePath} -> {ex.Message}");
                }
            }

            foreach (string filePath in processed)
            {
                listBox1.Items.Remove(filePath);
            }

            if (errors.Count > 0)
            {
                MessageBox.Show(string.Join(Environment.NewLine, errors), "ファイル名変更エラー");
            }
        }

        private string GetControlTextByName(string controlName)
        {
            Control[] controls = Controls.Find(controlName, true);
            if (controls.Length == 0)
            {
                return string.Empty;
            }
            return controls[0].Text ?? string.Empty;
        }
    }
}
