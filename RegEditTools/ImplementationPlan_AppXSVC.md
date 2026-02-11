# AppXSVC 停止機能実装計画

## 概要
AppXSVC (AppX Deployment Service) を停止するための機能を既存のRegEditツールに追加します。これにより、ユーザーはGUIから簡単にサービスの無効化や状態確認を行うことができます。

## 変更内容

### 1. RegistryHelper.cs の改修
*   **目的**: HKEY_LOCAL_MACHINE (HKLM) へのアクセスをサポートするため。
*   **変更点**:
    *   `ReadValue`, `WriteValue`, `DeleteValue` メソッドに `RegistryKey root` パラメータを受け取るオーバーロードを追加。
    *   既存のメソッドは `Registry.CurrentUser` をデフォルトとして使用するように変更（後方互換性維持）。
    *   `ShowConfirmationDialog` でルートキー名を表示するように変更。

### 2. BackupManager.cs の改修
*   **目的**: バックアップ時にルートキーの情報も保持し、正しいハイブに復元できるようにするため。
*   **変更点**:
    *   `RegBackupData` クラスに `RootKeyName` プロパティを追加。
    *   `SaveBackup` でルートキー名も JSON に保存。
    *   `RestoreBackup` で JSON からルートキー名を読み取り、適切な `RegistryKey` (HKLM, HKCU等) を選択して復元動作を実行。

### 3. Form1.Designer.cs の改修
*   **目的**: 新機能用のUIタブを追加。
*   **変更点**:
    *   `tabControl1` に `tabPage2` ("AppXSVC停止") を追加。
    *   既存のタブと同様のレイアウトでボタン (`btnCheck2`, `btnApply2`, `btnRestore2`, `btnOpenRegedit2`)、ログボックス (`txtLog2`)、ラベル (`infoLabel2`) を配置。

### 4. Form1.cs の改修
*   **目的**: UIイベントとロジックの接続。
*   **変更点**:
    *   AppXSVC 用の定数を定義 (`AppXKeyPath`, `AppXValueName`, `AppXDisabled`="4" 等)。
    *   新しいタブのボタンイベントハンドラを実装。
        *   **状態確認**: HKLMの該当キーを読み取り、現在の設定 (無効/手動/自動) を表示。
        *   **修正を適用**: 現在の値をバックアップ後、値を "4" (無効) に変更。
        *   **元に戻す**: バックアップから元の設定を復元。
        *   **RegEditで開く**: レジストリエディタを HKEY_LOCAL_MACHINE の該当キーで開く。

## 使用方法
1.  タブ「AppXSVC停止」を選択します。
2.  「状態確認」ボタンで現在の設定を確認します。
3.  「修正を適用」ボタンを押すと、AppXSVCが無効化されます（要再起動）。
4.  必要に応じて「元に戻す」ボタンで以前の状態に戻せます。
