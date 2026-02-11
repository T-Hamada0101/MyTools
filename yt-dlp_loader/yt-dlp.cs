using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yt_dlp_loader.Properties;

namespace yt_dlp_loader
{
    // yt_dlp クラスを内部クラスとして定義
    internal class yt_dlp
    {
        public static void RunYtDlp()
        {
            // yt-dlpに渡す引数例（URLやオプションを指定）
            string arguments = "https://www.youtube.com/watch?v=dQw4w9WgXcQ";

            // 出力・エラーのハンドラを定義
            void OutputHandler(string data) => Console.WriteLine("標準出力: " + data);
            void ErrorHandler(string data) => Console.WriteLine("標準エラー: " + data);

            // メソッド呼び出し
            yt_dlp.RunYtDlpCore(arguments, OutputHandler, ErrorHandler);
        }
        public static void RunYtDlpCore(string arguments)
        {
            // 新しいプロセスを作成
            var process = new Process();
            // 実行するファイル名を設定
            process.StartInfo.FileName = "yt-dlp_loader.exe";
            // コマンドライン引数を設定
            process.StartInfo.Arguments = arguments;
            // プロセスを開始
            process.Start();
            // プロセスの終了を待機
            process.WaitForExit();
        }
        // RunYtDlp メソッドを定義、引数としてコマンドライン引数、出力ハンドラ、エラーハンドラを受け取る
        public static void RunYtDlpCore(string arguments, Action<string> outputHandler, Action<string> errorHandler)
        {
            // 新しいプロセスを作成
            var process = new Process();
            // 実行するファイル名を設定
            process.StartInfo.FileName = "yt-dlp_loader.exe";
            // コマンドライン引数を設定
            process.StartInfo.Arguments = arguments;
            // シェルを使用しない設定
            process.StartInfo.UseShellExecute = false;
            // 標準出力をリダイレクト
            process.StartInfo.RedirectStandardOutput = true;
            // 標準エラーをリダイレクト
            process.StartInfo.RedirectStandardError = true;
            // ウィンドウを表示しない設定
            process.StartInfo.CreateNoWindow = true;

            // 標準出力データを受信したときのイベントハンドラを設定
            process.OutputDataReceived += (sender, e) =>
            {
                // 受信したデータが null でない場合、出力ハンドラを呼び出す
                if (e.Data != null) outputHandler?.Invoke(e.Data);
            };
            // 標準エラーデータを受信したときのイベントハンドラを設定
            process.ErrorDataReceived += (sender, e) =>
            {
                // 受信したデータが null でない場合、エラーハンドラを呼び出す
                if (e.Data != null) errorHandler?.Invoke(e.Data);
            };

            // プロセスを開始
            process.Start();
            // 標準出力の読み取りを開始
            process.BeginOutputReadLine();
            // 標準エラーの読み取りを開始
            process.BeginErrorReadLine();
            // プロセスの終了を待機
            process.WaitForExit();
        }
        public static void UpdateYtDlp()
        {
            // yt-dlpを更新するための引数
            string updateArguments = "-U";

            // yt-dlp.exeを管理者として実行するための設定
            Process process = new Process();
            process.StartInfo.FileName = Settings.Default.ExePath;
            process.StartInfo.Arguments = updateArguments;
            process.StartInfo.Verb = "runas"; // 管理者として実行
            process.StartInfo.UseShellExecute = true; // これが必要
            process.Start();
            process.WaitForExit();
        }
    }
}
