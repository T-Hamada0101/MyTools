using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Copilot
{
    public class T_centers
    {
        public long id = 0;
        public string center
        {
            set
            {
                center1byte = value;
                center2byte = KanaEx.ToZenkakuKana(center1byte);
            }
            get
            {
                return center1byte;
            }
        }
        public string center1byte = "";//センタ名全角
        public string center2byte = "";//センタ名全角
        public string post_code = "";
        public string address = "";
        public string tel = "";
        public string fax = "";
    }
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


        private void dappertest(string ConnectString)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectString))
            {
                string query = "SELECT * FROM MyTableName WHERE Foo = @Foo AND Bar = @Bar";

                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("@Foo", "foo");
                dictionary.Add("@Bar", "bar");

                var results = connection.Query<MyTableName>(query, new DynamicParameters(dictionary));
            }
        }

        private void UpadteWithDapper(T_centers center,string ConnectString)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectString))
            {
                //レコードを更新する
                string query = "UPDATE T_centers SET center = @center, post_code = @post_code, address = @address, tel = @tel, fax = @fax WHERE id = @id";
                connection.Execute(query, center);



                //string query = "UPDATE T_centers SET center = @center, post_code = @post_code, address = @address, tel = @tel, fax = @fax WHERE id = @id";
                //connection.Execute(query, center);

            }
        }
    }
}
