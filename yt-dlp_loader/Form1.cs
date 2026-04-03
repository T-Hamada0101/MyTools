using System;
using System.Threading.Tasks;

namespace yt_dlp_loader
{
    public partial class Form1 : Form
    {
        private readonly ApplicationService applicationService;
        private readonly FormActionRunner formActionRunner;
        private readonly FormSettingsMapper formSettingsMapper;
        private readonly ConsoleTextBoxWriter consoleTextBoxWriter;
        private readonly ConfigFileTextBoxViewer configFileTextBoxViewer;
        private readonly UrlListBoxAdapter urlListBoxAdapter;

        public Form1()
            : this(new ApplicationService()) { }

        internal Form1(ApplicationService applicationService)
        {
            this.applicationService = applicationService;
            InitializeComponent();
            formActionRunner = new FormActionRunner(this);
            AddDropEvents();
            formSettingsMapper = new FormSettingsMapper(
                textBoxExePath,
                textBoxUrlFile,
                textBoxDLFolder,
                checkBox1,
                textBox3,
                numericUpDown1,
                checkBox3,
                checkBox2,
                checkBox6,
                checkBox7,
                textBox1,
                checkBox4,
                textBox4,
                checkBox5,
                textBox5,
                comboBox1,
                applicationService.ResolveBrowserProfileName
            );
            consoleTextBoxWriter = new ConsoleTextBoxWriter(textBoxConsole);
            configFileTextBoxViewer = new ConfigFileTextBoxViewer(textBoxConfigFile);
            urlListBoxAdapter = new UrlListBoxAdapter(listBox1);

            // comboBox1 にブラウザ候補を追加
            comboBox1.Items.AddRange(applicationService.GetBrowserProfileDisplayNames());

            //this.TopMost = true;
        }
        private void LoadProperties()
        {
            var appSettings = applicationService.LoadSettings();
            formSettingsMapper.Apply(appSettings);

            // ユーザーAppDataフォルダの設定ファイルパスを取得
            var configFilePath = applicationService.GetMainConfigFilePath();
            configFileTextBoxViewer.Show(configFilePath);
            formActionRunner.ShowWarning(applicationService.GetBrowserProfilesLoadWarningMessage());
        }
        public void SaveSetting()
        {
            var appSettings = formSettingsMapper.Build();
            var context = applicationService.SaveSettings(appSettings);
            configFileTextBoxViewer.Show(context.ConfigFilePath);
        }

        private void AddDropEvents()
        {
            //------------D&D イベント登録------------
            this.AllowDrop = true;// ドラッグを受け入れる
            this.DragEnter += new DragEventHandler(ProcDragEnter);
            this.DragDrop += new DragEventHandler(ProcDragDrop);

        }
        public void ProcDragEnter(object? sender, DragEventArgs e)// D&D イベント
        {
            if (e.Data is null) return;
            //// ドラッグされたデータ形式を確認し URL とテキストとファイルを受け付ける
            if (
                e.Data.GetDataPresent("UniformResourceLocator")
                || e.Data.GetDataPresent(DataFormats.FileDrop)
                || e.Data.GetDataPresent(DataFormats.Text)
            )
            {
                e.Effect = DragDropEffects.Link;
            }
        }
        private void ProcDragDrop(object? sender, DragEventArgs e)// D&D イベント
        {
            urlListBoxAdapter.AddIfValid(UrlDragDropReader.Read(e));
        }
        private async void buttonStart_Click(object sender, EventArgs e)
        {
            var appSettings = formSettingsMapper.Build();
            var urls = urlListBoxAdapter.GetAll();
            if (urls.Count == 0)
            {
                return;
            }

            await formActionRunner.RunAsync(buttonStart, async () =>
            {
                consoleTextBoxWriter.Clear();
                var consoleHandler = consoleTextBoxWriter.CreateHandler();
                var context = await applicationService.StartBatchAsync(
                    appSettings,
                    urls,
                    consoleHandler,
                    consoleHandler
                );
                configFileTextBoxViewer.Show(context.ConfigFilePath);
                urlListBoxAdapter.Clear();
            });
        }

        // ブラウザ cookie の設定をドロップダウンで切り替える
        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            var context = applicationService.PreviewSettings(formSettingsMapper.Build());
            configFileTextBoxViewer.Show(context.ConfigFilePath);
        }

        /// <summary>
        /// 設定ファイル保存ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            SaveSetting();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// フォルダを開くボタン
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            formActionRunner.Run(() =>
            {
                applicationService.OpenDownloadDirectory(formSettingsMapper.Build().DownloadDirectory);
            });
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Open ブラウザボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            formActionRunner.Run(() =>
            {
                applicationService.OpenUrl(
                    "https://www.youtube.com/",
                    formSettingsMapper.Build().SelectBrowserProfile
                );
            });
        }

        /// <summary>
        /// Listの１番目を yt-dlp で実行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            if (!urlListBoxAdapter.HasItems())
            {
                return;
            }

            formActionRunner.Run(() =>
            {
                string url = urlListBoxAdapter.GetFirstOrEmpty();
                consoleTextBoxWriter.Clear();
                var consoleHandler = consoleTextBoxWriter.CreateHandler();
                var context = applicationService.StartSingle(
                    formSettingsMapper.Build(),
                    url,
                    consoleHandler,
                    consoleHandler
                );
                configFileTextBoxViewer.Show(context.ConfigFilePath);
                urlListBoxAdapter.RemoveFirst();
            });
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadProperties();
        }

        private async void buttonUpdate_Click(object sender, EventArgs e)
        {
            await formActionRunner.RunAsync(buttonUpdate, async () =>
            {
                await applicationService.UpdateBinaryAsync(formSettingsMapper.Build());
            });
        }
    }
}
