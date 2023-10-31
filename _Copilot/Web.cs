using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;
using System.Threading;
//using ReturnedItemCheck.Excel;
//using ReturnedItemCheck.Utils;
using AngleSharp.Html.Parser;
using AngleSharp.Html.Dom;
using AngleSharp.Dom;
using WebDriverManager.DriverConfigs;
using System.IO;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
//using ReturnedItemCheck.WebContents;
using SnlData;
using System.Net.NetworkInformation;

namespace _Copilot
{
    internal class Web
    {
        public static bool PingHost(string nameOrAddress)
        {
            bool pingable = false;
            Ping pinger = new Ping();

            try
            {
                PingReply reply = pinger.Send(nameOrAddress);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }

            return pingable;
        }
    }

    /// <summary>
    /// Edge用 WebDriverWrapper
    /// </summary>
    public class Edge : IDisposable
    {
        /// <summary>
        /// Webdriver取得用
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        IntPtr? windowHandle = null;

        WebDriver edge = null;
        public static string WebdriverDir = Paths.ExeFolder + @"\Edge";

        public Edge()
        {
            //WebdriverDirの作成
            CreateDir(WebdriverDir);

            //フォルダが3個以上なら古いフォルダを削除
            //全てのDLしたドライバーを保存して行きフォルダが増えて行くため
            DeleteOldDirectoeiesSortName(WebdriverDir, 3);

            //Edgeのversionに合わせWebDriverの自動更新
            // UpdateDriver:インストールされているバージョンのWebDriverをSetUp
            EdgeConfig edgeConfig = new EdgeConfig();
            //string downloadDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads";
            //string downloadDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            new DriverManager(WebdriverDir).SetUpDriver(edgeConfig, VersionResolveStrategy.MatchingBrowser, Architecture.Auto);
            //exeのDirにEdgeフォルダが作成されWebDriverを保管
            //base\Edge\112.0.1722.39\x64\msedgedriver.exe(16MB程度)
        }

        /// <summary>
        /// ドライバーのフォルダが指定個数個以上なら古いフォルダを削除
        /// </summary>
        /// <param name="terget"></param>
        void DeleteOldDirectoeiesSortName(string folderPath, int count)
        {
            if (!Directory.Exists(folderPath))
            {
                return;
            }
            DirectoryInfo di = new DirectoryInfo(folderPath);
            var directoeies = di.GetDirectories("*").OrderByDescending(f => f.Name).ToList();//降順ソート
            for (int i = 0; i < directoeies.Count; i++)
            {
                if (i >= count)
                {
                    //Console.WriteLine(directoeies[i].FullName);
                    Directory.Delete(directoeies[i].FullName, true);
                }
            }
        }

        /// <summary>
        /// フォルダを作成(既にある場合はスルーされる)
        /// </summary>
        void CreateDir(string path)
        {
            Directory.CreateDirectory(path);
        }

        public static void OpenByEdge(string uri)
        {
            System.Diagnostics.Process.Start(@"C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe\", uri);
        }

        private void BootEdge(bool isHeadlessMode = true)
        {
            // DriverService起動時のコマンドプロンプトを表示しない
            EdgeDriverService driverService = EdgeDriverService.CreateDefaultService();
            if (isHeadlessMode)
            {
                driverService.HideCommandPromptWindow = true;
            }

            //タイムアウト設定(デフォ:20秒) <=クリック(Submit)時のタイムアウトは別（30秒）
            //driverService.InitializationTimeout = new TimeSpan(1, 3, 59);//3min
            //Console.WriteLine(driverService.InitializationTimeout);

            EdgeOptions options = new EdgeOptions();
            if (isHeadlessMode)
            {
                options.AddArgument("--headless");//非表示(ヘッドレストモード)
            }

            //タイムアウト(デフォ:60秒)
            TimeSpan timeSpan = new TimeSpan(0, 6, 0);//6min


            try
            {
                //EdgeDriver(EdgeDriverService service, EdgeOptions options, TimeSpan commandTimeout);
                edge = new EdgeDriver(driverService, options, timeSpan);
            }
            catch (Exception e)
            {
                switch (e.HResult)
                {
                    case -2146233088:
                        //Unable to obtain MicrosoftEdge using Selenium Manager
                        //←2バイト文字に非対？なのでpathに2byte文字が含まれているかも
                        MessageBox.Show("Webドライバーが見当たりません。" +
                            "\n実行フォルダのフルパスに日本語が含まれている可能性があります、" +
                            "日本語が含まれないフォルダに移動してから再度実行してみて下さい");
                        return;
                        break;
                    default:
                        throw;
                }
            }
            windowHandle = GetForegroundWindow();
        }

        public bool IsAliveEdge()
        {
            if (edge == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool ConnectVisivle(string url)
        {
            return Connect(url, false);
        }

        /// <summary>
        /// ページを取得
        /// </summary>
        /// <param name="url"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public bool Connect(string url, bool isHeadlessMode = true)
        {
            bool result = false;
            if (edge == null)
            {
                BootEdge(isHeadlessMode);
                if (edge == null) return result;
            }

            if (url == "")
            {
                throw new EdgeExceptionEx("入力範囲外です");
                //return false;
                //DisplayMessageAndQuit("入力範囲外");
            }
            if (edge == null)
            {
                BootEdge(isHeadlessMode);
                if (edge == null) return result;
            }
            try
            {
                edge.Navigate().GoToUrl(url);
                result = true;
            }
            catch (Exception e)
            {
                throw new EdgeExceptionEx("接続出来ません");
                //return false;
                //DisplayMessageAndQuit(string.Format("接続出来ません\r\n\r\n{0}\r\n\r\n{1}", e.HResult,e.Message ));
                //Quit();
            }
            //正しく接続できているかの確認
            if (edge.Url != url)
            {
                throw new EdgeExceptionEx("接続出来ません");
                //return false;
                //DisplayMessageAndQuit("接続出来ません");
                //Quit();
            }
            //DNS_PROBE_FINISHED_NXDOMAIN



            return result;
        }

        public List<string> GetValues(string id, string tagname)
        {
            List<string> list = new List<string>();
            try
            {
                IWebElement _elm = edge.FindElement(By.Id(id));
                ReadOnlyCollection<IWebElement> _elms = _elm.FindElements(By.TagName(tagname));
                //ReadOnlyCollection<IWebElement> _elms = edge.FindElement(By.Id(id)).op;
                foreach (IWebElement _e in _elms)
                {
                    string _val = _e.GetAttribute("value");
                    list.Add(_val);
                }
                //項目数は219以上で遅い
                return list;
            }
            catch (Exception e)
            {
                throw new EdgeExceptionEx(string.Format("エレメント[{0}]が取得出来ません", id));
                //DisplayMessageAndQuit(string.Format("エレメント[{0}]が取得出来ません\r\n\r\n{1}\r\n\r\n{2}", id, e.HResult, e.Message));
                //throw;
            }
        }

        public List<string> GetValueAndTexts(string id, string tagname)
        {
            List<string> list = new List<string>();
            try
            {
                IWebElement _elm = edge.FindElement(By.Id(id));
                ReadOnlyCollection<IWebElement> _elms = _elm.FindElements(By.TagName(tagname));
                //ReadOnlyCollection<IWebElement> _elms = edge.FindElement(By.Id(id)).op;
                foreach (IWebElement _e in _elms)
                {
                    string _val = _e.GetAttribute("value");
                    string _txt = _e.Text;
                    list.Add(_val + "," + _txt);
                }

                return list;
            }
            catch (Exception e)
            {
                throw new EdgeExceptionEx(string.Format("エレメント[{0}]が取得出来ません", id));
                //DisplayMessageAndQuit(string.Format("エレメント[{0}]が取得出来ません\r\n\r\n{1}\r\n\r\n{2}", id, e.HResult, e.Message));
                //throw;
            }
        }

        public string GetValue(string id)
        {
            try
            {
                IWebElement _elm = edge.FindElement(By.Id(id));
                string _val = _elm.GetAttribute("value");
                return _val;
            }
            catch (Exception e)
            {
                throw new EdgeExceptionEx(string.Format("エレメント[{0}]が取得出来ません", id));
                //DisplayMessageAndQuit(string.Format("エレメント[{0}]が取得出来ません\r\n\r\n{1}\r\n\r\n{2}", id, e.HResult, e.Message));
                //throw;
            }
        }

        public string GetText(string id)
        {
            IWebElement _elm = edge.FindElement(By.Id(id));
            return _elm.Text;
        }

        public void SetValue(string id, string value)
        {
            edge.FindElement(By.Id(id)).SendKeys(value);
        }

        public void Click(string id)
        {
            try
            {
                edge.FindElement(By.Id(id)).Click();
            }
            catch (Exception e)
            {
                throw new EdgeExceptionEx(string.Format("書込みタイムアウト{0}", id));

                //DisplayMessageAndQuit(string.Format("タイムアウト\r\n\r\n{0}\r\n\r\n{1}", e.HResult, e.Message));
                //var waitTask = Task.Delay(120000);    // msec
                //waitTask.Wait();
                //edgeのReBoot必要か？
            }


            /*
             * エラー：タイムアウト時
            OpenQA.Selenium.WebDriverException
            HResult=0x80131500
            Message=The HTTP request to the remote WebDriver server for URL http://localhost:55577/session/37431725639192e7a3e3214b10d9e700/element/8defb716-36ae-48ca-a62c-7ac67440c887/click timed out after 60 seconds.
            Source=WebDriver
            スタック トレース:
             場所 OpenQA.Selenium.Remote.HttpCommandExecutor.Execute(Command commandToExecute)
             場所 OpenQA.Selenium.Remote.DriverServiceCommandExecutor.Execute(Command commandToExecute)
             場所 OpenQA.Selenium.WebDriver.Execute(String driverCommandToExecute, Dictionary`2 parameters)
             場所 OpenQA.Selenium.WebDriver.InternalExecute(String driverCommandToExecute, Dictionary`2 parameters)
             場所 OpenQA.Selenium.WebElement.Execute(String commandToExecute, Dictionary`2 parameters)
             場所 OpenQA.Selenium.WebElement.Click()
             場所 Web.Edge.Click(String id) (C:\Users\user\Source\Repos\SnlProject\ZaikoSysAccess\ZaikoSysAccess\Web\Edge.cs):行 83
             場所 Web.ZaikoSys.Rental_setzaiko_meisai.Write(Edge edge, Form1 F, Int32 index) (C:\Users\user\Source\Repos\SnlProject\ZaikoSysAccess\ZaikoSysAccess\Web\ZaikoSys\Rental-setzaiko-meisai.cs):行 93
             場所 Web.ZaikoSys.Rental_setzaiko_meisai.Write(Edge edge, Form1 F) (C:\Users\user\Source\Repos\SnlProject\ZaikoSysAccess\ZaikoSysAccess\Web\ZaikoSys\Rental-setzaiko-meisai.cs):行 82
             場所 ZaikoSysAccess.Form1.button_書込み_Click(Object sender, EventArgs e) (C:\Users\user\Source\Repos\SnlProject\ZaikoSysAccess\ZaikoSysAccess\Form1.cs):行 114
             場所 System.Windows.Forms.Control.OnClick(EventArgs e)
             場所 System.Windows.Forms.Button.OnClick(EventArgs e)
             場所 System.Windows.Forms.Button.OnMouseUp(MouseEventArgs mevent)
             場所 System.Windows.Forms.Control.WmMouseUp(Message& m, MouseButtons button, Int32 clicks)
             場所 System.Windows.Forms.Control.WndProc(Message& m)
             場所 System.Windows.Forms.ButtonBase.WndProc(Message& m)
             場所 System.Windows.Forms.Button.WndProc(Message& m)
             場所 System.Windows.Forms.Control.ControlNativeWindow.OnMessage(Message& m)
             場所 System.Windows.Forms.Control.ControlNativeWindow.WndProc(Message& m)
             場所 System.Windows.Forms.NativeWindow.DebuggableCallback(IntPtr hWnd, Int32 msg, IntPtr wparam, IntPtr lparam)
             場所 System.Windows.Forms.UnsafeNativeMethods.DispatchMessageW(MSG& msg)
             場所 System.Windows.Forms.Application.ComponentManager.System.Windows.Forms.UnsafeNativeMethods.IMsoComponentManager.FPushMessageLoop(IntPtr dwComponentID, Int32 reason, Int32 pvLoopData)
             場所 System.Windows.Forms.Application.ThreadContext.RunMessageLoopInner(Int32 reason, ApplicationContext context)
             場所 System.Windows.Forms.Application.ThreadContext.RunMessageLoop(Int32 reason, ApplicationContext context)
             場所 System.Windows.Forms.Application.Run(Form mainForm)
             場所 ZaikoSysAccess.Program.Main() (C:\Users\user\Source\Repos\SnlProject\ZaikoSysAccess\ZaikoSysAccess\Program.cs):行 19

              内部例外 1:
              TaskCanceledException: タスクが取り消されました。

             */
        }

        public void Checked(string id)
        {
            IWebElement checkbox = edge.FindElement(By.Id(id));
            if (!checkbox.Selected)
            {
                checkbox.Click();
            }
        }

        public void UnChecked(string id)
        {
            IWebElement checkbox = edge.FindElement(By.Id(id));
            if (checkbox.Selected)
            {
                checkbox.Click();
            }
        }

        public void DisplayMessageAndQuit(string message = "Edgeを終了します")
        {
            //↓要using
            //System.Windows.Forms.MessageBox.Show(message);
            Quit();
        }

        public void Quit()
        {
            //edge.Close();//アクティブになっているタブを終了
            if (edge != null)
            {
                try
                {
                    edge.Quit();//すべてのタブを閉じてブラウザを終了
                }
                catch (Exception)
                {

                    //throw;
                }

            }
        }

        public void Dispose()
        {
            if (edge is null)
            {
                return;
            }
            edge.Quit();
        }

        public bool IsDisposed()
        {
            if (edge == null)
            {
                return true;
            }
            return false;
        }
        //表示しているページのソースコードを取得
        public string GetPageSource()
        {
            return edge.PageSource;
        }

        public void GetTableExcelCOM(string id)
        {
            //ExcelCom excel = new ExcelCom(Paths.Downloads);
        }

    }
}

