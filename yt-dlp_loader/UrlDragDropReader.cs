using System.Windows.Forms;

namespace yt_dlp_loader
{
    internal static class UrlDragDropReader
    {
        public static string Read(DragEventArgs e)
        {
            if (e.Data is null)
            {
                return string.Empty;
            }

            // ファイルドロップとテキストドロップの両方から URL 文字列を拾う
            if (e.Data.GetDataPresent(DataFormats.FileDrop)
                && e.Data.GetData(DataFormats.FileDrop) is string[] strList
                && strList.Length > 0)
            {
                return strList[0];
            }

            if (e.Data.GetDataPresent(DataFormats.Text)
                && e.Data.GetData(DataFormats.Text) is string text)
            {
                return text;
            }

            return string.Empty;
        }
    }
}
