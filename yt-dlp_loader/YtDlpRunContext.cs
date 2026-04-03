namespace yt_dlp_loader
{
    internal class YtDlpRunContext
    {
        public YtDlpOptions Options { get; init; } = new();
        public string ConfigFilePath { get; init; } = string.Empty;
    }
}
