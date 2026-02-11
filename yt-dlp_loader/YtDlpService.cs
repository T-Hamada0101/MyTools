using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
            settings.AddPrefix1 = options.CustomPrefix1;
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

        public void RunYtDlp(YtDlpOptions options, string? additionalArguments = null, Action<string>? outputHandler = null, Action<string>? errorHandler = null)
        {
            if (string.IsNullOrWhiteSpace(options.ExePath))
            {
                return;
            }

            // ユーザーAppDataの設定ファイルを指定
            var configFilePath = options.EnsureConfigFilePath();
            var arguments = $@"--config-location ""{configFilePath}""";

            if (!string.IsNullOrWhiteSpace(additionalArguments))
            {
                arguments += " " + additionalArguments;
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = options.ExePath,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };

            // yt-dlpがUTF-8で出力するように環境変数を設定
            startInfo.EnvironmentVariables["PYTHONIOENCODING"] = "utf-8";
            startInfo.EnvironmentVariables["PYTHONLEGACYWINDOWSSTDIO"] = "0";
            // Windows 10以降でコンソールのコードページをUTF-8に設定
            startInfo.EnvironmentVariables["PYTHONUTF8"] = "1";

            var process = new Process
            {
                StartInfo = startInfo
            };

            if (outputHandler != null)
            {
                process.OutputDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        outputHandler(e.Data);
                    }
                };
            }

            if (errorHandler != null)
            {
                process.ErrorDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        errorHandler(e.Data);
                    }
                };
            }

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            // 非同期で実行（UIをブロックしない）
            Task.Run(() =>
            {
                process.WaitForExit();
                process.Dispose();
            });
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

        public string GetCookiesOption(string? selectedBrowser) => Browser.GetCookiesOption(selectedBrowser);

        private static IEnumerable<string> BuildConfigLines(YtDlpOptions options, string cookiesOption)
        {
            var lines = new List<string>();
            var fileNameTemplate = "%(title)s";

            // プレフィックスを先頭に追加
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

