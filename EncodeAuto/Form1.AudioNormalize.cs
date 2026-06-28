using System;
using System.Windows.Forms;

namespace EncodeAuto
{
    public partial class Form1
    {
        private void Form1_AudioNormalize_Load(object? sender, EventArgs e)
        {
            EnsureAudioNormalizePresetLength();

            // 画面表示後に、選択中プリセットの音量最適化状態を反映する
            SyncAudioNormalizeFromPreset();

            // プリセットを切り替えた後も、音量最適化のチェック状態を合わせる
            foreach (RadioButton radioButton in radioButtons)
            {
                radioButton.CheckedChanged -= RadioButton_AudioNormalize_CheckedChanged;
                radioButton.CheckedChanged += RadioButton_AudioNormalize_CheckedChanged;
            }
        }

        private void RadioButton_AudioNormalize_CheckedChanged(object? sender, EventArgs e)
        {
            if (sender is RadioButton radioButton && radioButton.Checked)
            {
                SyncAudioNormalizeFromPreset();
            }
        }

        private void CK_AudioNormalize_CheckedChanged(object? sender, EventArgs e)
        {
            SaveAudioNormalizeToPreset();
        }

        private void SyncAudioNormalizeFromPreset()
        {
            int index = SelectedRadioButton();
            bool value = GetAudioNormalizePresetValue(index);
            if (CK_AudioNormalize.Checked != value)
            {
                CK_AudioNormalize.Checked = value;
            }

            Properties.Settings.Default.AudioNormalize = value;
        }

        private void SaveAudioNormalizeToPreset()
        {
            EnsureAudioNormalizePresetLength();

            int index = SelectedRadioButton();
            if (IsValidAudioNormalizePresetIndex(index))
            {
                preset.audioNormalize[index] = CK_AudioNormalize.Checked;
            }

            Properties.Settings.Default.AudioNormalize = CK_AudioNormalize.Checked;
        }

        private bool GetAudioNormalizePresetValue(int index)
        {
            if (IsValidAudioNormalizePresetIndex(index))
            {
                return preset.audioNormalize[index];
            }

            return Properties.Settings.Default.AudioNormalize;
        }

        private bool IsValidAudioNormalizePresetIndex(int index)
        {
            return preset != null
                && preset.audioNormalize != null
                && index >= 0
                && index < preset.audioNormalize.Length;
        }

        private void EnsureAudioNormalizePresetLength()
        {
            // 共通のプリセット拡張処理に任せて、追加項目だけ取り残されないようにする
            EnsurePresetLength();
        }
    }
}
