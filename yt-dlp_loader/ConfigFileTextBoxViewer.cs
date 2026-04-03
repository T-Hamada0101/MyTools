using System.IO;
using System.Windows.Forms;

namespace yt_dlp_loader
{
    internal class ConfigFileTextBoxViewer
    {
        private readonly TextBox textBox;

        public ConfigFileTextBoxViewer(TextBox textBox)
        {
            this.textBox = textBox;
        }

        public void Show(string configFilePath)
        {
            if (!File.Exists(configFilePath))
            {
                textBox.Clear();
                return;
            }

            // 生成済み config をそのまま表示し、実行内容をすぐ確認できるようにする
            textBox.Lines = File.ReadAllLines(configFilePath);
        }
    }
}
