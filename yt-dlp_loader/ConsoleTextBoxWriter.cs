using System;
using System.Windows.Forms;

namespace yt_dlp_loader
{
    internal class ConsoleTextBoxWriter
    {
        private readonly TextBox textBox;

        public ConsoleTextBoxWriter(TextBox textBox)
        {
            this.textBox = textBox;
        }

        public void Clear()
        {
            // 実行前に前回ログを消し、今回の出力だけ追えるようにする
            InvokeOnUiThread(() => textBox.Clear());
        }

        public Action<string> CreateHandler()
        {
            // バックグラウンド出力も UI スレッドへ戻して安全に追記する
            return (data) => AppendLine(data);
        }

        private void AppendLine(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            InvokeOnUiThread(() =>
            {
                textBox.AppendText(text + Environment.NewLine);
                textBox.SelectionStart = textBox.Text.Length;
                textBox.ScrollToCaret();
            });
        }

        private void InvokeOnUiThread(Action action)
        {
            if (textBox.InvokeRequired)
            {
                textBox.Invoke(action);
                return;
            }

            action();
        }
    }
}
