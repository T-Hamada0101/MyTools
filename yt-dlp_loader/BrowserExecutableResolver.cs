using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace yt_dlp_loader
{
    internal class BrowserExecutableResolver
    {
        public string Resolve(BrowserProfile profile)
        {
            var explicitPath = profile.ExecutablePath?.Trim() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(explicitPath))
            {
                if (File.Exists(explicitPath))
                {
                    return explicitPath;
                }

                throw new FileNotFoundException(
                    $"browser_profiles.json の executablePath が見つかりません: {explicitPath}"
                );
            }

            var candidates = GetCandidatePaths(profile.BrowserName).ToList();
            var resolvedPath = candidates.FirstOrDefault(File.Exists);
            if (!string.IsNullOrWhiteSpace(resolvedPath))
            {
                return resolvedPath;
            }

            var browserName = string.IsNullOrWhiteSpace(profile.BrowserName)
                ? "browser"
                : profile.BrowserName;
            var searchList = candidates.Count == 0
                ? "候補なし"
                : string.Join(Environment.NewLine, candidates);

            throw new FileNotFoundException(
                $"{browserName} の実行ファイルが見つかりません。browser_profiles.json の executablePath を指定するか、次の候補先を確認してください。{Environment.NewLine}{searchList}"
            );
        }

        private static IEnumerable<string> GetCandidatePaths(string browserName)
        {
            var lowerName = BrowserLaunchSupport.NormalizeOrThrow(browserName);
            var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            var programFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            switch (lowerName)
            {
                case "chrome":
                    return new[]
                    {
                        Path.Combine(programFiles, "Google", "Chrome", "Application", "chrome.exe"),
                        Path.Combine(programFilesX86, "Google", "Chrome", "Application", "chrome.exe"),
                        Path.Combine(localAppData, "Google", "Chrome", "Application", "chrome.exe")
                    };
                case "edge":
                    return new[]
                    {
                        Path.Combine(programFiles, "Microsoft", "Edge", "Application", "msedge.exe"),
                        Path.Combine(programFilesX86, "Microsoft", "Edge", "Application", "msedge.exe")
                    };
                default:
                    return new[]
                    {
                        Path.Combine(programFiles, "Mozilla Firefox", "firefox.exe"),
                        Path.Combine(programFilesX86, "Mozilla Firefox", "firefox.exe")
                    };
            }
        }
    }
}
