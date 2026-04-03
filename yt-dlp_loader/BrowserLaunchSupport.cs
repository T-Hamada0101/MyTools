namespace yt_dlp_loader
{
    internal static class BrowserLaunchSupport
    {
        public static string NormalizeOrThrow(string browserName)
        {
            var normalizedName = Normalize(browserName);
            if (!string.IsNullOrWhiteSpace(normalizedName))
            {
                return normalizedName;
            }

            var displayName = string.IsNullOrWhiteSpace(browserName) ? "(empty)" : browserName;
            throw new System.InvalidOperationException(
                $"ブラウザ '{displayName}' は起動未対応です。browser には firefox / chrome / edge を指定してください。"
            );
        }

        private static string Normalize(string browserName)
        {
            switch ((browserName ?? string.Empty).Trim().ToLowerInvariant())
            {
                case "chrome":
                    return "chrome";
                case "edge":
                case "msedge":
                    return "edge";
                case "firefox":
                    return "firefox";
                default:
                    return string.Empty;
            }
        }
    }
}
