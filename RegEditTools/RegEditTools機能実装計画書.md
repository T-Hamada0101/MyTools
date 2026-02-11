# RegEdit ツール機能実装計画書

この計画書は、Windows 11 のフォルダ種類自動判別を無効化する機能を備えたレジストリ編集ツールの実装手順を概説します。

## ユーザーレビュー必須事項

> [!IMPORTANT]
> このツールは Windows レジストリを変更します。ツール自体にバックアップ機能を作成しますが、コードを実行する前にレジストリの手動バックアップを強く推奨します。

## 変更案

### UI 実装
#### [MODIFY] [Form1.cs](file:///c:/Users/na6ce/source/repos/T-Hamada0101/MyTools/RegEdit/Form1.cs) & [Form1.Designer.cs](file:///c:/Users/na6ce/source/repos/T-Hamada0101/MyTools/RegEdit/Form1.Designer.cs)
- メインフォームに `TabControl` を追加します。
- 「Windows 11 フォルダ種類無効化」用の「タブ1」を作成します。
- ログ/ステータス出力用のテキストボックスを追加します。
- ボタンを追加します：「ステータス確認」、「修正を適用」、「バックアップから復元」、「RegEditで開く」。

### ロジック実装
#### [NEW] [RegistryHelper.cs](file:///c:/Users/na6ce/source/repos/T-Hamada0101/MyTools/RegEdit/RegistryHelper.cs)
- レジストリ操作を安全に行うためのヘルパークラス。
- メソッド：`ReadValue`, `WriteValue`, `DeleteValue`。

#### [NEW] [BackupManager.cs](file:///c:/Users/na6ce/source/repos/T-Hamada0101/MyTools/RegEdit/BackupManager.cs)
- レジストリの状態保存と復元を扱うクラス。
- 変更前に `FolderType` の元の値を保存します。
- フォーマット：JSON または単純なテキストファイル。

## 検証計画

### 手動検証
1.  **ステータス確認**: アプリを実行し、「ステータス確認」をクリック。現在のレジストリ値を報告することを確認。
2.  **修正を適用**: 「修正を適用」をクリック。
    - プログラムが成功を報告することを確認。
    - `regedit.exe` で `HKEY_CURRENT_USER\Software\Classes\Local Settings\Software\Microsoft\Windows\Shell\Bags\AllFolders\Shell` の `FolderType` が `NotSpecified` に設定されていることを確認。
3.  **復元**: 「バックアップから復元」をクリック。
    - レジストリキーが以前の状態に戻る（または存在しなかった場合は削除される）ことを確認。
4.  **RegEditで開く**: 「RegEditで開く」をクリック。
    - `regedit.exe` が起動し、対象のキーが開かれることを確認。
