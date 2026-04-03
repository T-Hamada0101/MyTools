using System;
using System.Diagnostics;

namespace yt_dlp_loader
{
    internal class BrowserLauncher
    {
        private readonly BrowserProfileRepository browserProfileRepository;
        private readonly BrowserExecutableResolver browserExecutableResolver;
        private readonly ProcessLauncher processLauncher;

        public BrowserLauncher()
            : this(
                new BrowserProfileRepository(),
                new BrowserExecutableResolver(),
                new ProcessLauncher()
            ) { }

        public BrowserLauncher(
            BrowserProfileRepository browserProfileRepository,
            BrowserExecutableResolver browserExecutableResolver,
            ProcessLauncher processLauncher
        )
        {
            this.browserProfileRepository = browserProfileRepository;
            this.browserExecutableResolver = browserExecutableResolver;
            this.processLauncher = processLauncher;
        }

        public Process? Start(string url, string? selectedProfileName)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return null;
            }

            return processLauncher.Start(CreateProcessStartInfo(url, selectedProfileName));
        }

        private ProcessStartInfo CreateProcessStartInfo(string url, string? selectedProfileName)
        {
            var profile = browserProfileRepository.GetProfileOrDefault(selectedProfileName);
            var browserName = BrowserLaunchSupport.NormalizeOrThrow(profile.BrowserName);
            var executablePath = browserExecutableResolver.Resolve(profile);
            var startInfo = new ProcessStartInfo
            {
                FileName = executablePath,
                UseShellExecute = false
            };

            // ブラウザ種別ごとの起動引数差分だけをここで持つ
            switch (browserName)
            {
                case "chrome":
                case "edge":
                    startInfo.Arguments =
                        $"--profile-directory=\"{profile.ProfileDirectory}\" \"{url}\"";
                    break;
                default:
                    startInfo.Arguments = $"-P \"{profile.ProfileName}\" \"{url}\"";
                    break;
            }

            return startInfo;
        }
    }
}
