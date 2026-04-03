using System.IO;

namespace yt_dlp_loader
{
    internal class AppSettingsValidator
    {
        public string? ValidateForBatchRun(AppSettings appSettings)
        {
            var exePathError = ValidateExePath(appSettings.ExePath);
            if (exePathError != null)
            {
                return exePathError;
            }

            if (string.IsNullOrWhiteSpace(appSettings.UrlFilePath))
            {
                return "url File を設定してください。";
            }

            if (string.IsNullOrWhiteSpace(appSettings.DownloadDirectory))
            {
                return "DL Folder を設定してください。";
            }

            return null;
        }

        public string? ValidateForSingleRun(AppSettings appSettings, string url)
        {
            var exePathError = ValidateExePath(appSettings.ExePath);
            if (exePathError != null)
            {
                return exePathError;
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                return "実行する URL がありません。";
            }

            if (string.IsNullOrWhiteSpace(appSettings.DownloadDirectory))
            {
                return "DL Folder を設定してください。";
            }

            return null;
        }

        public string? ValidateForUpdate(AppSettings appSettings)
        {
            return ValidateExePath(appSettings.ExePath);
        }

        private static string? ValidateExePath(string exePath)
        {
            if (string.IsNullOrWhiteSpace(exePath))
            {
                return "yt-dlp.exe を設定してください。";
            }

            if (!File.Exists(exePath))
            {
                return $"yt-dlp.exe が見つかりません: {exePath}";
            }

            return null;
        }
    }
}
