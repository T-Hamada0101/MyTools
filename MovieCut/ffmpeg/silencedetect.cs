using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
//using Natsort;

namespace MovieCut.ffmpeg
{
    /// <summary>
    /// 無音部分の取得
    /// </summary>
    internal class silencedetect
    {
        //static List<string> GetMovie(string wkDir)
        //{
        //    var files = Directory.GetFiles(wkDir);
        //    return files.Where(x => x.EndsWith(".MOV") || x.EndsWith(".mp4") || x.EndsWith(".mov")).OrderByNatural().ToList();
        //}

        public static void GetSilentReport(string movie, string noiseDB = "0.005",string durationSec = "2")
        {
            //duration:アニメのOPEDは開始終了に0.5秒の無音が入るので余裕を持ってd=0.45を指定するとその区間以上で検出する。
            try
            {
                string log = @"D:\WD12share\MyToolsRelease\NVEncC_7.31_x64\log.txt";
                // This is a simplified version of the original Python function.
                // It doesn't include the logic for parsing the output and splitting the movie file.
                var startInfo = new ProcessStartInfo
                {
                    FileName = ffmpeg.GetFfmpegPath(),
                    Arguments = $"-i {movie} -af silencedetect=noise={noiseDB}dB:d={durationSec} -f null -",//2>{log}",
                    RedirectStandardOutput = false,
                    RedirectStandardError = true,//ここに状況が出力される
                    //RedirectStandardInput = true,//プロセスに対してコマンドの送付が可能（FFmpegは「Q」キーで停止可能）
                    UseShellExecute = false,//コマンドウィンドウを共有
                    CreateNoWindow = true,//コンソールアプリケーションを非表示にする(どちらでもOK)

                };
                using (var process = Process.Start(startInfo))
                {
                    //// コマンド終了後にイベント発行させる
                    //process.EnableRaisingEvents = true;
                    //process.Exited += new EventHandler(process_Exited);

                    //var so = process.StandardOutput.ReadToEnd();
                    var se = process.StandardError.ReadToEnd();
                    /* //StandardErrorの取得方法
                    RedirectStandardOutput = false,
                    RedirectStandardError = true,
                    UseShellExecute = false,//コマンドウィンドウを共有しない
                     */

                    process.WaitForExit();

                    //Console.WriteLine(so);
                    //Console.WriteLine("[app3] ------");
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
