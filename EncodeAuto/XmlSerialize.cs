using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncodeAuto
{
    //XMLファイルに保存するオブジェクトのためのクラス
    public class PresetClass
    {
        public string[] presetName = new string[12];
        public string[] batPath = new string[12];
        public string[] argment = new string[12];
        public string[] safix = new string[12];
        public string[] outDir = new string[12];
        public bool[] afterOriginMove = new bool[12];
        public bool[] pauseCMD = new bool[12];
        public bool[] sameDirOutput = new bool[12];
        public bool[] shortFileName = new bool[12];
    }
    internal class XmlSerialize
    {
        //保存先のファイル名
        static string fileName = System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + @"\Preset.xml";

        //SampleClassオブジェクトをXMLファイルに保存する
        public static void Save(PresetClass @class)
        {
            IfFileNotExistsThenCreate(fileName);
            //XmlSerializerオブジェクトを作成
            //オブジェクトの型を指定する
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(PresetClass));
            //書き込むファイルを開く（UTF-8 BOM無し）
            System.IO.StreamWriter sw = new System.IO.StreamWriter(
                fileName, false, new System.Text.UTF8Encoding(false));
            //シリアル化し、XMLファイルに保存する
            serializer.Serialize(sw, @class);
            //ファイルを閉じる
            sw.Close();
        }

        //XMLファイルをSampleClassオブジェクトに復元する
        public static PresetClass? Load()
        {
            if (!File.Exists(fileName))return null;
            if(new FileInfo(fileName).Length < 1)return null;

            //XmlSerializerオブジェクトを作成
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(PresetClass));
            //読み込むファイルを開く
            System.IO.StreamReader sr = new System.IO.StreamReader(
                fileName, new System.Text.UTF8Encoding(false));
            //XMLファイルから読み込み、逆シリアル化する
            var _tmp = serializer.Deserialize(sr);
            //ファイルを閉じる
            sr.Close();

            if (_tmp is null) return null;
            PresetClass obj = (PresetClass)_tmp;

            return obj;
        }

        private static void IfFileNotExistsThenCreate(string path)
        {
            if (!File.Exists(path))
            {
                FileStream fs = File.Create(path);
                fs.Close();
            }
        }
    }
}
