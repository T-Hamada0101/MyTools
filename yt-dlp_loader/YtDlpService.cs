using System;
using System.Collections.Generic;
using System.IO;

namespace yt_dlp_loader
{
    internal class YtDlpService
    {
        private readonly AppRuntimePaths appRuntimePaths;
        private readonly YtDlpConfigBuilder ytDlpConfigBuilder;
        private readonly ProcessLauncher processLauncher;

        public YtDlpService()
            : this(new AppRuntimePaths(), new YtDlpConfigBuilder(), new ProcessLauncher()) { }

        public YtDlpService(
            AppRuntimePaths appRuntimePaths,
            YtDlpConfigBuilder ytDlpConfigBuilder,
            ProcessLauncher processLauncher
        )
        {
            this.appRuntimePaths = appRuntimePaths;
            this.ytDlpConfigBuilder = ytDlpConfigBuilder;
            this.processLauncher = processLauncher;
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

        public void RunYtDlp(
            YtDlpOptions options,
            string? additionalArguments = null,
            Action<string>? outputHandler = null,
            Action<string>? errorHandler = null,
            string? configFilePath = null
        )
        {
            if (string.IsNullOrWhiteSpace(options.ExePath))
            {
                return;
            }

            // 実行時は指定された config を優先し、無ければ通常 config を使う
            var resolvedConfigFilePath = string.IsNullOrWhiteSpace(configFilePath)
                ? appRuntimePaths.MainConfigFilePath
                : configFilePath;
            var arguments = $@"--config-location ""{resolvedConfigFilePath}""";

            if (!string.IsNullOrWhiteSpace(additionalArguments))
            {
                arguments += " " + additionalArguments;
            }

            var startInfo = processLauncher.CreateUtf8ConsoleProcessStartInfo(
                options.ExePath,
                arguments
            );
            processLauncher.StartAndMonitor(startInfo, outputHandler, errorHandler);
        }

        public string PrepareConfigFile(
            YtDlpOptions options,
            bool includeUrlFile = true,
            string? configFilePath = null
        )
        {
            var resolvedConfigFilePath = string.IsNullOrWhiteSpace(configFilePath)
                ? appRuntimePaths.MainConfigFilePath
                : configFilePath;
            if (string.IsNullOrWhiteSpace(resolvedConfigFilePath))
            {
                return string.Empty;
            }

            EnsureDirectoryExists(resolvedConfigFilePath);
            var lines = ytDlpConfigBuilder.Build(options, includeUrlFile);
            File.WriteAllLines(resolvedConfigFilePath, lines);
            return resolvedConfigFilePath;
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
