using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace EncodeAuto
{
    public static class Regexs
    {
        public static string CleanInput(string strIn)
        {
            // Replace invalid characters with empty strings.
            try
            {
                //return Regex.Replace(strIn, @"[^\\w\\.@-]", "", RegexOptions.None, TimeSpan.FromSeconds(1.5));
                return Regex.Replace(strIn, @"[^\\!/g]", "");
                //@"[\u002F]"スラッシュ
                //@"[\u005C]"バックスラッシュ
            }
            // If we timeout when replacing invalid characters,
            // we should return Empty.
            catch (RegexMatchTimeoutException)
            {
                return String.Empty;
            }
        }

        public static Dictionary<string, string> TrancerateUnicodeList(string input)
        {
            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            foreach (char c in input)
            {
                try
                {
                    keyValues.Add(c.ToString(), $"U+{((int)c).ToString("X4")}");
                }
                catch (Exception)
                {

                }
            }
            return keyValues;
        }
        /// <summary>
        /// 入力文字制限
        /// </summary>
        /// <param name="inputString">入力文字列</param>
        /// <returns>バリデーション後の文字列</returns>
        public static string InputValueValidate(string inputString)
        {
            Dictionary<string, string> keyValues = TrancerateUnicodeList(inputString);
            // 正規表現リストに基づき
            for (int i = 0; i < InputBlockRegexList.Count; i++)
            {
                inputString = Regex.Replace(inputString, InputBlockRegexList[i], "");
                //inputString = CleanInput(inputString);
            }
            return inputString;
        }
        /// <summary>
        /// 入力制限をかける正規表現文字列のリスト
        /// </summary>
        private static readonly List<string> InputBlockRegexList = new List<string>()
        {
            "[\u2000-\u2FFF]",                // 多くの記号
            "[\u23F3]",                       // (HOURGLASS WITH FLOWING SAND)
            "[\u25FD-\u25FE]",                // (WHITE MEDIUM SMALL SQUAR)(BLACK MEDIUM SMALL SQUARE)
            "[\u2600-\u26FF]",                // その他の記号(Miscellaneous Symbols)
            "[\u2705]",                       // (WHITE HEAVY CHECK MARK)
            "[\u2714]",                       // (HEAVY CHECK MARK)
            "[\u2764]",                       // (HEAVY BLACK HEART)
            "[\u274C]",                       // (CROSS MARK)
            "[\u274E]",                       // (NEGATIVE SQUARED CROSS MARK)
            "[\u2753-\u2755]",                // (BLACK QUESTION MARK ORNAMENT/WHITE QUESTION MARK ORNAMENT/WHITE EXCLAMATION MARK ORNAMENT)
            "[\u2757]",                       // (HEAVY EXCLAMATION MARK SYMBOL)
            "[\u27BF]",                       // (DOUBLE CURLY LOOP)
            "[\u2795-\u2797]",                // (HEAVY PLUS SIGN)・(HEAVY MINUS SIGN)・(HEAVY DIVISION SIGN)
            "[\u2B50]",                       // (WHITE MEDIUM STAR)
            "[\u2B55]",                       // (HEAVY LARGE CIRCLE)
            "[\u2B1B-\u2B1C]",                // (BLACK LARGE SQUARE)・(WHITE LARGE SQUARE)
            "[\u0530-\u058F]",                // アルメニア文字(Armenian)
            "[\u0A00-\u0A7F]",                // グルムキー文字(Gurmukhi)
            "[\uD800-\uDBFF][\uDC00-\uDFFF]", // サロゲートペア(Surrogates)
            "[\uE000-\uF8FF]",                // 私用領域(Private Use Area)
            "[\u29F8]",                       // スラッシュ
            "[\u0021]",                       // !
            "[\u003F]",                       // ?
            "[\u203C]",                       // ‼
            "[\u2B50]",                       // ⭐
        };
    }
}
