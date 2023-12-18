using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
//using Natsort;

namespace MovieCut.ffmpeg
{
    internal class silencedetect
    {
        //static List<string> GetMovie(string wkDir)
        //{
        //    var files = Directory.GetFiles(wkDir);
        //    return files.Where(x => x.EndsWith(".MOV") || x.EndsWith(".mp4") || x.EndsWith(".mov")).OrderByNatural().ToList();
        //}

        public static void CutSilent(string movie, string noiseDB = "0.001",string durationSec = "2")
        {
            //duration:アニメのOPEDは開始終了に0.5秒の無音が入るので余裕を持ってd=0.45を指定するとその区間以上で検出する。
            try
            {
                // This is a simplified version of the original Python function.
                // It doesn't include the logic for parsing the output and splitting the movie file.
                var startInfo = new ProcessStartInfo
                {
                    FileName = ffmpeg.GetFfmpegPath(),
                    Arguments = $"-i {movie} -af silencedetect=noise={noiseDB}dB:d={durationSec} -f null -",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    //コマンドウィンドウを共有
                    UseShellExecute = false,
                    //CreateNoWindow = true,
                    //コンソールアプリケーションを表示にする
                    CreateNoWindow = false,
                };
                using (var process = Process.Start(startInfo))
                {
                    var so = process.StandardOutput.ReadToEnd();
                    var se = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    Console.WriteLine(so);
                    Console.WriteLine("[app3] ------");
                    Console.WriteLine(se);
                    process.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
