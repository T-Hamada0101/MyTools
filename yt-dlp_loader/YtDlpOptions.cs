using System;
using System.IO;

namespace yt_dlp_loader
{
    //設定ファイルは %LOCALAPPDATA%\yt-dlp_loader\yt-dlp.conf に保存されます。
    internal class YtDlpOptions
    {
        public string ExePath { get; init; } = string.Empty;
        public string UrlFilePath { get; init; } = string.Empty;
        public string DownloadDirectory { get; init; } = string.Empty;
        public bool IsOpenUrl { get; init; }
        public int BrowserWaitSeconds { get; init; }
        public int DownloadThreads { get; init; } = 1;
        public bool AddDownloaderName { get; init; }
        public bool AddVideoId { get; init; }
        public bool LimitSize720p { get; init; }
        public bool UseCustomPrefix1 { get; init; }
        public string CustomPrefix1 { get; init; } = string.Empty;
        public bool UseCustomSuffix1 { get; init; }
        public string CustomSuffix1 { get; init; } = string.Empty;
        public bool UseCustomSuffix2 { get; init; }
        public string CustomSuffix2 { get; init; } = string.Empty;
        public string SelectedBrowserProfile { get; init; } = Browser.DefaultProfileName;

        public string EnsureConfigFilePath()
        {
            // ユーザーのAppData\Local\yt-dlp_loaderフォルダに設定ファイルを保存
            //"C:\Users\{user}\AppData\Local\yt-dlp_loader\yt-dlp.conf"
            // これにより、Program Filesなどの保護されたディレクトリを用いず書き込み可能
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var configDir = Path.Combine(appDataPath, "yt-dlp_loader");
            Directory.CreateDirectory(configDir);
            return Path.Combine(configDir, "yt-dlp.conf");
        }
    }
}

