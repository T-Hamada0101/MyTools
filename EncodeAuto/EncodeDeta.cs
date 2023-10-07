using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace EncodeAuto
{
    internal class EncodeDeta
    {
        /// <summary>
        /// 対象ファイルlist
        /// </summary>
        public List<string> InputFiles = new List<string>();

        /// <summary>
        /// 絵文字ファイル名(オリジナル)とエンコード後名
        /// </summary>
        public Dictionary<string, string> After_Encoded = new Dictionary<string, string>();
        /// <summary>
        /// 絵文字ファイル名(オリジナル),一時変更ファイル名*エンコード後名
        /// </summary>
        public Dictionary<string, string> Org_After_Encoded = new Dictionary<string, string>();

        public string batPath = "";
        public string arguments = "";
        public string safix = "";
        public bool isMove = false;
        public string outDir = "";

        private string inputDir;
        private string finishedOriginalDir;
        private string errorDir;
        public EncodeDeta(List<string> _list)
        {
            batPath = Properties.Settings.Default.ExePath;
            arguments = Properties.Settings.Default.Arguments;
            safix = Properties.Settings.Default.Safix;
            isMove = Properties.Settings.Default.MoveComp;
            outDir = Properties.Settings.Default.OutputDir;

            inputDir = outDir + @"\Input";
            finishedOriginalDir = outDir + @"\FinishedOriginal";
            errorDir = outDir + @"\Error";

            CleateDir(outDir);
            CleateDir(inputDir);
            CleateDir(finishedOriginalDir);
            CleateDir(errorDir);
            ClearFile(batPath);
            WriteBatFile(_list);
        }

        private void WriteBatFile(List<string> _list)
        {
            AppendTextToFile(batPath, @"chcp 65001");//chcp 65001
            AppendTextToFile(batPath, @"cd /d %~dp0");
            foreach (string pass in _list)
            {
                InputFiles.Add(pass);

                //絵文字除去
                string after = RenameEmojiFile(pass);

                //拡張子なしのファイル名の取得:
                string _noExt = Path.GetFileNameWithoutExtension(after);
                //エンコード後のファイル名
                string encoded = outDir + @"\" + _noExt + safix;

                //出世魚の名前保存
                Org_After_Encoded.Add(pass, after + @"*" + encoded);

                string arg = arguments.Replace(@"%input", @"""" + after + @"""");
                arg = arg.Replace(@"%out", @"""" + encoded + @"""");
                arg = arg + @" --log-level error --log "+ errorDir + @"\errorlog.txt";
                AppendTextToFile(batPath, arg);
                AppendTextToFile(batPath, "PAUSE");
            }

        }
        public void AppendTextToFile(string filePath, string text)
        {
            EncodingProvider provider = System.Text.CodePagesEncodingProvider.Instance;
            //var encoding = provider.GetEncoding("shift-jis");
            var encoding = Encoding.UTF8;
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(filePath, true, encoding))
            // System.Text.Encoding.GetEncoding("Shift-JIS"))
            //TextBox1.Textの内容を書き込む
            {
                sw.WriteLine(text);
            }
        }

        /// <summary>
        /// コマンドプロンプトで使用できない絵文字等を置き換える
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private string RenameEmojiFile(string filePath)
        {
            string after = Regexs.InputValueValidate(filePath);
            if (!after.Equals(filePath))
            {
                File.Move(filePath, after);
            }
            return after;
        }

        public void ComebackEmojiFile()
        {
            foreach (var item in Org_After_Encoded)
            {
                try
                {
                    File.Move(item.Value, item.Key);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                finally
                {
                    Org_After_Encoded.Remove(item.Key);
                }
            }
        }
        /// <summary>
        /// 後処理
        /// </summary>
        public void PostProcessing()
        {
            foreach (var item in Org_After_Encoded)
            {
                string org = item.Key;
                string[] aft_enc = item.Value.Split('*');
                string after = aft_enc[0];
                string encord = aft_enc[1];

                //元ファイルを最初の名前に戻す
                File.Move(after, org);

                //エンコード後のファイルがあれば成功フォルダへ
                string disteny = finishedOriginalDir;
                if (!File.Exists(encord))
                {
                    //失敗の場合はエラーフォルダへ
                    disteny = errorDir;
                }
                MoveDir(org, disteny);
            }
        }

        private void MoveDir(string terget,string outDir)
        {
            //ファイル名の取得:
            string neme = Path.GetFileName(terget);
            File.Move(terget, outDir + @"\" + neme);
        }
        public void MoveCompleatedFile()
        {
            if (!Properties.Settings.Default.MoveComp)
            {
                return;
            }
            string ourDir = Properties.Settings.Default.OutputDir + @"\completed";
            CleateDir(ourDir);
            foreach (var item in InputFiles)
            {
                try
                {
                    string fName = Path.GetFileName(item);
                    File.Move(item, ourDir + @"\" + fName);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        public void ClearFile(string filePath)
        {
            File.WriteAllText(filePath, string.Empty);
        }

        private void CleateDir(string path)
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
    }
}
