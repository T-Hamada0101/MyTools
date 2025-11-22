using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
namespace yt_dlp_loader
{
    internal static class Browser
    {
        /// <summary>
        /// profile名の一覧をkey,valueで管理
        /// </summary>
        /// string: プロファイル表示名
        /// string browser: ブラウザ名
        /// string profileName: プロファイル名 ブラウザコマンド起動時の引数用
        /// string profileDir: プロファイルディレクトリ名 yt_dlpに渡す用
        private static readonly Dictionary<string, (string browser, string profileName, string profileDir)> browserProfiles = new()
        {
            //firefoxでプロファイルを確認するには about:profiles
            //chromeでプロファイルを確認するには chrome://version/
            { "default", ("firefox", "default-release", "xltthzbp.default-release") },
            { "Firefox ZZ", ("firefox", "default-release", "xltthzbp.default-release") },
            { "Firefox Suteyo", ("firefox", "Suteyo", "dkj5wpar.Suteyo") },
            { "Chrome ZZ", ("chrome", "Default", "Default") },
            { "Chrome Suteyo", ("chrome", "Profile 2", "Profile 2") }
        };

        internal static IReadOnlyDictionary<string, (string browser, string profileName, string profileDir)> BrowserProfiles => browserProfiles;

        internal static string[] GetProfileDisplayNames() => BrowserProfiles.Keys.ToArray();

        internal static string DefaultProfileName => BrowserProfiles.Keys.FirstOrDefault() ?? string.Empty;

        internal static (string browser, string profileName, string profileDir) GetProfileOrDefault(string? profileName)
        {
            if (!string.IsNullOrWhiteSpace(profileName) && BrowserProfiles.TryGetValue(profileName, out var profile))
            {
                return profile;
            }

            if (!string.IsNullOrEmpty(DefaultProfileName) && BrowserProfiles.TryGetValue(DefaultProfileName, out var defaultProfile))
            {
                return defaultProfile;
            }

            return ("firefox", "default-release", "xltthzbp.default-release");
        }

        internal static string GetCookiesOption(string? selectedBrowser)
        {
            var (browser, _, profileDir) = GetProfileOrDefault(selectedBrowser);
            return $@"--cookies-from-browser {browser}:{profileDir}";
        }

        internal static string ResolveProfileName(string? profileName)
        {
            if (!string.IsNullOrWhiteSpace(profileName) && BrowserProfiles.ContainsKey(profileName))
            {
                return profileName;
            }

            return DefaultProfileName;
        }

        internal static ProcessStartInfo CreateProcessStartInfo(string url, string? profileName)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("url must not be null or empty.", nameof(url));
            }

            var (browser, resolvedProfileName, profileDir) = GetProfileOrDefault(profileName);
            var startInfo = new ProcessStartInfo
            {
                UseShellExecute = false
            };

            switch (browser)
            {
                case "chrome":
                    startInfo.FileName = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
                    startInfo.Arguments = $"--profile-directory=\"{profileDir}\" \"{url}\"";
                    break;
                default:
                    startInfo.FileName = @"C:\Program Files\Mozilla Firefox\firefox.exe";
                    startInfo.Arguments = $"-P \"{resolvedProfileName}\" \"{url}\"";
                    break;
            }

            return startInfo;
        }

        internal static IReadOnlyCollection<string> SupportedBrowsers { get; } = new[]
        {
            "chrome",
            "chromium",
            "edge",
            "firefox",
            "brave",
            "opera"
        };

        internal static bool IsSupportedBrowser(string browserName)
        {
            if (string.IsNullOrWhiteSpace(browserName))
            {
                return false;
            }

            return SupportedBrowsers.Contains(browserName.ToLowerInvariant());
        }

        internal static string GetBrowserExecutable(string browserName)
        {
            if (string.IsNullOrWhiteSpace(browserName))
            {
                throw new ArgumentException("browserName must not be null or empty.", nameof(browserName));
            }

            return browserName.ToLowerInvariant() switch
            {
                "chrome" => "chrome.exe",
                "chromium" => "chromium.exe",
                "edge" => "msedge.exe",
                "firefox" => "firefox.exe",
                "brave" => "brave.exe",
                "opera" => "opera.exe",
                _ => throw new ArgumentException($"Unsupported browser: {browserName}", nameof(browserName)),
            };
        }
    }
}
