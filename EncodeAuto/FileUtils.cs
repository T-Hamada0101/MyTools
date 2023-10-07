using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EncodeAuto
{
    internal class FileUtils
    {
        /// <summary>
        /// フォルダ内も探索しすべてのファイルパスを返す
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        internal static List<string> GetAllFilePath(string[] files)
        {
            List<string> result = new List<string>();
            foreach (string file in files)
            {
                string[] _fs = new string[] { file };
                if (IsDirectory(file))
                {
                    _fs = FileUtils.GetAllFilesInDir(file);
                }

                foreach (string f in _fs)
                {
                    if (File.Exists(f)) { result.Add(f); }
                }
            }
            return result;
        }

        internal static string[] GetAllFilesInDir(string dir)
        {
            return Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories);
        }

        /// <summary>
        /// パスがディレクトリならtrue
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static bool IsDirectory(string path)
        {
            if (string.IsNullOrEmpty(path)) return false;
            return File.GetAttributes(path).HasFlag(FileAttributes.Directory);
        }

    }
}
