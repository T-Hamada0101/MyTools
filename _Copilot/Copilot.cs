using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Copilot
{
    internal class Copilot
    {
        //サーバーのファイルパス
        string serverPath = @"\\";
        //接続できるか
        bool canConnect = false;
        //接続できるかの確認
        public void CheckConnect()
        {
            try
            {
                System.IO.Directory.GetFiles(serverPath);
                canConnect = true;
            }
            catch
            {
                canConnect = false;
            }
        }
        public bool CanConnect
        {
            get { return canConnect; }
        }
    }
}
