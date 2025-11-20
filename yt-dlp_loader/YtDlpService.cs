using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using yt_dlp_loader.Properties;

namespace yt_dlp_loader
{
    internal class YtDlpService
    {
        public void SaveSettings(YtDlpOptions options)
        {
            var settings = Settings.Default;
            settings.ExePath = options.ExePath;
            settings.UrlFilePath = options.UrlFilePath;
            settings.DownloadDirectory = options.DownloadDirectory;
            settings.IsOpenUrl = options.IsOpenUrl;
            settings.BrowserOpenTime = options.BrowserWaitSeconds;
            settings.DLThreads = options.DownloadThreads;
            settings.AddDownloaderName = options.AddDownloaderName;
            settings.AddVideoId = options.AddVideoId;
            settings.LimitSize720p = options.LimitSize720p;
            settings.AddText1 = options.CustomSuffix1;
            settings.AddText2 = options.CustomSuffix2;
            settings.SelectBrowserProfile = options.SelectedBrowserProfile;
            settings.Save();
        }

        public void WriteUrlFile(string urlFilePath, IEnumerable<string> urls)
        {
            if (string.IsNullOrWhiteSpace(urlFilePath))
            {
                return;
            }

            EnsureDirectoryExists(urlFilePath);
            File.WriteAllLines(urlFilePath, urls);
        }

        public void RunYtDlp(string exePath, string? arguments = null)
        {
            if (string.IsNullOrWhiteSpace(exePath))
            {
                return;
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = exePath,
                UseShellExecute = true,
            };

            if (!string.IsNullOrWhiteSpace(arguments))
            {
                startInfo.Arguments = arguments!;
            }

            Process.Start(startInfo);
        }

        public void WriteConfigFile(string configFilePath, YtDlpOptions options, string cookiesOption)
        {
            if (string.IsNullOrWhiteSpace(configFilePath))
            {
                return;
            }

            EnsureDirectoryExists(configFilePath);
            var lines = BuildConfigLines(options, cookiesOption);
            File.WriteAllLines(configFilePath, lines);
        }

        public string GetCookiesOption(string? selectedBrowser, out string profileDir)
        {
            string browser = "firefox";
            profileDir = @"dkj5wpar.Suteyo";

            switch (selectedBrowser)
            {
                case "Firefox":
                    browser = "firefox";
                    profileDir = @"19cp24ya.default-release-1";
                    break;
                case "Firefox PPP":
                    browser = "firefox";
                    profileDir = @"dkj5wpar.Suteyo";
                    break;
                case "Chrome":
                    browser = "chrome";
                    profileDir = @"Profile 2";
                    break;
            }

            return $@"--cookies-from-browser {browser}:{profileDir}";
        }

        private static IEnumerable<string> BuildConfigLines(YtDlpOptions options, string cookiesOption)
        {
            var lines = new List<string>();
            var outputTemplate = Path.Combine(options.DownloadDirectory, "%(title)s");

            if (options.AddDownloaderName)
            {
                outputTemplate += "_%(uploader)s";
            }

            if (options.AddVideoId)
            {
                outputTemplate += "_%(id)s";
            }

            if (options.UseCustomSuffix1 && !string.IsNullOrWhiteSpace(options.CustomSuffix1))
            {
                outputTemplate += options.CustomSuffix1.Trim();
            }

            if (options.UseCustomSuffix2 && !string.IsNullOrWhiteSpace(options.CustomSuffix2))
            {
                outputTemplate += options.CustomSuffix2.Trim();
            }

            lines.Add($@"-o ""{outputTemplate}.%(ext)s""");
            lines.Add("--no-mtime");
            lines.Add("--console-title");

            if (!string.IsNullOrWhiteSpace(cookiesOption))
            {
                lines.Add(cookiesOption);
            }

            lines.Add($@"-a ""{options.UrlFilePath}""");

            if (options.LimitSize720p)
            {
                lines.Add("--format-sort res:720");
            }

            return lines;
        }

        private static void EnsureDirectoryExists(string filePath)
        {
            var directory = Path.GetDirectoryName(filePath);
            if (string.IsNullOrWhiteSpace(directory))
            {
                return;
            }

            Directory.CreateDirectory(directory);
        }
    }
}

