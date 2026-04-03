using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace yt_dlp_loader
{
    internal class ProcessLauncher
    {
        public Process Start(ProcessStartInfo startInfo)
        {
            var process = new Process
            {
                StartInfo = startInfo
            };
            process.Start();
            return process;
        }

        public void StartAndMonitor(
            ProcessStartInfo startInfo,
            Action<string>? outputHandler = null,
            Action<string>? errorHandler = null
        )
        {
            var process = new Process
            {
                StartInfo = startInfo
            };

            if (outputHandler != null)
            {
                process.OutputDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        outputHandler(e.Data);
                    }
                };
            }

            if (errorHandler != null)
            {
                process.ErrorDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        errorHandler(e.Data);
                    }
                };
            }

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            // 実行完了待ちは別タスクへ逃がし、UI 停止を避ける
            Task.Run(() =>
            {
                process.WaitForExit();
                process.Dispose();
            });
        }

        public void StartAndWait(ProcessStartInfo startInfo)
        {
            using var process = Start(startInfo);
            process.WaitForExit();
        }

        public ProcessStartInfo CreateUtf8ConsoleProcessStartInfo(string fileName, string arguments)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };

            // yt-dlp が UTF-8 で出力するように環境変数をそろえる
            startInfo.EnvironmentVariables["PYTHONIOENCODING"] = "utf-8";
            startInfo.EnvironmentVariables["PYTHONLEGACYWINDOWSSTDIO"] = "0";
            startInfo.EnvironmentVariables["PYTHONUTF8"] = "1";
            return startInfo;
        }
    }
}
