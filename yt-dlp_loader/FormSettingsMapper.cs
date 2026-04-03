using System;
using System.Windows.Forms;

namespace yt_dlp_loader
{
    internal class FormSettingsMapper
    {
        private readonly TextBox textBoxExePath;
        private readonly TextBox textBoxUrlFile;
        private readonly TextBox textBoxDownloadDirectory;
        private readonly CheckBox checkBoxOpenUrl;
        private readonly TextBox textBoxBrowserOpenTime;
        private readonly NumericUpDown numericUpDownThreads;
        private readonly CheckBox checkBoxAddDownloaderName;
        private readonly CheckBox checkBoxAddVideoId;
        private readonly CheckBox checkBoxLimitSize720p;
        private readonly CheckBox checkBoxUseCustomPrefix1;
        private readonly TextBox textBoxCustomPrefix1;
        private readonly CheckBox checkBoxUseCustomSuffix1;
        private readonly TextBox textBoxCustomSuffix1;
        private readonly CheckBox checkBoxUseCustomSuffix2;
        private readonly TextBox textBoxCustomSuffix2;
        private readonly ComboBox comboBoxBrowserProfiles;
        private readonly Func<string?, string> resolveBrowserProfileName;

        public FormSettingsMapper(
            TextBox textBoxExePath,
            TextBox textBoxUrlFile,
            TextBox textBoxDownloadDirectory,
            CheckBox checkBoxOpenUrl,
            TextBox textBoxBrowserOpenTime,
            NumericUpDown numericUpDownThreads,
            CheckBox checkBoxAddDownloaderName,
            CheckBox checkBoxAddVideoId,
            CheckBox checkBoxLimitSize720p,
            CheckBox checkBoxUseCustomPrefix1,
            TextBox textBoxCustomPrefix1,
            CheckBox checkBoxUseCustomSuffix1,
            TextBox textBoxCustomSuffix1,
            CheckBox checkBoxUseCustomSuffix2,
            TextBox textBoxCustomSuffix2,
            ComboBox comboBoxBrowserProfiles,
            Func<string?, string> resolveBrowserProfileName
        )
        {
            this.textBoxExePath = textBoxExePath;
            this.textBoxUrlFile = textBoxUrlFile;
            this.textBoxDownloadDirectory = textBoxDownloadDirectory;
            this.checkBoxOpenUrl = checkBoxOpenUrl;
            this.textBoxBrowserOpenTime = textBoxBrowserOpenTime;
            this.numericUpDownThreads = numericUpDownThreads;
            this.checkBoxAddDownloaderName = checkBoxAddDownloaderName;
            this.checkBoxAddVideoId = checkBoxAddVideoId;
            this.checkBoxLimitSize720p = checkBoxLimitSize720p;
            this.checkBoxUseCustomPrefix1 = checkBoxUseCustomPrefix1;
            this.textBoxCustomPrefix1 = textBoxCustomPrefix1;
            this.checkBoxUseCustomSuffix1 = checkBoxUseCustomSuffix1;
            this.textBoxCustomSuffix1 = textBoxCustomSuffix1;
            this.checkBoxUseCustomSuffix2 = checkBoxUseCustomSuffix2;
            this.textBoxCustomSuffix2 = textBoxCustomSuffix2;
            this.comboBoxBrowserProfiles = comboBoxBrowserProfiles;
            this.resolveBrowserProfileName = resolveBrowserProfileName;
        }

        public AppSettings Build()
        {
            int waitSeconds = ParseBrowserOpenTime();
            int threads = Math.Max(1, (int)numericUpDownThreads.Value);

            // 画面の入力値を 1 か所で AppSettings へまとめる
            return new AppSettings
            {
                ExePath = textBoxExePath.Text,
                UrlFilePath = textBoxUrlFile.Text,
                DownloadDirectory = textBoxDownloadDirectory.Text,
                IsOpenUrl = checkBoxOpenUrl.Checked,
                BrowserOpenTime = waitSeconds,
                DLThreads = threads,
                AddDownloaderName = checkBoxAddDownloaderName.Checked,
                AddVideoId = checkBoxAddVideoId.Checked,
                LimitSize720p = checkBoxLimitSize720p.Checked,
                UseCustomPrefix1 = checkBoxUseCustomPrefix1.Checked,
                AddPrefix1 = textBoxCustomPrefix1.Text,
                UseCustomSuffix1 = checkBoxUseCustomSuffix1.Checked,
                AddText1 = textBoxCustomSuffix1.Text,
                UseCustomSuffix2 = checkBoxUseCustomSuffix2.Checked,
                AddText2 = textBoxCustomSuffix2.Text,
                SelectBrowserProfile = resolveBrowserProfileName(comboBoxBrowserProfiles.SelectedItem as string)
            };
        }

        public void Apply(AppSettings appSettings)
        {
            // 保存済み設定をまとめて画面へ戻し、反映漏れを防ぐ
            textBoxExePath.Text = appSettings.ExePath;
            textBoxUrlFile.Text = appSettings.UrlFilePath;
            textBoxDownloadDirectory.Text = appSettings.DownloadDirectory;
            checkBoxOpenUrl.Checked = appSettings.IsOpenUrl;
            textBoxBrowserOpenTime.Text = appSettings.BrowserOpenTime.ToString();
            numericUpDownThreads.Value = NormalizeThreadCount(appSettings.DLThreads);
            checkBoxAddDownloaderName.Checked = appSettings.AddDownloaderName;
            checkBoxAddVideoId.Checked = appSettings.AddVideoId;
            checkBoxLimitSize720p.Checked = appSettings.LimitSize720p;
            checkBoxUseCustomPrefix1.Checked = appSettings.UseCustomPrefix1;
            textBoxCustomPrefix1.Text = appSettings.AddPrefix1;
            checkBoxUseCustomSuffix1.Checked = appSettings.UseCustomSuffix1;
            textBoxCustomSuffix1.Text = appSettings.AddText1;
            checkBoxUseCustomSuffix2.Checked = appSettings.UseCustomSuffix2;
            textBoxCustomSuffix2.Text = appSettings.AddText2;
            SelectBrowserProfile(appSettings.SelectBrowserProfile);
        }

        private int ParseBrowserOpenTime()
        {
            if (!int.TryParse(textBoxBrowserOpenTime.Text, out int waitSeconds))
            {
                return 0;
            }

            return waitSeconds;
        }

        private decimal NormalizeThreadCount(int threads)
        {
            return Math.Max(numericUpDownThreads.Minimum, Math.Min(numericUpDownThreads.Maximum, threads));
        }

        private void SelectBrowserProfile(string? profileName)
        {
            if (comboBoxBrowserProfiles.Items.Count == 0)
            {
                comboBoxBrowserProfiles.SelectedIndex = -1;
                return;
            }

            string targetProfile = resolveBrowserProfileName(profileName);
            if (string.IsNullOrEmpty(targetProfile))
            {
                comboBoxBrowserProfiles.SelectedIndex = 0;
                return;
            }

            int index = comboBoxBrowserProfiles.Items.IndexOf(targetProfile);
            comboBoxBrowserProfiles.SelectedIndex = index >= 0 ? index : 0;
        }
    }
}
