using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCut.ffmpeg
{
    internal class ffmpeg
    {
        public static string GetFfmpegPath()
        {
            string ffmpegPath = Properties.Settings.Default.ffmpegPath;
            if (ffmpegPath == "")
            {
                // ffmpeg.exeが見つからない場合は、ffmpeg.exeを探す
                ffmpegPath = SearchFfmpeg();
                if (ffmpegPath == "")
                {
                    ffmpegPath = OpenFileDialog();
                    if (ffmpegPath == "")
                    {
                        throw new Exception("ffmpeg.exeが見つかりません");
                    }
                }
            }
            return ffmpegPath;
        }
        public static string SearchFfmpeg()
        {
            string ffmpegPath = "";
            string[] paths = Environment.GetEnvironmentVariable("PATH").Split(';');
            foreach (string path in paths)
            {
                string ffmpeg = Path.Combine(path, "ffmpeg.exe");
                if (File.Exists(ffmpeg))
                {
                    ffmpegPath = ffmpeg;
                    break;
                }
            }
            return ffmpegPath;
        }
        public static string OpenFileDialog()
        {
            string ffmpegPath = "";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "ffmpeg file(*.exe)|*.exe|All files (*.*)|*.*";
            ofd.Title = "ffmpeg.exeを選択してください";
            ofd.CheckFileExists = true;
            ofd.Multiselect = false;
            ofd.ReadOnlyChecked = false;
            ofd.ShowReadOnly = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ffmpegPath = ofd.FileName;
                ////　Get file path
                //string filePath = ofd.FileName;
                ////　Get file name
                //string fileName = System.IO.Path.GetFileName(filePath);
                ////　Get file extension
                //string fileExtension = System.IO.Path.GetExtension(filePath);
                ////　Get file directory
                //string fileDirectory = System.IO.Path.GetDirectoryName(filePath);
            }
            return ffmpegPath;
        }
        public static string GetFfmpegArgs(string input, string output, string start, string end)
        {
            string ffmpegPath = GetFfmpegPath();
            string args = $"-ss {start} -i \"{input}\" -to {end} -c copy \"{output}\"";
            return args;
        }
    }
}
