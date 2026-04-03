using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace yt_dlp_loader
{
    internal class AppSettingsStore
    {
        private readonly BrowserProfileRepository browserProfileRepository;
        private readonly AppRuntimePaths appRuntimePaths;

        public AppSettingsStore()
            : this(new BrowserProfileRepository(), new AppRuntimePaths()) { }

        public AppSettingsStore(
            BrowserProfileRepository browserProfileRepository,
            AppRuntimePaths appRuntimePaths
        )
        {
            this.browserProfileRepository = browserProfileRepository;
            this.appRuntimePaths = appRuntimePaths;
        }

        public AppSettings Load()
        {
            // まず正規の JSON を読み、無ければ旧 user.config を移行する
            var fileSettings = LoadFromJsonFile();
            if (fileSettings != null)
            {
                return fileSettings;
            }

            var legacySettings = LoadFromLegacyUserConfig();
            if (legacySettings != null)
            {
                SaveToJsonFile(legacySettings);
                return legacySettings;
            }

            return new AppSettings();
        }

        public void Save(AppSettings appSettings)
        {
            // 保存先を JSON へ一本化し、追跡しやすくする
            SaveToJsonFile(Normalize(appSettings));
        }

        public string GetSettingsFilePath()
        {
            return appRuntimePaths.AppSettingsFilePath;
        }

        private AppSettings? LoadFromJsonFile()
        {
            if (!File.Exists(appRuntimePaths.AppSettingsFilePath))
            {
                return null;
            }

            try
            {
                var json = File.ReadAllText(appRuntimePaths.AppSettingsFilePath, Encoding.UTF8);
                var appSettings = JsonSerializer.Deserialize<AppSettings>(json);
                return appSettings == null ? null : Normalize(appSettings);
            }
            catch
            {
                // JSON が壊れている場合は新規設定か旧設定移行へフォールバックする
                return null;
            }
        }

        private AppSettings? LoadFromLegacyUserConfig()
        {
            if (!Directory.Exists(appRuntimePaths.SettingsDirectoryPath))
            {
                return null;
            }

            try
            {
                var legacyConfigPath = Directory
                    .GetFiles(
                        appRuntimePaths.SettingsDirectoryPath,
                        "user.config",
                        SearchOption.AllDirectories
                    )
                    .OrderByDescending(File.GetLastWriteTimeUtc)
                    .FirstOrDefault();

                return string.IsNullOrWhiteSpace(legacyConfigPath)
                    ? null
                    : ParseLegacyUserConfig(legacyConfigPath);
            }
            catch
            {
                return null;
            }
        }

        private AppSettings? ParseLegacyUserConfig(string configPath)
        {
            try
            {
                var document = XDocument.Load(configPath);
                var settings = document
                    .Descendants()
                    .Where(element => element.Name.LocalName == "setting")
                    .ToDictionary(
                        element => (string?)element.Attribute("name") ?? string.Empty,
                        element =>
                            element.Elements().FirstOrDefault(child => child.Name.LocalName == "value")
                                ?.Value ?? string.Empty,
                        StringComparer.Ordinal
                    );

                if (settings.Count == 0)
                {
                    return null;
                }

                return Normalize(new AppSettings
                {
                    ExePath = GetStringSetting(settings, "ExePath"),
                    UrlFilePath = GetStringSetting(settings, "UrlFilePath"),
                    DownloadDirectory = GetStringSetting(settings, "DownloadDirectory"),
                    IsOpenUrl = GetBoolSetting(settings, "IsOpenUrl"),
                    BrowserOpenTime = GetIntSetting(settings, "BrowserOpenTime", 5),
                    DLThreads = GetIntSetting(settings, "DLThreads", 1),
                    AddDownloaderName = GetBoolSetting(settings, "AddDownloaderName"),
                    AddVideoId = GetBoolSetting(settings, "AddVideoId"),
                    LimitSize720p = GetBoolSetting(settings, "LimitSize720p"),
                    UseCustomPrefix1 = GetBoolSetting(settings, "UseCustomPrefix1"),
                    AddPrefix1 = GetStringSetting(settings, "AddPrefix1"),
                    UseCustomSuffix1 = GetBoolSetting(settings, "UseCustomSuffix1"),
                    AddText1 = GetStringSetting(settings, "AddText1"),
                    UseCustomSuffix2 = GetBoolSetting(settings, "UseCustomSuffix2"),
                    AddText2 = GetStringSetting(settings, "AddText2"),
                    SelectBrowserProfile = GetStringSetting(settings, "SelectBrowserProfile")
                });
            }
            catch
            {
                return null;
            }
        }

        private static string GetStringSetting(
            IReadOnlyDictionary<string, string> settings,
            string key
        )
        {
            return settings.TryGetValue(key, out var value) ? value ?? string.Empty : string.Empty;
        }

        private static bool GetBoolSetting(IReadOnlyDictionary<string, string> settings, string key)
        {
            return settings.TryGetValue(key, out var value) && bool.TryParse(value, out var result)
                ? result
                : false;
        }

        private static int GetIntSetting(
            IReadOnlyDictionary<string, string> settings,
            string key,
            int defaultValue
        )
        {
            return settings.TryGetValue(key, out var value) && int.TryParse(value, out var result)
                ? result
                : defaultValue;
        }

        private void SaveToJsonFile(AppSettings appSettings)
        {
            appRuntimePaths.EnsureSettingsDirectoryExists();
            var json = JsonSerializer.Serialize(
                appSettings,
                new JsonSerializerOptions { WriteIndented = true }
            );
            File.WriteAllText(appRuntimePaths.AppSettingsFilePath, json, new UTF8Encoding(false));
        }

        private AppSettings Normalize(AppSettings appSettings)
        {
            return new AppSettings
            {
                ExePath = appSettings.ExePath ?? string.Empty,
                UrlFilePath = appSettings.UrlFilePath ?? string.Empty,
                DownloadDirectory = appSettings.DownloadDirectory ?? string.Empty,
                IsOpenUrl = appSettings.IsOpenUrl,
                BrowserOpenTime = Math.Max(0, appSettings.BrowserOpenTime),
                DLThreads = Math.Max(1, appSettings.DLThreads),
                AddDownloaderName = appSettings.AddDownloaderName,
                AddVideoId = appSettings.AddVideoId,
                LimitSize720p = appSettings.LimitSize720p,
                UseCustomPrefix1 = appSettings.UseCustomPrefix1,
                AddPrefix1 = appSettings.AddPrefix1 ?? string.Empty,
                UseCustomSuffix1 = appSettings.UseCustomSuffix1,
                AddText1 = appSettings.AddText1 ?? string.Empty,
                UseCustomSuffix2 = appSettings.UseCustomSuffix2,
                AddText2 = appSettings.AddText2 ?? string.Empty,
                SelectBrowserProfile = browserProfileRepository.ResolveProfileName(
                    appSettings.SelectBrowserProfile
                )
            };
        }
    }
}
