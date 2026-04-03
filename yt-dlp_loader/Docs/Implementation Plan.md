# Implementation Plan

最終更新日: 2026-04-03

## 1. 目的

`yt-dlp_loader` の設定処理と外部連携処理を整理し、今後の機能追加で破綻しにくい構成へ寄せる。  
今回の計画は、全面作り直しではなく、既存 UI を生かしながら責務境界を切り直すことを前提にする。

## 補足: 現在の進捗

- `Phase 1` から `Phase 4` の主要な分離作業は完了
- 設定保存は `Properties.Settings` ではなく `%LOCALAPPDATA%\yt-dlp_loader\app-settings.json` に一本化済み
- `Start` のブラウザ順次起動は非同期化済み
- `DLスレッド数` は `yt-dlp.conf` の `-N` に接続済み
- `AppRuntimePaths`、`YtDlpConfigBuilder`、`AppCompositionRoot`、`AppSettingsValidator` を追加済み
- 現在の主な残作業は「手動確認の積み上げ」「ブラウザ実行パスの外出し」「将来の headless 入口検討」

## 2. 進め方の方針

- 先に設定モデルを一本化する
- 次に `yt-dlp` 実行経路を一本化する
- その後でブラウザ連携を分離する
- 最後に UI からサービス層を呼ぶ形へ寄せる

この順番にする理由は、設定と実行が揃わないまま分割を始めると、途中で動作差分が増えやすいためである。

## 3. スコープ

### 対象

- `Form1.cs` の設定読込、設定保存、実行開始、更新実行
- `YtDlpService.cs` の設定保存、設定ファイル生成、実行
- `Browser.cs` の JSON 読込、cookie 指定生成、ブラウザ起動引数生成
- `yt-dlp.cs` の更新実行と旧実行経路
- `Properties.Settings` の見直し

### 対象外

- UI デザインの全面変更
- CLI 化や IPC 化
- IndigoMovieManager 連携の実装
- 新しいダウンロード機能の追加

## 4. 前提と採用方針

### 前提

- 既存の WinForms UI は当面維持する
- 設定保存先は `%LOCALAPPDATA%\yt-dlp_loader\app-settings.json` を使う
- `yt-dlp.conf` を生成して実行する方式は維持する

### 採用方針

- `DLThreads` は `yt-dlp` の `-N` へ接続する
- `1 movie DL` と `Update` も通常実行と同じ実行窓口へ寄せる
- ブラウザ対応は、まず実装済みの挙動に合わせて整理する

## 5. 実装フェーズ

## Phase 1: 設定モデルの一本化

### 目的

画面状態、保存状態、実行状態のズレを減らす。

### 作業

1. `AppSettings` を追加する
2. `AppSettingsStore` を追加し、`Properties.Settings` との変換をまとめる
3. `Form1` の `LoadProperties` を `AppSettings` 読込経由に置き換える
4. `SaveSetting` を `AppSettings` 保存経由に置き換える
5. カスタム接頭辞・接尾辞の有効フラグを保存対象へ追加する

### 変更候補

- `Form1.cs`
- `YtDlpService.cs`
- `Properties/Settings.settings`
- `Properties/Settings.Designer.cs`
- 必要なら `App.config`
- 新規 `AppSettings.cs`
- 新規 `AppSettingsStore.cs`

### 完了条件

- 起動時に画面状態が正しく復元される
- チェックボックスと文字列値がセットで保存される
- 画面から保存した内容と次回起動時の表示が一致する

## Phase 2: `yt-dlp` 実行窓口の一本化

### 目的

通常実行、単体実行、更新実行で設定反映と例外処理の流れを揃える。

### 作業

1. `YtDlpRunner` を追加する
2. `RunYtDlp` の責務を `YtDlpRunner` へ移す
3. `buttonStart_Click` を `YtDlpRunner` 呼び出しへ寄せる
4. `button5_Click` の直接 `Process.Start` を廃止する
5. `yt-dlp.cs` の `UpdateYtDlp` を `YtDlpRunner` に吸収する
6. `yt-dlp.cs` が不要なら削除する

### 変更候補

- `Form1.cs`
- `YtDlpService.cs`
- `yt-dlp.cs`
- 新規 `YtDlpRunner.cs`

### 完了条件

- `Start`、`1 movie DL`、`Update` が同じ実行窓口を通る
- `yt-dlp.exe` のパス参照元が統一される
- 実行失敗時の扱いが UI で判断できる形になる

## Phase 3: ブラウザ連携の分割

### 目的

ブラウザ定義、cookie 指定、ブラウザ起動を別責務へ分ける。

### 作業

1. `BrowserProfileRepository` を追加する
2. `BrowserCookieOptionBuilder` を追加する
3. `BrowserLauncher` を追加する
4. `Browser.cs` の静的責務を順次移す
5. サポート対象ブラウザを明示し、未対応値を早期に弾く

### 変更候補

- `Browser.cs`
- `Form1.cs`
- `YtDlpService.cs`
- 新規 `BrowserProfileRepository.cs`
- 新規 `BrowserCookieOptionBuilder.cs`
- 新規 `BrowserLauncher.cs`

### 完了条件

- `browser_profiles.json` 読込と URL 起動処理が分離される
- cookie 指定生成が UI 非依存になる
- 対応ブラウザと未対応ブラウザの扱いが明確になる

## Phase 4: `Form1` の責務縮小

### 目的

`Form1` を入力と表示に寄せ、実処理の集約点を別に置く。

### 作業

1. `ApplicationService` を追加する
2. `buttonStart_Click` の処理をサービス呼び出しへ寄せる
3. `UpdateConfigFile` の責務をサービス側へ移す
4. `RunBrowser` の呼び出し責務をサービス側へ移す
5. `Form1` には UI 更新とイベント配線を残す

### 変更候補

- `Form1.cs`
- `YtDlpService.cs`
- 新規 `ApplicationService.cs`

### 完了条件

- `Form1` が設定保存、プロセス起動、設定ファイル生成の詳細を直接持たない
- 主要処理が UI なしでも追える構成になる

## 6. ドキュメント更新

実装と合わせて次を更新する。

- `README.md`
- `Docs/構成概要.md`
- `Docs/利用手順と設定.md`
- `Docs/レビュー_設定と連携のリファクタリング観点.md`

必要なら、フェーズ完了時点で短い変更記録を `Docs` に追加する。

## 7. テスト観点

### 手動確認

1. 設定を変更して保存する
2. アプリを再起動し、画面状態が維持されることを確認する
3. `Start` で `yt-dlp.conf` が期待どおり生成されることを確認する
4. `1 movie DL` が通常実行と同じ設定系を通ることを確認する
5. `Update` が正しい `yt-dlp.exe` を使うことを確認する
6. ブラウザプロファイル変更時に cookie 指定が正しく更新されることを確認する

### 確認ポイント

- チェックボックスの保存漏れがない
- 画面入力と保存済み設定の参照元が混在しない
- ブラウザ未対応時に黙って失敗しない
- 実行失敗時に原因が追える

## 8. リスク

- `Properties.Settings` 変更時に既存ユーザー設定との互換性確認が必要
- `App.config` と自動生成コードの関係が崩れると、設定反映が読みづらくなる
- `1 movie DL` の動作差分を吸収する際、現行の簡易挙動が変わる可能性がある
- ブラウザ実行パスの固定仕様をどう扱うかで追加修正が増える

## 9. 最初の実装単位

最初の 1 つ目の変更セットは、次の最小単位にする。

1. `AppSettings` と `AppSettingsStore` を追加する
2. カスタム接頭辞・接尾辞の有効フラグを保存対象に入れる
3. `LoadProperties` と `SaveSetting` を新しい設定モデルへ寄せる

この単位なら副作用を比較的限定しつつ、レビューで見つかった最重要のズレを先に潰せる。
