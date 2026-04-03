namespace yt_dlp_loader
{
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
        public string SelectedBrowserProfile { get; init; } = string.Empty;
    }
}
