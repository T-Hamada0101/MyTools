using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;

namespace RegEditTools
{
    public partial class Form1 : Form
    {
        // Target Key Path (Tab 1 - HKCU)
        private const string TargetKeyPath =
            @"Software\Classes\Local Settings\Software\Microsoft\Windows\Shell\Bags\AllFolders\Shell";
        private const string TargetValueName = "FolderType";
        private const string TargetValueData = "NotSpecified";
        private const string BackupName = "Win11FolderTypeBackup";

        // Target Key Path (Tab 2 - HKLM AppXSvc)
        private const string AppXKeyPath = @"SYSTEM\CurrentControlSet\Services\AppXSvc";
        private const string AppXValueName = "Start";
        private const string AppXDisabled = "4";
        private const string AppXManual = "3";
        private const string AppXBackupName = "AppXSvcBackup";

        public Form1()
        {
            InitializeComponent();
            Log("アプリケーションが起動しました。");
            Log2("アプリケーションが起動しました。");
        }

        private void Log(string message)
        {
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
        }

        private void Log2(string message)
        {
            txtLog2.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
        }

        // --- Tab 1 Handlers ---

        private void btnCheck_Click(object sender, EventArgs e)
        {
            Log("現在のレジストリ状態を確認しています...");
            string? value = RegistryHelper.ReadValue(TargetKeyPath, TargetValueName);

            if (value == null)
            {
                Log($"現在の設定: 未設定 (値が存在しません)");
            }
            else
            {
                Log($"現在の設定: {value}");
                if (value == TargetValueData)
                {
                    Log("★ 推奨設定 (NotSpecified) になっています。");
                }
                else
                {
                    Log("! 推奨設定ではありません。");
                }
            }

            if (BackupManager.BackupExists(BackupName))
            {
                Log("バックアップ: 存在します。");
            }
            else
            {
                Log("バックアップ: 存在しません。");
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                // Backup if not exists
                if (!BackupManager.BackupExists(BackupName))
                {
                    Log("バックアップを作成しています...");
                    BackupManager.SaveBackup(BackupName, TargetKeyPath, TargetValueName);
                    Log("バックアップを作成しました。");
                }

                Log("設定を適用しています...");
                RegistryHelper.WriteValue(TargetKeyPath, TargetValueName, TargetValueData);
                Log($"設定完了: {TargetKeyPath}\\{TargetValueName} = {TargetValueData}");
                Log("変更を反映させるには再起動が必要です。");
                MessageBox.Show(
                    "設定を適用しました。\n変更を反映するにはPCの再起動が必要です。",
                    "完了",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                Log($"エラーが発生しました: {ex.Message}");
                MessageBox.Show(
                    $"エラー: {ex.Message}",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            try
            {
                if (!BackupManager.BackupExists(BackupName))
                {
                    Log("復元可能なバックアップが見つかりません。");
                    MessageBox.Show(
                        "バックアップが見つかりません。",
                        "エラー",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                Log("バックアップから復元しています...");
                BackupManager.RestoreBackup(BackupName);
                Log("復元が完了しました。");
                Log("変更を反映させるには再起動が必要です。");
                MessageBox.Show(
                    "設定を元に戻しました。\n変更を反映するにはPCの再起動が必要です。",
                    "完了",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                Log($"エラーが発生しました: {ex.Message}");
                MessageBox.Show(
                    $"エラー: {ex.Message}",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnOpenRegedit_Click(object sender, EventArgs e)
        {
            try
            {
                // RegEditのLastKeyを設定して、特定のキーを開いた状態で起動する
                string regeditLastKeyPath =
                    @"Software\Microsoft\Windows\CurrentVersion\Applets\RegEdit";
                string fullPath = @"Computer\HKEY_CURRENT_USER\" + TargetKeyPath;

                // regedit起動準備のため確認ダイアログはスキップ
                RegistryHelper.WriteValue(
                    regeditLastKeyPath,
                    "LastKey",
                    fullPath,
                    showConfirmation: false
                );

                Log("RegEditを起動しています...");
                string regeditPath = System.IO.Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Windows),
                    "regedit.exe"
                );
                System.Diagnostics.Process.Start(regeditPath);
            }
            catch (Exception ex)
            {
                Log($"RegEditの起動に失敗しました: {ex.Message}");
                MessageBox.Show(
                    $"エラー: {ex.Message}",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // --- Tab 2 Handlers (AppXSvc) ---

        private void btnCheck2_Click(object sender, EventArgs e)
        {
            Log2("AppXSvcの状態を確認しています(HKLM)...");
            // HKLM
            string? value = RegistryHelper.ReadValue(
                Registry.LocalMachine,
                AppXKeyPath,
                AppXValueName
            );

            if (value == null)
            {
                Log2($"現在の設定: 値が存在しません (エラーの可能性あり)");
            }
            else
            {
                string status = value switch
                {
                    AppXDisabled => "無効 (4) ★推奨",
                    AppXManual => "手動 (3)",
                    "2" => "自動 (2)",
                    _ => $"{value}",
                };
                Log2($"現在の設定: {status}");
            }

            if (BackupManager.BackupExists(AppXBackupName))
            {
                Log2("バックアップ: 存在します。");
            }
            else
            {
                Log2("バックアップ: 存在しません。");
            }
        }

        private void btnApply2_Click(object sender, EventArgs e)
        {
            try
            {
                // Backup if not exists
                if (!BackupManager.BackupExists(AppXBackupName))
                {
                    Log2("バックアップを作成しています(HKLM)...");
                    BackupManager.SaveBackup(
                        AppXBackupName,
                        Registry.LocalMachine,
                        AppXKeyPath,
                        AppXValueName
                    );
                    Log2("バックアップを作成しました。");
                }

                Log2("AppXSvcを無効化(Start=4)しています...");
                RegistryHelper.WriteValue(
                    Registry.LocalMachine,
                    AppXKeyPath,
                    AppXValueName,
                    AppXDisabled
                );
                Log2($"設定完了: {AppXKeyPath}\\{AppXValueName} = {AppXDisabled}");
                Log2("変更を反映させるには再起動が必要です。");
                MessageBox.Show(
                    "設定を適用しました。\n変更を反映するにはPCの再起動が必要です。",
                    "完了",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                Log2($"エラーが発生しました: {ex.Message}");
                MessageBox.Show(
                    $"エラー: {ex.Message}",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnRestore2_Click(object sender, EventArgs e)
        {
            try
            {
                if (!BackupManager.BackupExists(AppXBackupName))
                {
                    Log2("復元可能なバックアップが見つかりません。");
                    MessageBox.Show(
                        "バックアップが見つかりません。",
                        "エラー",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                Log2("バックアップから復元しています...");
                BackupManager.RestoreBackup(AppXBackupName);
                Log2("復元が完了しました。");
                Log2("変更を反映させるには再起動が必要です。");
                MessageBox.Show(
                    "設定を元に戻しました。\n変更を反映するにはPCの再起動が必要です。",
                    "完了",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                Log2($"エラーが発生しました: {ex.Message}");
                MessageBox.Show(
                    $"エラー: {ex.Message}",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnOpenRegedit2_Click(object sender, EventArgs e)
        {
            try
            {
                // RegEditのLastKeyを設定
                string regeditLastKeyPath =
                    @"Software\Microsoft\Windows\CurrentVersion\Applets\RegEdit";
                string fullPath = @"Computer\HKEY_LOCAL_MACHINE\" + AppXKeyPath;

                // regedit起動準備 (LastKey is in HKCU)
                RegistryHelper.WriteValue(
                    Registry.CurrentUser,
                    regeditLastKeyPath,
                    "LastKey",
                    fullPath,
                    showConfirmation: false
                );

                Log2("RegEditを起動しています...");
                string regeditPath = System.IO.Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Windows),
                    "regedit.exe"
                );
                System.Diagnostics.Process.Start(regeditPath);
            }
            catch (Exception ex)
            {
                Log2($"RegEditの起動に失敗しました: {ex.Message}");
                MessageBox.Show(
                    $"エラー: {ex.Message}",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
