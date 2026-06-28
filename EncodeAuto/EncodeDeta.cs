using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
//using MimeTypes;
using HeyRed.Mime;
using System.Threading;
using Microsoft.VisualBasic.Devices;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Globalization;
//using System.Net.Mime;
//using Microsoft.WindowsAPICodePack.Shell;
//using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

namespace EncodeAuto
{
    internal class EncodeDeta
    {
        private const string AudioNormalizeFilter = "loudnorm=I=-16:TP=-1.5:LRA=11:print_format=summary";
        private const double AudioNormalizeTargetIntegrated = -16.0;
        private static readonly object AudioNormalizeSummaryLock = new object();

        /// <summary>
        /// 対象ファイルlist
        /// </summary>
        public List<string> InputFiles = new List<string>();
        public List<string> ErrorOrgFiles = new List<string>();

        /// <summary>
        /// 絵文字ファイル名(オリジナル)とエンコード後名
        /// </summary>
        public Dictionary<string, string> After_Encoded = new Dictionary<string, string>();
        /// <summary>
        /// 絵文字ファイル名(オリジナル),一時変更ファイル名*エンコード後名
        /// </summary>
        public Dictionary<string, string> Org_After_Encoded = new Dictionary<string, string>();

        public string batPath = "";
        public string bat2Path = "";
        public string arguments = "";
        public string safix = "";
        public bool isENcodeSameDir = false;
        public bool isMoveOrg = false;
        public string baseDir = "";
        public string CompressedOutDir = "";
        public string FatCompressedOut = "";
        public bool isAudioNormalize = false;
        public string audioNormalizeLogDir = "";

        private string inputDir;
        private string finishedOriginalDir;
        public string errorDir;
        public EncodeDeta(int sessionNum,List<string> _list, bool IsErrorLogOut = false, bool isMergeMode = false, bool isCutMode = false,List<string> cutTimes = null)
        {
            batPath = Properties.Settings.Default.ExePath;
            batPath = batPath.Replace(".bat", sessionNum.ToString() + ".bat");
            arguments = Properties.Settings.Default.Arguments;
            safix = Properties.Settings.Default.Safix;
            isENcodeSameDir = Properties.Settings.Default.SameDirOutput;
            isMoveOrg = Properties.Settings.Default.MoveComp;
            isAudioNormalize = Properties.Settings.Default.AudioNormalize;
            baseDir = Properties.Settings.Default.OutputDir;

            bat2Path = batPath.Replace(".bat", "2.bat");
            inputDir = baseDir + @"\Input";
            finishedOriginalDir = baseDir + @"\FinishedOriginal";
            CompressedOutDir = baseDir + @"\CompressedOut";
            FatCompressedOut = baseDir + @"\FatCompressedOut";
            errorDir = baseDir + @"\Error";
            audioNormalizeLogDir = baseDir + @"\AudioNormalizeLog";

            CleateDir(baseDir);
            CleateDir(inputDir);
            CleateDir(finishedOriginalDir);
            CleateDir(errorDir);
            CleateDir(FatCompressedOut);
            if (isAudioNormalize)
            {
                CleateDir(audioNormalizeLogDir);
            }
            ClearFile(batPath);
            ClearFile(bat2Path);
            if (isCutMode)
            {
                WriteBatFileForCut(_list, false, IsErrorLogOut, cutTimes);
            }
            else if (isMergeMode)
            {
                WriteBatFile(_list, false, IsErrorLogOut);
            }
            else//エンコードのみ
            {
                WriteBatFile(_list, false, IsErrorLogOut);
            }
        }

        private void WriteBatFileForCut(List<string> _list, bool renameEmoji, bool ErrorOutOn,List<string> cutTimes)
        {
            AppendTextToFile(batPath, @"chcp 65001", "UTF-8");//chcp 65001
            AppendTextToFile(batPath, @"cd /d %~dp0", "UTF-8");

            foreach (string pass in _list)
            {
                //if (!FileUtils.IsMovieFile(pass)) { continue; }

                InputFiles.Add(pass);

                string after = pass;
                if (renameEmoji)
                {
                    //絵文字除去
                    after = RenameEmojiFile(pass);
                }

                //拡張子なしのファイル名の取得:
                string _noExt = Path.GetFileNameWithoutExtension(after);

                //出力先分岐
                string? outDir;
                if (isENcodeSameDir)
                {
                    outDir = Path.GetDirectoryName(after);
                    //nullの場合はbaseDir
                    if (outDir is null)
                    {
                        outDir = baseDir;
                    }
                }
                else
                {
                    outDir = baseDir;
                }

                int num= 1;
                string startTime = "00:00:00";
                string endTime = "00:00:00";
                foreach (string time in cutTimes)
                {
                    endTime = time;
                    //エンコード後のファイル名
                    string encoded = outDir + @"\" + _noExt + "_" + num.ToString() + safix;
                    int n = 0;
                    while (File.Exists(encoded))
                    {
                        n++;
                        encoded = outDir + @"\" + _noExt + "_" + num.ToString() + "_" + n.ToString() + safix;
                    }
                    //出世魚の名前保存
                    //Org_After_Encoded.Add(pass, after + @"*" + encoded);



                    string arg = arguments.Replace(@"%input", @"""" + after + @"""");
                    arg = arg.Replace(@"%out", @"""" + encoded + @"""");

                    arg = arg.Replace(@"%st", startTime);
                    arg = arg.Replace(@"%ed", endTime);
                    arg = ApplyAudioNormalizeOption(arg, encoded);

                    if (ErrorOutOn)
                    {//エラーログ出力
                        arg = AddErrorLogOption(arg);
                    }

                    startTime = endTime;

                    //1ファイル分の処理書き込み
                    AppendTextToFile(batPath, arg, "UTF-8");
                    //ウエイト1秒
                    AppendTextToFile(batPath, @"timeout /t 1 /nobreak > nul", "UTF-8");
                    num++;
                }
            }
            //ウエイト2秒
            AppendTextToFile(batPath, @"timeout /t 2 /nobreak > nul", "UTF-8");
            //プロンプトを一時停止
            if (Properties.Settings.Default.Pause) { AppendTextToFile(batPath, "PAUSE", "UTF-8"); }
        }

        private void WriteBatFile(List<string> _list,bool renameEmoji,bool ErrorOutOn)
        {
            AppendTextToFile(batPath, @"chcp 65001", "UTF-8");//chcp 65001
            AppendTextToFile(batPath, @"cd /d %~dp0", "UTF-8");

            //マージモード時の音声ファイル
            string audioFile = "";

            //_list[0]に":"が含まれている場合はマージモード
            //_list[0]:動画ファイルパスを収納
            //audioFile:音声ファイルパスを収納
            if (_list[0].Contains(":::"))
            {
                string[] _files = _list[0].Split(":::");
                List<string> tmp = FileUtils.GetAllFilePath(_files);
                audioFile = tmp[1];
                if (renameEmoji)
                {
                    //絵文字除去
                    audioFile = RenameEmojiFile(audioFile);
                }
                _list = new List<string>();
                _list.Add(tmp[0]);//1本のみ
            }

            foreach (string pass in _list)
            {
                //if (!FileUtils.IsMovieFile(pass)) { continue; }

                InputFiles.Add(pass);

                string after = pass;
                if (renameEmoji)
                {
                    //絵文字除去
                    after = RenameEmojiFile(pass);
                }

                //拡張子なしのファイル名の取得:
                string _noExt = Path.GetFileNameWithoutExtension(after);

                //出力先分岐
                string? outDir;
                if (isENcodeSameDir)
                {
                    outDir = Path.GetDirectoryName(after);
                    //nullの場合はbaseDir
                    if (outDir is null)
                    {
                        outDir = baseDir;
                    }
                }
                else
                {
                    outDir = baseDir;
                }

                //エンコード後のファイル名
                string encoded = outDir + @"\" + _noExt + safix;
                int n = 0;
                while (File.Exists(encoded)) 
                {
                    n++;
                    encoded = outDir + @"\" + _noExt + "_" + n.ToString() + safix;
                } 
                
                //出世魚的に変わる名前保存
                Org_After_Encoded.Add(pass, after + @"*" + encoded);

                //コマンドの置き換え
                string arg = arguments;
                if (audioFile != ""  )
                {
                    //arg = @"ffmpeg  -i %input --cqp 34 --output-res 1280x720 --audio-copy -o %out";
                    //arg = @"ffmpeg -i %input -i %audio  -c copy -map 0:0 -map 1:1  %out";
                    //パスにスペースが含まれている場合、コマンドライン引数として正しく解釈されるように、
                    //パスを二重引用符で囲む必要があります。

                    arg = @"ffmpeg -i %input -i %audio -c copy -map 0:0 -map 1:1 %out";

                    // 置き換え時にパスを二重引用符で囲む
                    arg = arg.Replace("%audio", "\"" + audioFile + "\"");
                    //arg = arg.Replace("%input", "\"" + inputFile + "\"");
                    //arg = arg.Replace("%out", "\"" + outputFile + "\"");

                    //arg = arguments.Replace(@"%audio", @"""" + audioFile + @"""");

                    //音声ファイルの場合はエンコード後のファイル名を変更

                }
                
                arg = arg.Replace(@"%input", @"""" + after + @"""");
                arg = arg.Replace(@"%out", @"""" + encoded + @"""");
                arg = ApplyAudioNormalizeOption(arg, encoded);
                
                if (ErrorOutOn && (audioFile == ""))
                {//エラーログ出力
                    arg = AddErrorLogOption(arg);
                }

                //1ファイル分の処理書き込み
                AppendTextToFile(batPath, arg, "UTF-8");

                //ウエイト1秒
                AppendTextToFile(batPath, @"timeout /t 1 /nobreak > nul", "UTF-8");
            }
            //ウエイト2秒
            AppendTextToFile(batPath, @"timeout /t 2 /nobreak > nul", "UTF-8");
            //プロンプトを一時停止
            if (Properties.Settings.Default.Pause) { AppendTextToFile(batPath, "PAUSE", "UTF-8"); }

        }

        private string ApplyAudioNormalizeOption(string arg, string encoded)
        {
            if (!isAudioNormalize || !IsSupportedAudioNormalizeCommand(arg))
            {
                return arg;
            }

            // 音量を変えるため、音声コピーを外して音声だけ再エンコードする
            string normalizedArg = RemoveOption(arg, "--audio-copy");
            normalizedArg = AddOptionIfMissing(normalizedArg, "--audio-codec", "aac");
            normalizedArg = AddOptionIfMissing(normalizedArg, "--audio-bitrate", "192");
            normalizedArg = AddOptionIfMissing(normalizedArg, "--audio-filter", AudioNormalizeFilter);
            normalizedArg = AddAudioNormalizeLogOption(normalizedArg, encoded);
            return normalizedArg;
        }

        private string AddAudioNormalizeLogOption(string arg, string encoded)
        {
            if (HasOption(arg, "--log"))
            {
                return arg;
            }

            // 音量調整時だけ、loudnorm の summary をファイル別ログに残す
            string logPath = BuildAudioNormalizeLogPath(encoded);
            string logOptions = @"--log-level info --log " + @"""" + logPath + @"""";
            return InsertBeforeOutputOption(arg, logOptions);
        }

        private string BuildAudioNormalizeLogPath(string encoded)
        {
            string fileName = Path.GetFileNameWithoutExtension(encoded) + "_AudioNormalize.log";
            return Path.Combine(audioNormalizeLogDir, fileName);
        }

        private void AddAudioNormalizeSummary(string org, string encoded)
        {
            try
            {
                if (!isAudioNormalize)
                {
                    return;
                }

                string logPath = BuildAudioNormalizeLogPath(encoded);
                if (!File.Exists(logPath))
                {
                    return;
                }

                string logText = File.ReadAllText(logPath, new UTF8Encoding(false));
                double? inputIntegrated = ExtractLoudnormValue(logText, "Input Integrated");
                double? outputIntegrated = ExtractLoudnormValue(logText, "Output Integrated");
                if (inputIntegrated is null)
                {
                    return;
                }

                // 実際の出力LUFSが取れない場合は、目標LUFSとの差分を目安として出す
                double afterIntegrated = outputIntegrated ?? AudioNormalizeTargetIntegrated;
                double adjustment = afterIntegrated - inputIntegrated.Value;
                string signText = adjustment.ToString("+0.0;-0.0;0.0", CultureInfo.InvariantCulture);
                string outputText = outputIntegrated?.ToString("0.0", CultureInfo.InvariantCulture) ?? "目標値";
                string line = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                    + "\t" + Path.GetFileName(org)
                    + "\t約 " + signText + " dB"
                    + "\t(" + inputIntegrated.Value.ToString("0.0", CultureInfo.InvariantCulture)
                    + " LUFS -> " + outputText + " LUFS)"
                    + "\tlog: " + logPath;

                AppendAudioNormalizeSummaryLine(line);
            }
            catch
            {
                // 音量ログの要約に失敗しても、エンコード後処理は止めない
            }
        }

        private double? ExtractLoudnormValue(string logText, string label)
        {
            Match match = Regex.Match(
                logText,
                Regex.Escape(label) + @":\s*([+-]?\d+(?:\.\d+)?)",
                RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                return null;
            }

            if (double.TryParse(match.Groups[1].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
            {
                return value;
            }

            return null;
        }

        private void AppendAudioNormalizeSummaryLine(string line)
        {
            string summaryPath = Path.Combine(audioNormalizeLogDir, "AudioNormalizeSummary.txt");
            lock (AudioNormalizeSummaryLock)
            {
                if (!File.Exists(summaryPath) || new FileInfo(summaryPath).Length == 0)
                {
                    File.AppendAllText(summaryPath, "日時\tファイル\t音量調整量\tラウドネス\tログ\n", new UTF8Encoding(false));
                }

                File.AppendAllText(summaryPath, line + "\n", new UTF8Encoding(false));
            }
        }

        private bool IsSupportedAudioNormalizeCommand(string arg)
        {
            // QSVEncC/NVEncC は同じ音声オプションで音量フィルタを使える
            return arg.Contains("QSVEncC", StringComparison.OrdinalIgnoreCase)
                || arg.Contains("NVEncC", StringComparison.OrdinalIgnoreCase);
        }

        private string AddErrorLogOption(string arg)
        {
            string logPath = Path.Combine(errorDir, "errorlog.txt");

            if (IsFfmpegCommand(arg))
            {
                return AddFfmpegErrorLogOption(arg, logPath);
            }

            if (HasOption(arg, "--log"))
            {
                return arg;
            }

            // QSVEncC/NVEncC 系は専用のログオプションでエラーだけ残す
            return arg + @" --log-level error --log " + QuoteCommandValue(logPath);
        }

        private bool IsFfmpegCommand(string arg)
        {
            // コマンド先頭がffmpegなら、ffmpeg用のログ指定へ切り替える
            return Regex.IsMatch(
                arg.TrimStart(),
                @"^(""[^""]*ffmpeg(?:\.exe)?""|[^\s""]*ffmpeg(?:\.exe)?)(\s|$)",
                RegexOptions.IgnoreCase);
        }

        private string AddFfmpegErrorLogOption(string arg, string logPath)
        {
            string result = arg;

            if (!HasOption(result, "-hide_banner"))
            {
                result = InsertAfterCommandName(result, "-hide_banner");
            }

            if (!HasOption(result, "-loglevel") && !HasOption(result, "-v"))
            {
                result = InsertAfterCommandName(result, "-loglevel error");
            }

            if (!Regex.IsMatch(result, @"\s2>{1,2}", RegexOptions.IgnoreCase))
            {
                result += @" 2>> " + QuoteCommandValue(logPath);
            }

            return result;
        }

        private string InsertAfterCommandName(string arg, string optionText)
        {
            string trimmed = arg.TrimStart();
            int startIndex = arg.Length - trimmed.Length;

            if (trimmed.StartsWith("\""))
            {
                int endQuoteIndex = trimmed.IndexOf('"', 1);
                if (endQuoteIndex >= 0)
                {
                    return arg.Insert(startIndex + endQuoteIndex + 1, " " + optionText);
                }
            }

            Match commandName = Regex.Match(trimmed, @"^\S+");
            if (commandName.Success)
            {
                return arg.Insert(startIndex + commandName.Length, " " + optionText);
            }

            return arg + " " + optionText;
        }

        private string QuoteCommandValue(string value)
        {
            return @"""" + value + @"""";
        }

        private string RemoveOption(string normalizedArg, string optionName)
        {
            string pattern = @"(^|\s)" + Regex.Escape(optionName) + @"(?:[=:]\S+|\s+(?!-)\S+)?(?=\s|$)";
            return Regex.Replace(normalizedArg, pattern, " ", RegexOptions.IgnoreCase).Trim();
        }

        private string AddOptionIfMissing(string arg, string optionName, string optionValue)
        {
            if (HasOption(arg, optionName))
            {
                return arg;
            }

            return InsertBeforeOutputOption(arg, optionName + " " + optionValue);
        }

        private bool HasOption(string arg, string optionName)
        {
            string pattern = @"(^|\s)" + Regex.Escape(optionName) + @"(?=\s|=|$)";
            return Regex.IsMatch(arg, pattern, RegexOptions.IgnoreCase);
        }

        private string InsertBeforeOutputOption(string arg, string optionText)
        {
            Match outputOption = Regex.Match(arg, @"\s(?=-o\b|--output\b)", RegexOptions.IgnoreCase);
            if (outputOption.Success)
            {
                return arg.Insert(outputOption.Index, " " + optionText);
            }

            return arg + " " + optionText;
        }


        public void AppendTextToFile(string filePath, string text,string encode)
        {
            EncodingProvider provider = System.Text.CodePagesEncodingProvider.Instance;
            Encoding? encoding = null;
            switch (encode)
            {
                case "shift-jis":
                    encoding = provider.GetEncoding("shift-jis");
                    break;
                case "UTF-8":
                    //encoding = Encoding.UTF8;//BOMあり
                    encoding = new System.Text.UTF8Encoding(false);//BOMなし
                    break;
                default:
                    break;
            }
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
                string finalOrgLocation = org;
                bool IsError = false;
                //元ファイルの最終移動先
                string distenyOrg = org;

                if (File.Exists(encord))
                {
                    AddAudioNormalizeSummary(org, encord);

                    //エンコード後のファイルがあれば元ファイルを処理済みフォルダへ
                    distenyOrg = finishedOriginalDir;

                    //圧縮後ファイルの移動
                    if (!isENcodeSameDir)
                    {
                        string f = encord;
                        //長いファイル名を省略する
                        if (Properties.Settings.Default.ShortFileName)
                        {
                            //tergetのファイル名が50字を超えれば省略する
                            string fName = Path.GetFileName(f);
                            if (fName.Length > 60)
                            {
                                //拡張子を除いたファイル名のみ取得
                                fName = Path.GetFileNameWithoutExtension(f);
                                //30文字に短縮

                                fName = fName.Substring(0, 50);
                                //拡張子を付与
                                fName += Path.GetExtension(f);

                            }
                            string sh = Path.GetDirectoryName(f) + @"\" + fName;
                            //tergetを短縮したファイル名に変更
                            File.Move(f, sh);
                            f = sh;
                        }


                        //圧縮効果ありの場合は
                        if (IsSizeCompressed(org, f))
                        {
                            //エンコード後のファイルを圧縮完了フォルダへ
                            MoveDir(f, CompressedOutDir);
                        }
                        else
                        {
                            //エンコード後のファイルを肥大圧縮完了フォルダへ
                            MoveDir(f, FatCompressedOut);
                        }
                    }
                }
                else
                {
                    IsError = true;
                    if (isMoveOrg)
                    {
                        //失敗の場合は元ファイルをエラーフォルダへ
                        distenyOrg = errorDir;
                    }
                    else
                    {
                        distenyOrg = org;
                    }
                    
                }
                //元ファイルの移動処理
                if (isMoveOrg)
                {
                    finalOrgLocation =MoveDir(org, distenyOrg);
                }

                //エラーの場合はエラーリストへ追加
                if (IsError)
                {
                    ErrorOrgFiles.Add(finalOrgLocation);
                }
            }
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
                    Thread.Sleep(50);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        public void ClearFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                //ファイルがなければ作成
                filePath = Path.GetFullPath(filePath);
                FileStream fs = File.Create(filePath);
                fs.Close();

                //File.Create(filePath).Close();
            }
            File.WriteAllText(filePath, string.Empty);
        }

        private void CleateDir(string path)
        {
            FileUtils.CleateDir(path);
        }

        /// <summary>
        /// Afterのサイズが圧縮されていればTrue
        /// </summary>
        /// <returns></returns>
        private bool IsSizeCompressed(string org, string after)
        {
            return FileUtils.IsSmaller(org, after); ;
        }

        /// <summary>
        /// 指定DirへMove(重複時は上書き)
        /// </summary>
        /// <param name="terget"></param>
        /// <param name="outDir"></param>
        /// <returns>orgFileの最終所在地</returns>
        private string MoveDir(string terget, string outDir)
        {
            

            return FileUtils.MoveDir(terget, outDir);
        }

    }
}
