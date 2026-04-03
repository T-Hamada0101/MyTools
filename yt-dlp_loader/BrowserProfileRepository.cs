using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace yt_dlp_loader
{
    internal class BrowserProfileRepository
    {
        private readonly AppRuntimePaths appRuntimePaths;
        private readonly IReadOnlyDictionary<string, BrowserProfile> browserProfiles;
        private readonly string loadWarningMessage;

        public BrowserProfileRepository()
            : this(new AppRuntimePaths()) { }

        public BrowserProfileRepository(AppRuntimePaths appRuntimePaths)
        {
            this.appRuntimePaths = appRuntimePaths;
            var loadResult = LoadProfiles();
            browserProfiles = loadResult.BrowserProfiles;
            loadWarningMessage = loadResult.WarningMessage;
        }

        public string ProfilesFilePath => appRuntimePaths.BrowserProfilesFilePath;

        public string LoadWarningMessage => loadWarningMessage;

        public string[] GetProfileDisplayNames()
        {
            return browserProfiles.Keys.ToArray();
        }

        public string ResolveProfileName(string? profileName)
        {
            if (!string.IsNullOrWhiteSpace(profileName) && browserProfiles.ContainsKey(profileName))
            {
                return profileName;
            }

            return GetDefaultProfileName();
        }

        public BrowserProfile GetProfileOrDefault(string? profileName)
        {
            var resolvedProfileName = ResolveProfileName(profileName);
            if (
                !string.IsNullOrWhiteSpace(resolvedProfileName)
                && browserProfiles.TryGetValue(resolvedProfileName, out var profile)
            )
            {
                return profile;
            }

            // browser_profiles.json が無い場合でも最低限の既定値で動かす
            return new BrowserProfile
            {
                DisplayName = "default",
                BrowserName = "firefox",
                ProfileName = "default-release",
                ProfileDirectory = "default-release",
                ExecutablePath = string.Empty
            };
        }

        public string GetDefaultProfileName()
        {
            return browserProfiles.Keys.FirstOrDefault() ?? string.Empty;
        }

        private BrowserProfileLoadResult LoadProfiles()
        {
            var result = new Dictionary<string, BrowserProfile>(StringComparer.Ordinal);
            if (!File.Exists(ProfilesFilePath))
            {
                return new BrowserProfileLoadResult(result, string.Empty);
            }

            try
            {
                var json = File.ReadAllText(ProfilesFilePath);
                var entries = JsonSerializer.Deserialize<List<ProfileEntry>>(json);
                if (entries == null)
                {
                    return new BrowserProfileLoadResult(
                        result,
                        $"browser_profiles.json を読み込めませんでした: 配列形式の JSON ではありません。{Environment.NewLine}{ProfilesFilePath}"
                    );
                }

                foreach (var entry in entries)
                {
                    if (
                        string.IsNullOrWhiteSpace(entry.displayName)
                        || string.IsNullOrWhiteSpace(entry.browser)
                    )
                    {
                        continue;
                    }

                    result[entry.displayName] = new BrowserProfile
                    {
                        DisplayName = entry.displayName,
                        BrowserName = entry.browser,
                        ProfileName = entry.profileName ?? string.Empty,
                        ProfileDirectory = entry.profileDir ?? string.Empty,
                        ExecutablePath = entry.executablePath ?? string.Empty
                    };
                }

                if (entries.Count > 0 && result.Count == 0)
                {
                    return new BrowserProfileLoadResult(
                        result,
                        $"browser_profiles.json に有効なブラウザ定義がありません。displayName と browser を確認してください。{Environment.NewLine}{ProfilesFilePath}"
                    );
                }
            }
            catch (Exception ex)
            {
                return new BrowserProfileLoadResult(
                    result,
                    $"browser_profiles.json の読込に失敗しました: {ex.Message}{Environment.NewLine}{ProfilesFilePath}"
                );
            }

            return new BrowserProfileLoadResult(result, string.Empty);
        }

        private class ProfileEntry
        {
            public string displayName { get; set; } = string.Empty;
            public string browser { get; set; } = string.Empty;
            public string profileName { get; set; } = string.Empty;
            public string profileDir { get; set; } = string.Empty;
            public string executablePath { get; set; } = string.Empty;
        }

        private class BrowserProfileLoadResult
        {
            public BrowserProfileLoadResult(
                IReadOnlyDictionary<string, BrowserProfile> browserProfiles,
                string warningMessage
            )
            {
                BrowserProfiles = browserProfiles;
                WarningMessage = warningMessage;
            }

            public IReadOnlyDictionary<string, BrowserProfile> BrowserProfiles { get; }
            public string WarningMessage { get; }
        }
    }
}
