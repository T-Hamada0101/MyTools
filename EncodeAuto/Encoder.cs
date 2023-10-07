using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncodeAuto
{
    internal class Encoder
    {
        public Encoder(EncodeDeta deta)
        {
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
            Process p =Process.Start(deta.batPath);
            await p.WaitForExitAsync();
            Thread.Sleep(5000); // 5秒間スレッドを停止
            deta.PostProcessing();
            //deta.ComebackEmojiFile();
            //deta.MoveCompleatedFile();
            Console.WriteLine("Encoder finished");
        }
    }
}
