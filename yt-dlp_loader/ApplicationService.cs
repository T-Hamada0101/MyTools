using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace yt_dlp_loader
{
    internal class ApplicationService
    {
        private readonly AppSettingsStore appSettingsStore;
        private readonly AppRuntimePaths appRuntimePaths;
        private readonly AppSettingsValidator appSettingsValidator;
        private readonly BrowserUrlBatchOpener browserUrlBatchOpener;
        private readonly BrowserProfileRepository browserProfileRepository;
        private readonly BrowserLauncher browserLauncher;
        private readonly ProcessLauncher processLauncher;
        private readonly YtDlpRunner ytDlpRunner;

        public ApplicationService()
            : this(
                new AppSettingsStore(),
                new AppRuntimePaths(),
                new AppSettingsValidator(),
                new BrowserUrlBatchOpener(),
                new BrowserProfileRepository(),
                new BrowserLauncher(),
                new ProcessLauncher(),
                new YtDlpRunner()
            ) { }

        public ApplicationService(
            AppSettingsStore appSettingsStore,
            AppRuntimePaths appRuntimePaths,
            AppSettingsValidator appSettingsValidator,
            BrowserUrlBatchOpener browserUrlBatchOpener,
            BrowserProfileRepository browserProfileRepository,
            BrowserLauncher browserLauncher,
            ProcessLauncher processLauncher,
            YtDlpRunner ytDlpRunner
        )
        {
            this.appSettingsStore = appSettingsStore;
            this.appRuntimePaths = appRuntimePaths;
            this.appSettingsValidator = appSettingsValidator;
            this.browserUrlBatchOpener = browserUrlBatchOpener;
            this.browserProfileRepository = browserProfileRepository;
            this.browserLauncher = browserLauncher;
            this.processLauncher = processLauncher;
            this.ytDlpRunner = ytDlpRunner;
        }

        public AppSettings LoadSettings()
        {
            return appSettingsStore.Load();
        }

        public string[] GetBrowserProfileDisplayNames()
        {
            return browserProfileRepository.GetProfileDisplayNames();
        }

        public string ResolveBrowserProfileName(string? profileName)
        {
            return browserProfileRepository.ResolveProfileName(profileName);
        }

        public string GetBrowserProfilesLoadWarningMessage()
        {
            return browserProfileRepository.LoadWarningMessage;
        }

        public string GetMainConfigFilePath()
        {
            return appRuntimePaths.MainConfigFilePath;
        }

        public YtDlpRunContext SaveSettings(AppSettings appSettings)
        {
            return ytDlpRunner.SaveSettings(appSettings);
        }

        public YtDlpRunContext PreviewSettings(AppSettings appSettings)
        {
            return ytDlpRunner.PreviewSettings(appSettings);
        }

        public async Task<YtDlpRunContext> StartBatchAsync(
            AppSettings appSettings,
            IReadOnlyCollection<string> urls,
            Action<string>? outputHandler = null,
            Action<string>? errorHandler = null
        )
        {
            ThrowIfInvalid(appSettingsValidator.ValidateForBatchRun(appSettings));
            var context = ytDlpRunner.StartBatch(appSettings, urls, outputHandler, errorHandler);
            await browserUrlBatchOpener.OpenAsync(urls, context.Options);
            return context;
        }

        public YtDlpRunContext StartSingle(
            AppSettings appSettings,
            string url,
            Action<string>? outputHandler = null,
            Action<string>? errorHandler = null
        )
        {
            ThrowIfInvalid(appSettingsValidator.ValidateForSingleRun(appSettings, url));
            return ytDlpRunner.StartSingle(appSettings, url, outputHandler, errorHandler);
        }

        public Task UpdateBinaryAsync(AppSettings appSettings)
        {
            ThrowIfInvalid(appSettingsValidator.ValidateForUpdate(appSettings));
            return Task.Run(() => ytDlpRunner.UpdateBinary(appSettings));
        }

        public Process? OpenUrl(string url, string? selectedProfileName)
        {
            return browserLauncher.Start(url, ResolveBrowserProfileName(selectedProfileName));
        }

        public void OpenDownloadDirectory(string downloadDirectory)
        {
            if (string.IsNullOrWhiteSpace(downloadDirectory))
            {
                return;
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = "EXPLORER.EXE",
                Arguments = downloadDirectory,
                UseShellExecute = true
            };
            processLauncher.Start(startInfo);
        }

        private static void ThrowIfInvalid(string? validationMessage)
        {
            if (!string.IsNullOrWhiteSpace(validationMessage))
            {
                throw new InvalidOperationException(validationMessage);
            }
        }
    }
}
