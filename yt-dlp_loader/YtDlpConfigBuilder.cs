using System.Collections.Generic;
using System.IO;

namespace yt_dlp_loader
{
    internal class YtDlpConfigBuilder
    {
        private readonly BrowserCookieOptionBuilder browserCookieOptionBuilder;

        public YtDlpConfigBuilder()
            : this(new BrowserCookieOptionBuilder()) { }

        public YtDlpConfigBuilder(BrowserCookieOptionBuilder browserCookieOptionBuilder)
        {
            this.browserCookieOptionBuilder = browserCookieOptionBuilder;
        }

        public IReadOnlyList<string> Build(YtDlpOptions options, bool includeUrlFile = true)
        {
            var cookiesOption = browserCookieOptionBuilder.Build(options.SelectedBrowserProfile);
            var lines = new List<string>();
            var fileNameTemplate = "%(title)s";

            // 画面設定を順にたどって、yt-dlp.conf の各行を組み立てる
            if (options.UseCustomPrefix1 && !string.IsNullOrWhiteSpace(options.CustomPrefix1))
            {
                fileNameTemplate = options.CustomPrefix1.Trim() + fileNameTemplate;
            }

            if (options.AddDownloaderName)
            {
                fileNameTemplate += "_%(uploader)s";
            }

            if (options.AddVideoId)
            {
                fileNameTemplate += "_%(id)s";
            }

            if (options.UseCustomSuffix1 && !string.IsNullOrWhiteSpace(options.CustomSuffix1))
            {
                fileNameTemplate += options.CustomSuffix1.Trim();
            }

            if (options.UseCustomSuffix2 && !string.IsNullOrWhiteSpace(options.CustomSuffix2))
            {
                fileNameTemplate += options.CustomSuffix2.Trim();
            }

            var outputTemplate = Path.Combine(options.DownloadDirectory, fileNameTemplate);

            lines.Add($@"-o ""{outputTemplate}.%(ext)s""");
            lines.Add("--no-mtime");
            lines.Add("--console-title");

            if (options.DownloadThreads > 1)
            {
                lines.Add($"-N {options.DownloadThreads}");
            }

            if (!string.IsNullOrWhiteSpace(cookiesOption))
            {
                lines.Add(cookiesOption);
            }

            if (includeUrlFile && !string.IsNullOrWhiteSpace(options.UrlFilePath))
            {
                lines.Add($@"-a ""{options.UrlFilePath}""");
            }

            if (options.LimitSize720p)
            {
                lines.Add("--format-sort res:720");
            }

            return lines;
        }
    }
}
