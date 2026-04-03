# yt-dlp_loader

`yt-dlp_loader` は、`yt-dlp` を GUI から扱いやすくするための Windows Forms ツールです。  
URL のリストをまとめて渡し、保存先やファイル名ルール、ブラウザ cookie の取得元を切り替えながらダウンロードを実行できます。

## できること

- `yt-dlp.exe` の実行パスを保存する
- URL リストをファイルへ書き出して一括ダウンロードする
- 保存先フォルダとファイル名ルールを調整する
- ブラウザプロファイルを選んで cookie 付き実行を行う
- `yt-dlp.conf` を自動生成し、画面上で内容を確認する
- `yt-dlp -U` で本体更新を呼び出す

## 画面の役割

- `yt-dlp.exe`: 実行する `yt-dlp` 本体の場所
- `url File`: URL 一覧を書き出す先
- `DL Folder`: 保存先フォルダ
- `Start`: 設定保存、URL ファイル出力、`yt-dlp` 実行をまとめて行う
- `Save`: 現在の設定を保存し、設定ファイル表示を更新する
- `Update`: `yt-dlp -U` を管理者権限で実行する

## 処理の流れ

1. 画面から設定を読み込みます。
2. 画面入力を `AppSettings` にまとめます。
3. `browser_profiles.json` を使って cookie 取得先を決めます。
4. `AppSettings` を `YtDlpOptions` に変換します。
5. `%LOCALAPPDATA%\\yt-dlp_loader\\app-settings.json` と `%LOCALAPPDATA%\\yt-dlp_loader\\yt-dlp.conf` を更新します。
6. URL 一覧を指定ファイルへ保存します。
7. `yt-dlp` を設定ファイル付きで起動します。

## 設定保存

- 現在の設定保存先は `%LOCALAPPDATA%\\yt-dlp_loader\\app-settings.json` です
- `prefix` / `suffix` のチェック状態を含めて、画面設定は JSON にまとめて保存します
- 新規保存では `Properties.Settings` を使いません
- 旧 `user.config` が見つかった場合は、初回読込時に JSON へ移行します

## 前提

- Windows
- `.NET 6` 実行環境
- `yt-dlp.exe`
- 必要に応じて Firefox、Chrome、Edge のプロファイル設定

## ビルド

```powershell
dotnet build .\yt-dlp_loader.csproj
```

## ドキュメント

- [Implementation Plan](C:/Users/na6ce/source/repos/T-Hamada0101/MyTools/yt-dlp_loader/Docs/Implementation%20Plan.md)
- [構成概要](C:/Users/na6ce/source/repos/T-Hamada0101/MyTools/yt-dlp_loader/Docs/構成概要.md)
- [利用手順と設定](C:/Users/na6ce/source/repos/T-Hamada0101/MyTools/yt-dlp_loader/Docs/利用手順と設定.md)
- [レビュー: 設定と連携のリファクタリング観点](C:/Users/na6ce/source/repos/T-Hamada0101/MyTools/yt-dlp_loader/Docs/レビュー_設定と連携のリファクタリング観点.md)
- [調査結果: IndigoMovieManager 外部連携方針 skin と yt-dlp_loader 2026-04-01](C:/Users/na6ce/source/repos/T-Hamada0101/MyTools/yt-dlp_loader/Docs/調査結果_IndigoMovieManager外部連携方針_skinとyt-dlp_loader_2026-04-01.md)

## 主要ファイル

- `Form1.cs`: 画面入力と表示更新
- `ApplicationService.cs`: 画面から呼ぶ保存、実行、起動処理の入口
- `AppCompositionRoot.cs`: 共有サービスの組み立て入口
- `AppRuntimePaths.cs`: `%LOCALAPPDATA%\\yt-dlp_loader` 配下のパス管理
- `AppSettingsValidator.cs`: 実行前に必須設定を確認
- `ProcessLauncher.cs`: 外部プロセス起動の共通入口
- `FormActionRunner.cs`: 画面イベントの例外処理とボタン制御
- `BrowserUrlBatchOpener.cs`: URL の順次ブラウザ起動と待機
- `FormSettingsMapper.cs`: 画面コントロールと `AppSettings` の変換
- `ConsoleTextBoxWriter.cs`: 実行ログの表示更新
- `UrlListBoxAdapter.cs`: URL リストの追加、取得、削除
- `AppSettings.cs`: 画面設定の正規モデル
- `AppSettingsStore.cs`: `app-settings.json` の読込と保存
- `YtDlpRunner.cs`: 保存、config 生成、実行の流れをまとめる
- `YtDlpConfigBuilder.cs`: `yt-dlp.conf` の各行を組み立てる
- `BrowserProfileRepository.cs`: `browser_profiles.json` の読込と既定プロファイル解決
- `BrowserExecutableResolver.cs`: ブラウザ実行ファイルの解決
- `BrowserCookieOptionBuilder.cs`: cookie 指定文字列の生成
- `BrowserLauncher.cs`: ブラウザ起動処理
- `YtDlpService.cs`: URL ファイル生成、設定ファイル生成、`yt-dlp` 実行
- `YtDlpOptions.cs`: 画面設定の受け渡し
- `browser_profiles.json`: cookie 取得に使うブラウザ定義

## 補足

このリポジトリでは、詳細な検討メモは `Docs` に集約し、`README.md` は入口として短く保つ方針にしています。
