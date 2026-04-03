using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace yt_dlp_loader
{
    internal class YtDlpRunner
    {
        private readonly AppSettingsStore appSettingsStore;
        private readonly YtDlpService ytDlpService;
        private readonly AppRuntimePaths appRuntimePaths;
        private readonly ProcessLauncher processLauncher;

        public YtDlpRunner()
            : this(
                new AppSettingsStore(),
                new YtDlpService(),
                new AppRuntimePaths(),
                new ProcessLauncher()
            ) { }

        public YtDlpRunner(
            AppSettingsStore appSettingsStore,
            YtDlpService ytDlpService,
            AppRuntimePaths appRuntimePaths,
            ProcessLauncher processLauncher
        )
        {
            this.appSettingsStore = appSettingsStore;
            this.ytDlpService = ytDlpService;
            this.appRuntimePaths = appRuntimePaths;
            this.processLauncher = processLauncher;
        }

        public YtDlpRunContext SaveSettings(AppSettings appSettings)
        {
            // まず画面設定を保存し、その設定を元に通常実行用の config を更新する
            appSettingsStore.Save(appSettings);
            return PrepareMainRunContext(appSettings);
        }

        public YtDlpRunContext PreviewSettings(AppSettings appSettings)
        {
            // 保存前でも、今の画面状態から config 表示だけは更新できるようにする
            return PrepareMainRunContext(appSettings);
        }

        public YtDlpRunContext StartBatch(
            AppSettings appSettings,
            IReadOnlyCollection<string> urls,
            Action<string>? outputHandler = null,
            Action<string>? errorHandler = null
        )
        {
            // 一括実行では保存、config 更新、URL ファイル出力、実行を同じ流れに寄せる
            var context = SaveSettings(appSettings);
            ytDlpService.WriteUrlFile(context.Options.UrlFilePath, urls);
            ytDlpService.RunYtDlp(
                context.Options,
                null,
                outputHandler,
                errorHandler,
                context.ConfigFilePath
            );
            return context;
        }

        public YtDlpRunContext StartSingle(
            AppSettings appSettings,
            string url,
            Action<string>? outputHandler = null,
            Action<string>? errorHandler = null
        )
        {
            // 単体実行でも保存と通常 config は維持しつつ、実行だけ専用 config を使う
            var context = SaveSettings(appSettings);
            if (string.IsNullOrWhiteSpace(url))
            {
                return context;
            }

            var singleRunConfigFilePath = GetSingleRunConfigFilePath();
            ytDlpService.PrepareConfigFile(
                context.Options,
                includeUrlFile: false,
                configFilePath: singleRunConfigFilePath
            );

            string arguments = $@"-f bestvideo+bestaudio ""{url}""";
            ytDlpService.RunYtDlp(
                context.Options,
                arguments,
                outputHandler,
                errorHandler,
                singleRunConfigFilePath
            );

            return context;
        }

        public void UpdateBinary(AppSettings appSettings)
        {
            // 更新ボタンでも画面上の現在値を基準に動くようにする
            appSettingsStore.Save(appSettings);
            RunUpdate(appSettings.ExePath, processLauncher);
        }

        private YtDlpRunContext PrepareMainRunContext(AppSettings appSettings)
        {
            var options = appSettings.ToYtDlpOptions();
            var configFilePath = ytDlpService.PrepareConfigFile(options);

            return new YtDlpRunContext
            {
                Options = options,
                ConfigFilePath = configFilePath
            };
        }

        private string GetSingleRunConfigFilePath()
        {
            return appRuntimePaths.SingleRunConfigFilePath;
        }

        private static void RunUpdate(string exePath, ProcessLauncher processLauncher)
        {
            if (string.IsNullOrWhiteSpace(exePath))
            {
                return;
            }

            // 指定された yt-dlp.exe を管理者権限で更新する
            var startInfo = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = "-U",
                Verb = "runas",
                UseShellExecute = true
            };
            processLauncher.StartAndWait(startInfo);
        }
    }
}
