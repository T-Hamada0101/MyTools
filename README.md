# MyTools

日常作業を楽にするための自作 Windows ツール集です。主に C# / Windows Forms で作られており、動画処理、動画ダウンロード、レジストリ補助、Excel 読み込み、ブラウザ拡張などの小さなツールをまとめています。

個人用途を前提にしたリポジトリなので、設定値や外部ツールの配置はローカル環境に寄っています。使う前に対象ツールの前提と設定を確認してください。

## 収録ツール

| ツール | 種別 | 概要 |
| --- | --- | --- |
| `EncodeAuto` | .NET 6 WinForms | NVEncC / QSVEncC / ffmpeg などのコマンドをプリセット化し、動画・音声ファイルの一括エンコードを支援します。ドラッグ&ドロップ、並列実行、結合、カット、音量正規化ログ出力などを扱います。 |
| `yt-dlp_loader` | .NET 6 WinForms | `yt-dlp.exe` を GUI から実行するローダーです。URL 一覧、保存先、ファイル名ルール、ブラウザ cookie 取得元、設定ファイル生成をまとめて扱います。 |
| `MovieCut` | .NET 6 WinForms | ffmpeg を使った動画処理の実験ツールです。動画ファイルのドラッグ&ドロップ、ffmpeg パス設定、無音検出の呼び出しなどを行います。 |
| `RegEditTools` | .NET 8 WinForms | Windows レジストリ編集を補助します。Windows 11 のフォルダ種類自動判別や AppXSVC 関連設定を確認・変更し、変更前のバックアップと復元も扱います。 |
| `MuteVideoTwimgCom` | Chrome / Edge 拡張 | `video.twimg.com`、`x.com`、`twitter.com` 上の video 要素を自動でミュートし、音量を 0 にします。拡張本体は `TwimgMuter` フォルダです。 |
| `HDD_Info` | .NET Framework 4.8 Console | WMI からディスク情報と S.M.A.R.T. 予測情報を取得してコンソールへ表示します。管理者権限での実行を前提にしています。 |
| `MyExcelReader` | .NET Framework 4.6.1 WinForms | ExcelDataReader で `.xls` / `.xlsx` / `.xlsm` を読み込み、DataGridView に表示する簡易 Excel ビューアです。 |
| `_Copilot` | .NET Framework 4.6.1 WinForms | Selenium Edge、AngleSharp、SQLite などを使った Web 自動化・AI アシスタント実験用のプロジェクトです。 |

## リポジトリ構成

```text
MyTools.sln                 ソリューション
EncodeAuto/                 エンコード支援ツール
yt-dlp_loader/              yt-dlp GUI ローダー
MovieCut/                   ffmpeg 動画処理ツール
RegEditTools/               レジストリ編集補助ツール
MuteVideoTwimgCom/TwimgMuter/ ブラウザ拡張本体
HDD_Info/                   HDD / S.M.A.R.T. 情報取得ツール
MyExcelReader/              Excel 読み込みツール
_Copilot/                   実験用プロジェクト
```

## 開発環境

- Windows 10 / 11
- Visual Studio 2022
- .NET SDK 6.0 / 8.0
- .NET Framework 4.6.1 / 4.8 Developer Pack
- PowerShell 7.x

## 外部依存

- `EncodeAuto`: NVEncC、QSVEncC、ffmpeg など、プリセットに指定する実行ファイル
- `yt-dlp_loader`: `yt-dlp.exe`、必要に応じて Firefox / Chrome / Edge のプロファイル
- `MovieCut`: `ffmpeg.exe`
- `MyExcelReader`: NuGet の `ExcelDataReader` / `ExcelDataReader.DataSet`
- `_Copilot`: Selenium WebDriver、WebDriverManager、AngleSharp、Dapper、SQLite 関連パッケージ

## ビルド

SDK 形式のプロジェクトは `dotnet build` でビルドできます。

```powershell
dotnet build .\yt-dlp_loader\yt-dlp_loader.csproj
dotnet build .\EncodeAuto\EncodeAuto.csproj
dotnet build .\MovieCut\MovieCut.csproj
dotnet build .\RegEditTools\RegEditTools.csproj
```

非 SDK 形式の .NET Framework プロジェクトは、Visual Studio の Developer PowerShell などから MSBuild でビルドします。

```powershell
msbuild .\MyTools.sln /p:Configuration=Release
```

`packages.config` を使うプロジェクトは、先に Visual Studio または NuGet でパッケージを復元してください。

## 使い方メモ

- `yt-dlp_loader` の詳しい使い方は [yt-dlp_loader/README.md](yt-dlp_loader/README.md) を参照してください。
- `MuteVideoTwimgCom` は `MuteVideoTwimgCom/TwimgMuter` をブラウザの拡張機能として読み込みます。
- `RegEditTools` と `HDD_Info` は Windows の設定やハードウェア情報に触れるため、管理者権限が必要になる場合があります。
- レジストリを変更する前は、ツール内バックアップに加えて手動バックアップも取ると安心です。

## ドキュメント

- [yt-dlp_loader/Docs/構成概要.md](yt-dlp_loader/Docs/構成概要.md)
- [yt-dlp_loader/Docs/利用手順と設定.md](yt-dlp_loader/Docs/利用手順と設定.md)
- [RegEditTools/RegEditTools機能実装計画書.md](RegEditTools/RegEditTools機能実装計画書.md)
- [MuteVideoTwimgCom/TwimgMuter_Guide.md](MuteVideoTwimgCom/TwimgMuter_Guide.md)
- [EncodeAuto/Emoji_Processing_Specs.md](EncodeAuto/Emoji_Processing_Specs.md)

## 注意事項

このリポジトリのツールは個人利用向けです。外部コマンドの実行、ブラウザプロファイルの参照、レジストリ変更、管理者権限での情報取得を行うツールが含まれます。利用によって生じたいかなる損害についても責任は負いません。自己責任で利用してください。
