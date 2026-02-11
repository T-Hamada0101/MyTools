using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace yt_dlp_loader
{
    internal static class Browser
    {
        /// <summary>
        /// browser_profiles.json のパス
        /// </summary>
        private static readonly string ProfilesFilePath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "browser_profiles.json"
        );

        /// <summary>
        /// profile名の一覧をkey,valueで管理
        /// </summary>
        /// string: プロファイル表示名
        /// string browser: ブラウザ名
        /// string profileName: プロファイル名 ブラウザコマンド起動時の引数用
        /// string profileDir: プロファイルディレクトリ名 yt_dlpに渡す用
        private static readonly Dictionary<
            string,
            (string browser, string profileName, string profileDir)
        > browserProfiles = LoadProfiles();

        /// <summary>
        /// browser_profiles.json からプロファイル情報を読み込む
        /// </summary>
        private static Dictionary<
            string,
            (string browser, string profileName, string profileDir)
        > LoadProfiles()
        {
            var result =
                new Dictionary<string, (string browser, string profileName, string profileDir)>();

            if (!File.Exists(ProfilesFilePath))
            {
                return result;
            }

            try
            {
                var json = File.ReadAllText(ProfilesFilePath);
                var entries = JsonSerializer.Deserialize<List<ProfileEntry>>(json);
                if (entries == null)
                    return result;

                foreach (var e in entries)
                {
                    if (
                        !string.IsNullOrWhiteSpace(e.displayName)
                        && !string.IsNullOrWhiteSpace(e.browser)
                    )
                    {
                        result[e.displayName] = (
                            e.browser,
                            e.profileName ?? "",
                            e.profileDir ?? ""
                        );
                    }
                }
            }
            catch
            {
                // JSON読み込み失敗時は空辞書を返す
            }

            return result;
        }

        /// <summary>
        /// JSONデシリアライズ用クラス
        /// </summary>
        private class ProfileEntry
        {
            public string displayName { get; set; } = "";
            public string browser { get; set; } = "";
            public string profileName { get; set; } = "";
            public string profileDir { get; set; } = "";
        }

        internal static IReadOnlyDictionary<
            string,
            (string browser, string profileName, string profileDir)
        > BrowserProfiles => browserProfiles;

        internal static string[] GetProfileDisplayNames() => BrowserProfiles.Keys.ToArray();

        internal static string DefaultProfileName =>
            BrowserProfiles.Keys.FirstOrDefault() ?? string.Empty;

        internal static (string browser, string profileName, string profileDir) GetProfileOrDefault(
            string? profileName
        )
        {
            if (
                !string.IsNullOrWhiteSpace(profileName)
                && BrowserProfiles.TryGetValue(profileName, out var profile)
            )
            {
                return profile;
            }

            if (
                !string.IsNullOrEmpty(DefaultProfileName)
                && BrowserProfiles.TryGetValue(DefaultProfileName, out var defaultProfile)
            )
            {
                return defaultProfile;
            }

            return ("firefox", "default-release", "default-release");
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
            var startInfo = new ProcessStartInfo { UseShellExecute = false };

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

        internal static IReadOnlyCollection<string> SupportedBrowsers { get; } =
            new[] { "chrome", "chromium", "edge", "firefox", "brave", "opera" };

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
                throw new ArgumentException(
                    "browserName must not be null or empty.",
                    nameof(browserName)
                );
            }

            return browserName.ToLowerInvariant() switch
            {
                "chrome" => "chrome.exe",
                "chromium" => "chromium.exe",
                "edge" => "msedge.exe",
                "firefox" => "firefox.exe",
                "brave" => "brave.exe",
                "opera" => "opera.exe",
                _ => throw new ArgumentException(
                    $"Unsupported browser: {browserName}",
                    nameof(browserName)
                ),
            };
        }
    }
}
