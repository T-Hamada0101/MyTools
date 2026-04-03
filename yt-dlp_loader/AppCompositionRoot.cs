namespace yt_dlp_loader
{
    internal static class AppCompositionRoot
    {
        public static Form1 CreateMainForm()
        {
            var appRuntimePaths = new AppRuntimePaths();
            var processLauncher = new ProcessLauncher();
            var browserProfileRepository = new BrowserProfileRepository(appRuntimePaths);
            var browserCookieOptionBuilder = new BrowserCookieOptionBuilder(browserProfileRepository);
            var browserExecutableResolver = new BrowserExecutableResolver();
            var browserLauncher = new BrowserLauncher(
                browserProfileRepository,
                browserExecutableResolver,
                processLauncher
            );
            var browserUrlBatchOpener = new BrowserUrlBatchOpener(
                browserLauncher,
                browserProfileRepository
            );
            var appSettingsValidator = new AppSettingsValidator();
            var appSettingsStore = new AppSettingsStore(
                browserProfileRepository,
                appRuntimePaths
            );
            var ytDlpConfigBuilder = new YtDlpConfigBuilder(browserCookieOptionBuilder);
            var ytDlpService = new YtDlpService(
                appRuntimePaths,
                ytDlpConfigBuilder,
                processLauncher
            );
            var ytDlpRunner = new YtDlpRunner(
                appSettingsStore,
                ytDlpService,
                appRuntimePaths,
                processLauncher
            );
            var applicationService = new ApplicationService(
                appSettingsStore,
                appRuntimePaths,
                appSettingsValidator,
                browserUrlBatchOpener,
                browserProfileRepository,
                browserLauncher,
                processLauncher,
                ytDlpRunner
            );

            return new Form1(applicationService);
        }
    }
}
