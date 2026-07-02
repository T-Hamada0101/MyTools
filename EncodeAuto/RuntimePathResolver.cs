using System.Diagnostics;

namespace EncodeAuto
{
    internal static class RuntimePathResolver
    {
        private const string DefaultBatchFileName = "EndodeAuto.bat";
        private static readonly string[] EncoderNames =
        {
            "NVEncC64.exe",
            "NVEncC.exe",
            "QSVEncC64.exe",
            "QSVEncC.exe",
            "ffmpeg.exe",
            "ffprobe.exe",
        };

        public static string BuildSessionBatchPath(string configuredPath, int sessionNum)
        {
            string basePath = ExpandPath(configuredPath);
            if (string.IsNullOrWhiteSpace(basePath))
            {
                basePath = Path.Combine(GetUserDataDir(), DefaultBatchFileName);
            }

            if (!Path.IsPathRooted(basePath))
            {
                basePath = Path.Combine(GetUserDataDir(), basePath);
            }

            if (!string.Equals(Path.GetExtension(basePath), ".bat", StringComparison.OrdinalIgnoreCase))
            {
                basePath += ".bat";
            }

            string directory = Path.GetDirectoryName(basePath) ?? GetUserDataDir();
            if (!TryCreateDirectory(directory))
            {
                basePath = Path.Combine(GetUserDataDir(), Path.GetFileName(basePath));
                directory = Path.GetDirectoryName(basePath) ?? GetUserDataDir();
                Directory.CreateDirectory(directory);
            }

            string fileName = Path.GetFileNameWithoutExtension(basePath) + sessionNum + ".bat";
            return Path.Combine(directory, fileName);
        }

        public static string ResolveOutputDir(string configuredPath)
        {
            string outputDir = ExpandPath(configuredPath);
            if (string.IsNullOrWhiteSpace(outputDir))
            {
                outputDir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyVideos),
                    "EncodeAuto");
            }

            if (!Path.IsPathRooted(outputDir))
            {
                outputDir = Path.Combine(GetUserDataDir(), outputDir);
            }

            if (!TryCreateDirectory(outputDir))
            {
                outputDir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyVideos),
                    "EncodeAuto");

                if (!TryCreateDirectory(outputDir))
                {
                    outputDir = Path.Combine(GetUserDataDir(), "Output");
                    Directory.CreateDirectory(outputDir);
                }
            }

            return outputDir;
        }

        public static string ResolveCommandLine(string commandLine)
        {
            if (string.IsNullOrWhiteSpace(commandLine))
            {
                return commandLine;
            }

            CommandToken token = ReadCommandToken(commandLine);
            if (string.IsNullOrWhiteSpace(token.Value))
            {
                return commandLine;
            }

            string? executablePath = ResolveExecutable(token.Value);
            if (string.IsNullOrWhiteSpace(executablePath))
            {
                return commandLine;
            }

            string quotedExecutable = Quote(executablePath);
            return commandLine.Substring(0, token.Start)
                + quotedExecutable
                + commandLine.Substring(token.End);
        }

        public static string GetInitialBatchPath()
        {
            return Path.Combine(GetUserDataDir(), DefaultBatchFileName);
        }

        public static string GetInitialArguments()
        {
            string encoder = ResolveExecutable("NVEncC64.exe")
                ?? ResolveExecutable("QSVEncC64.exe")
                ?? ResolveExecutable("ffmpeg.exe")
                ?? "ffmpeg.exe";

            if (Path.GetFileName(encoder).StartsWith("ffmpeg", StringComparison.OrdinalIgnoreCase))
            {
                return Quote(encoder) + " -i %input -c:v libx264 -crf 23 -c:a aac -b:a 192k %out";
            }

            return Quote(encoder) + " -i %input --cqp 34 --audio-copy -o %out";
        }

        public static string GetInitialSafix()
        {
            return "_enc.mp4";
        }

        public static string GetInitialOutputDir()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), "EncodeAuto");
        }

        private static string? ResolveExecutable(string executableName)
        {
            string expanded = ExpandPath(executableName).Trim('"');
            if (Path.IsPathRooted(expanded) && File.Exists(expanded))
            {
                return expanded;
            }

            string fileName = Path.GetFileName(expanded);
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return null;
            }

            string[] fileNames = Path.HasExtension(fileName)
                ? new[] { fileName }
                : new[] { fileName, fileName + ".exe" };

            foreach (string directory in GetSearchDirectories())
            {
                foreach (string candidateName in fileNames)
                {
                    string candidate = Path.Combine(directory, candidateName);
                    if (File.Exists(candidate))
                    {
                        return candidate;
                    }
                }
            }

            foreach (string candidateName in fileNames)
            {
                string? pathCommand = SearchPath(candidateName);
                if (!string.IsNullOrWhiteSpace(pathCommand))
                {
                    return pathCommand;
                }
            }

            return null;
        }

        private static IEnumerable<string> GetSearchDirectories()
        {
            string appDir = AppContext.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            yield return appDir;

            string? parent = Directory.GetParent(appDir)?.FullName;
            if (!string.IsNullOrWhiteSpace(parent))
            {
                yield return parent;
            }

            string configuredBatch = ExpandPath(Properties.Settings.Default.ExePath);
            string? batchDir = Path.GetDirectoryName(configuredBatch);
            if (!string.IsNullOrWhiteSpace(batchDir))
            {
                yield return batchDir;
            }

            foreach (string encoderName in EncoderNames)
            {
                string? pathCommand = SearchPath(encoderName);
                if (!string.IsNullOrWhiteSpace(pathCommand))
                {
                    string? pathDir = Path.GetDirectoryName(pathCommand);
                    if (!string.IsNullOrWhiteSpace(pathDir))
                    {
                        yield return pathDir;
                    }
                }
            }
        }

        private static string? SearchPath(string executableName)
        {
            try
            {
                using Process process = new Process();
                process.StartInfo.FileName = "where";
                process.StartInfo.Arguments = executableName;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit(1000);

                return output
                    .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .FirstOrDefault(File.Exists);
            }
            catch
            {
                return null;
            }
        }

        private static string ExpandPath(string path)
        {
            return Environment.ExpandEnvironmentVariables(path ?? string.Empty);
        }

        private static string GetUserDataDir()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "EncodeAuto");
        }

        private static bool TryCreateDirectory(string directory)
        {
            try
            {
                Directory.CreateDirectory(directory);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static string Quote(string value)
        {
            return "\"" + value + "\"";
        }

        private static CommandToken ReadCommandToken(string commandLine)
        {
            int start = 0;
            while (start < commandLine.Length && char.IsWhiteSpace(commandLine[start]))
            {
                start++;
            }

            if (start >= commandLine.Length)
            {
                return new CommandToken(string.Empty, start, start);
            }

            if (commandLine[start] == '"')
            {
                int endQuote = commandLine.IndexOf('"', start + 1);
                if (endQuote >= 0)
                {
                    return new CommandToken(commandLine.Substring(start + 1, endQuote - start - 1), start, endQuote + 1);
                }
            }

            int end = start;
            while (end < commandLine.Length && !char.IsWhiteSpace(commandLine[end]))
            {
                end++;
            }

            return new CommandToken(commandLine.Substring(start, end - start), start, end);
        }

        private readonly struct CommandToken
        {
            public CommandToken(string value, int start, int end)
            {
                Value = value;
                Start = start;
                End = end;
            }

            public string Value { get; }
            public int Start { get; }
            public int End { get; }
        }
    }
}
