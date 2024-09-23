using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HeyRed.Mime;//Nuget MimeTypesMap
//
namespace EncodeAuto
{
    internal class FileUtils
    {
        /// <summary>
        /// 動画ファイルを判定
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        internal static bool IsMovieFile(string filePath)
        {
            //拡張子の取得:
            string _ext = Path.GetExtension(filePath);
            string _type = MimeTypesMap.GetMimeType(_ext);
            bool isVideo = _type.StartsWith("video/");
            return isVideo;
        }
        /// <summary>
        /// 音声ファイルを判定
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        internal static bool IsAudioFile(string filePath)
        {
            //拡張子の取得:
            string _ext = Path.GetExtension(filePath);
            string _type = MimeTypesMap.GetMimeType(_ext);
            bool isAudio = _type.StartsWith("audio/");
            return isAudio;
        }
        // find all imagesin a directory
        internal static List<string> GetImages(string dir)
        {
            List<string> result = new List<string>();
            string[] files = Directory.GetFiles(dir);
            foreach (string file in files)
            {
                if (IsImageFile(file))
                {
                    result.Add(file);
                }
            }
            return result;
        }

        static bool IsImageFile(string filePath)
        {
            //拡張子の取得:
            string _ext = Path.GetExtension(filePath);
            string _type = MimeTypesMap.GetMimeType(_ext);
            bool isImage = _type.StartsWith("image/");
            return isImage;
        }

        //find all movies in a directory
        internal static List<string> GetMovies(string dir)
        {
            List<string> result = new List<string>();
            string[] files = Directory.GetFiles(dir);
            foreach (string file in files)
            {
                if (IsMovieFile(file))
                {
                    result.Add(file);
                }
            }
            return result;
        }

        internal void OpenFile(string filePath)
        {
            try
            {
                System.Diagnostics.Process.Start(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Dirがなければ作成
        /// </summary>
        /// <param name="path"></param>
        internal static void CleateDir(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    //Console.WriteLine($"Directory created: {path}");
                }
                else
                {
                    //Console.WriteLine($"Directory already exists: {path}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating directory: {ex.Message}");
            }
        }
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
        /// <summary>
        /// ファイルサイズ比較
        /// </summary>
        /// <param name="org"></param>
        /// <param name="after"></param>
        /// <returns></returns>
        internal static bool IsSmaller(string org, string after)
        {
            FileInfo fo = new FileInfo(org);
            FileInfo fa = new FileInfo(after);
            if (fo.Length > fa.Length) { return true; }
            return false;
        }

        /// <summary>
        /// 指定DirへMove(重複時は上書き)
        /// </summary>
        /// <param name="terget"></param>
        /// <param name="outDir"></param>
        /// <returns>orgFileの最終所在地</returns>
        internal static string MoveDir(string terget, string outDir)
        {
            if(terget == outDir) { return terget; }
            //ファイル名の取得:
            string neme = Path.GetFileName(terget);
            string outpath = outDir + @"\" + neme;
            string result = outpath;
            if (terget == outpath) { return terget; }
            try
            {
                File.Move(terget, outpath, true);
            }
            catch (Exception)
            {
                Console.WriteLine(terget);
                Console.WriteLine(outpath);
                result = terget;
                //throw;
            }
            Thread.Sleep(100);
            return result;
        }
    }
}
