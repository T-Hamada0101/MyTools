using System;
using System.IO;

namespace yt_dlp_loader
{
    internal class AppRuntimePaths
    {
        public string ApplicationDirectoryPath => AppContext.BaseDirectory;

        public string SettingsDirectoryPath =>
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "yt-dlp_loader"
            );

        public string BrowserProfilesFilePath =>
            Path.Combine(ApplicationDirectoryPath, "browser_profiles.json");

        public string AppSettingsFilePath => Path.Combine(SettingsDirectoryPath, "app-settings.json");

        public string MainConfigFilePath => Path.Combine(SettingsDirectoryPath, "yt-dlp.conf");

        public string SingleRunConfigFilePath => Path.Combine(SettingsDirectoryPath, "yt-dlp.single.conf");

        public void EnsureSettingsDirectoryExists()
        {
            // 設定と config を置く作業フォルダはここでまとめて用意する
            Directory.CreateDirectory(SettingsDirectoryPath);
        }
    }
}
