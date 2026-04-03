using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace yt_dlp_loader
{
    internal class BrowserUrlBatchOpener
    {
        private readonly BrowserLauncher browserLauncher;
        private readonly BrowserProfileRepository browserProfileRepository;

        public BrowserUrlBatchOpener()
            : this(new BrowserLauncher(), new BrowserProfileRepository()) { }

        public BrowserUrlBatchOpener(
            BrowserLauncher browserLauncher,
            BrowserProfileRepository browserProfileRepository
        )
        {
            this.browserLauncher = browserLauncher;
            this.browserProfileRepository = browserProfileRepository;
        }

        public async Task OpenAsync(IReadOnlyCollection<string> urls, YtDlpOptions options)
        {
            if (!options.IsOpenUrl)
            {
                return;
            }

            var resolvedProfileName = browserProfileRepository.ResolveProfileName(
                options.SelectedBrowserProfile
            );
            var targets = urls.Where(url => !string.IsNullOrWhiteSpace(url)).ToList();
            var waitMilliseconds = Math.Max(0, options.BrowserWaitSeconds) * 1000;

            // URL を順に開き、必要な待機だけを非同期で挟んで UI 停止を避ける
            for (int i = 0; i < targets.Count; i++)
            {
                browserLauncher.Start(targets[i], resolvedProfileName);

                if (i < targets.Count - 1 && waitMilliseconds > 0)
                {
                    await Task.Delay(waitMilliseconds);
                }
            }
        }
    }
}
