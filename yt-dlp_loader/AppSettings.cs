namespace yt_dlp_loader
{
    internal class AppSettings
    {
        public string ExePath { get; init; } = string.Empty;
        public string UrlFilePath { get; init; } = string.Empty;
        public string DownloadDirectory { get; init; } = string.Empty;
        public bool IsOpenUrl { get; init; }
        public int BrowserOpenTime { get; init; } = 5;
        public int DLThreads { get; init; } = 1;
        public bool AddDownloaderName { get; init; }
        public bool AddVideoId { get; init; }
        public bool LimitSize720p { get; init; }
        public bool UseCustomPrefix1 { get; init; }
        public string AddPrefix1 { get; init; } = string.Empty;
        public bool UseCustomSuffix1 { get; init; }
        public string AddText1 { get; init; } = string.Empty;
        public bool UseCustomSuffix2 { get; init; }
        public string AddText2 { get; init; } = string.Empty;
        public string SelectBrowserProfile { get; init; } = string.Empty;

        public YtDlpOptions ToYtDlpOptions()
        {
            return new YtDlpOptions
            {
                ExePath = ExePath,
                UrlFilePath = UrlFilePath,
                DownloadDirectory = DownloadDirectory,
                IsOpenUrl = IsOpenUrl,
                BrowserWaitSeconds = BrowserOpenTime,
                DownloadThreads = DLThreads,
                AddDownloaderName = AddDownloaderName,
                AddVideoId = AddVideoId,
                LimitSize720p = LimitSize720p,
                UseCustomPrefix1 = UseCustomPrefix1,
                CustomPrefix1 = AddPrefix1,
                UseCustomSuffix1 = UseCustomSuffix1,
                CustomSuffix1 = AddText1,
                UseCustomSuffix2 = UseCustomSuffix2,
                CustomSuffix2 = AddText2,
                SelectedBrowserProfile = SelectBrowserProfile
            };
        }
    }
}
