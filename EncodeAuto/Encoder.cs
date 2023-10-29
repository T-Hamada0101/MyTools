using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EncodeAuto
{
    internal class Encoder
    {
        public Encoder(EncodeDeta deta)
        {
            //エンコード
            RunCommandAsync(deta);

            
            //deta.ComebackEmojiFile();
            //deta.MoveCompleatedFile();
        }
        ~Encoder()
        {
            Console.WriteLine("Encoder closed");
        }
        /// <summary>
        /// 外部コマンド実行
        /// </summary>
        /// <param name="_command"></param>
        /// <param name="_arguments"></param>
        public void RunCommand(string _command, string? _arguments)
        {
            if (string.IsNullOrEmpty(_arguments))
            {
                Process.Start(_command);
            }
            else
            {
                Process.Start(_command, _arguments);
            }
        }
        /// <summary>
        /// 非同期で外部コマンド実行
        /// </summary>
        /// <param name="command"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static async Task RunCommandAsync(EncodeDeta deta)
        {
            //エンコード
            await RunCoreAsync(deta);
            //Process p =Process.Start(deta.batPath);
            //await p.WaitForExitAsync();
            Thread.Sleep(1000); // 1秒間スレッドを停止
            deta.PostProcessing();
            //deta.ComebackEmojiFile();
            //deta.MoveCompleatedFile();
            Console.WriteLine("Encoder finished");

            //取りこぼし分の再エンコード
            //ErrorFile一覧
            //List<string> lists = FileUtils.GetAllFilePath(new string[] { deta.errorDir });
            List<string> errorFiles = deta.ErrorOrgFiles;
            //foreach (string _f in lists)
            //{
            //    if (!FileUtils.IsMovieFile(_f)) { continue; }
            //    //親detaクラスの一覧にないものは登録除外
            //    string _fName = Path.GetFileName(_f);
            //    foreach (var org in deta.InputFiles)
            //    {
            //        string orgName = Path.GetFileName(org);
            //        if (orgName == _fName)
            //        {
            //            files.Add(_f);
            //        }
            //    }
            //}
            if (errorFiles.Count == 0) return;
            EncodeDeta detaE = new EncodeDeta(0,errorFiles,true);//エラー出力あり
            await RunCoreAsync(detaE);
            Thread.Sleep(500); //0.5秒間スレッドを停止
            detaE.PostProcessing();
            //deta.ComebackEmojiFile();
            //deta.MoveCompleatedFile();
            Console.WriteLine("Encoder(E) finished");
        }

        public static async Task RunCoreAsync(EncodeDeta deta)
        {
            Process p = Process.Start(deta.batPath);
            await p.WaitForExitAsync();
        }
    }
}
