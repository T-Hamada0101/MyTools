namespace yt_dlp_loader
{
    internal class BrowserCookieOptionBuilder
    {
        private readonly BrowserProfileRepository browserProfileRepository;

        public BrowserCookieOptionBuilder()
            : this(new BrowserProfileRepository()) { }

        public BrowserCookieOptionBuilder(BrowserProfileRepository browserProfileRepository)
        {
            this.browserProfileRepository = browserProfileRepository;
        }

        public string Build(string? selectedProfileName)
        {
            // 選択中のプロファイルから yt-dlp 用の cookie 指定だけを組み立てる
            var profile = browserProfileRepository.GetProfileOrDefault(selectedProfileName);
            return $@"--cookies-from-browser {profile.BrowserName}:{profile.ProfileDirectory}";
        }
    }
}
