# 調査結果: IndigoMovieManager 外部連携方針 skin と yt-dlp_loader 2026-04-01

最終更新日: 2026-04-01

## 1. 目的

- `%USERPROFILE%\source\repos\IndigoMovieManager` に、`%USERPROFILE%\source\repos\T-Hamada0101\MyTools\yt-dlp_loader\yt-dlp_loader.csproj` の機能を組み込みたい
- 可能なら IndigoMovieManager の `skin` 機能を使いたい
- ただし `yt-dlp_loader` は「完全に外部プロジェクト」のまま保ちたい

## 2. 結論

- `skin` は使える
- ただし「今の WinForms 画面をそのまま IndigoMovieManager の skin に埋め込む」のは筋が悪い
- 一番よい形は、以下の二層構成である

1. IndigoMovieManager 側
   外部 skin / WebView2 を front UI として使う
2. yt-dlp_loader 側
   外部プロジェクトのまま sidecar/back-end として動かす

つまり、`skin` は「見せる側」、`yt-dlp_loader` は「実行する側」に分けるのが最も安全で保守しやすい。

## 3. 調査で確認できた事実

### 3.1 IndigoMovieManager 側の事実

- `skin` 配下はビルド成果物へコピーされる前提になっている
- 外部 skin は built-in ではなく `WebView2` 表示前提で扱われる
- 外部 skin を選ぶと、既存の WPF タブ群を隠して host を前面表示する
- 設定画面から skin を切り替える導線が既にある
- `wb.*` 互換 API はすでに最小実装が入り始めている
- ただし現状の API は movie 一覧表示寄りで、downloader 操作 API ではない

### 3.2 yt-dlp_loader 側の事実

- `yt-dlp_loader` は `net6.0-windows` の WinForms `WinExe`
- 起動入口は `Application.Run(new Form1())` だけで、CLI 実行や IPC 入口はまだ無い
- 実処理は `YtDlpService`、設定は `YtDlpOptions`、ブラウザプロファイル処理は `Browser` にまとまっている
- ただしこれらは今のところ `internal` 中心で、外部公開用の契約にはなっていない
- `browser_profiles.json` を実行ファイル基準で読み込む構成になっている
- 設定ファイルは `%LOCALAPPDATA%\yt-dlp_loader\yt-dlp.conf` に保存する設計になっている

## 4. なぜ「そのまま埋め込み」が厳しいか

### 4.1 UI モデルが違う

- IndigoMovieManager は `net8.0-windows` の WPF
- `yt-dlp_loader` は `net6.0-windows` の WinForms
- しかも `yt-dlp_loader` は `Form1` を直接起動する前提なので、部品として差し込む形にまだなっていない

### 4.2 skin は「補助パネル」ではなく「主表示切替」に近い

- IndigoMovieManager の外部 skin は、既存タブの中に小さく乗る構造ではない
- 既存タブを隠し、WebView2 host を主表示へ切り替える前提で作られている
- そのため downloader を常時補助 UI として見せたい場合、skin は少し役割が違う

### 4.3 既存 bridge は downloader 操作用ではない

- 現在の `wb.*` 互換 API は `update`, `getInfo`, `getInfos`, `focusThum`, `getSkinName`, `getDBName`, `getThumDir`, `trace` が中心
- URL キュー追加、設定保存、実行開始、進捗購読、更新実行のような downloader 専用 API はまだ無い

### 4.4 外部プロジェクトのまま保つなら、呼び出し境界を明示した方がよい

- 直接 `ProjectReference` で UI ごと混ぜると、依存方向が濁る
- `internal` 実装や WinForms 事情が IndigoMovieManager へ漏れやすい
- 「完全に外部プロジェクト」という方針とも相性が悪い

## 5. 推奨アーキテクチャ

## 案A: sidecar 方式

### 概要

- `yt-dlp_loader` は別プロセスのまま維持する
- ただし GUI 起動だけでなく、headless 実行モードを追加する
- IndigoMovieManager はその headless 入口をプロセス起動で呼ぶ
- skin 側は WebView2 上の HTML/JS で UI を出し、C# bridge 経由で外部プロセスへ依頼する

### 長所

- 外部プロジェクト性をきれいに維持できる
- 既存 WinForms 画面は残しつつ、別入口だけ増やせる
- IndigoMovieManager 本線へ WinForms 依存を持ち込まない
- 失敗時の切り離しがしやすい

### 短所

- CLI か JSON IPC の設計が必要
- 進捗通知の扱いを最初に決める必要がある

## 案B: Core ライブラリ切り出し方式

### 概要

- `yt-dlp_loader` から `YtDlpService`, `YtDlpOptions`, `Browser` などを `yt-dlp_loader.Core` に分離する
- WinForms 側はその Core を使う
- IndigoMovieManager も Core を参照する

### 長所

- 型安全に連携しやすい
- テストしやすい
- 設定や実行ロジックの重複を避けやすい

### 短所

- 外部プロジェクトの構成変更が比較的大きい
- 「完全に外部プロジェクト」の意味が、プロセス分離ではなくソース分離に寄る

## 案C: 現状の csproj をそのまま参照

### 評価

- 非推奨
- WinForms `WinExe` をそのまま組み込むのは、短期では動いても長期で重くなる
- 画面起動責務と実処理責務が分かれていないため、将来の拡張コストが高い

## 6. `skin` を使う場合の正しい位置づけ

### 向いている使い方

- downloader 専用の「モード画面」として外部 skin を使う
- HTML/JS ベースで URL 入力、実行ボタン、進捗表示、ログ表示を構成する
- IndigoMovieManager から見れば「外部 skin を 1 つ選ぶと downloader 画面に切り替わる」という体験にする

### 向いていない使い方

- 既存 movie 一覧の横に、WinForms downloader をそのまま貼り付ける
- current DB に紐づく skin 設定の枠組みへ、補助ツール UI を無理に押し込む

### 設計上のおすすめ

- WhiteBrowser 互換の `wb.*` API に downloader 専用責務を大量に混ぜない
- downloader 用は別の JS facade を用意する方が整理しやすい
- 例えば `window.immYtDlp.*` のような専用 API を skin HTML へ提供し、内部では既存 WebView2 bridge を使う形が良い

## 7. 実装するなら最初に切るべき境界

## Phase 1: yt-dlp_loader 側に headless 入口を作る

最低限ほしい機能は以下である。

- `profiles`
  ブラウザプロファイル一覧を JSON で返す
- `settings-load`
  現在設定を JSON で返す
- `settings-save`
  設定を JSON で受けて保存する
- `run`
  URL 一覧と設定を受けて yt-dlp を実行する
- `update-binary`
  `yt-dlp -U` を実行する

この段階では、既存 GUI は壊さず残す。

## Phase 2: IndigoMovieManager 側に client を作る

- 外部プロセス呼び出し専用の service を 1 本作る
- `ProcessStartInfo` と JSON 入出力だけを責務にする
- UI スレッドを塞がない
- ログとタイムアウトを最初から持つ

## Phase 3: skin 側に downloader 画面を作る

- URL 入力欄
- キュー一覧
- ブラウザプロファイル選択
- 保存先・命名ルール編集
- 実行ボタン
- 実行ログ表示

この UI は HTML/JS なので、外観はかなり自由に作れる。

## 8. 大粒度の判断

- 「外部プロジェクトのまま組み込みたい」という条件を最優先するなら、sidecar 方式が最適
- 「skin を使いたい」という条件も、その sidecar 方式なら満たせる
- 逆に、今の `yt-dlp_loader` WinForms をそのまま skin に入れる方向は避けるべき

## 9. 推奨結論

今回の方針は以下で固定するのが良い。

1. `yt-dlp_loader` は外部プロジェクトのまま維持する
2. `yt-dlp_loader` に headless 実行入口を追加する
3. IndigoMovieManager は外部プロセス client として連携する
4. UI は IndigoMovieManager の external skin / WebView2 で作る
5. downloader 専用 API は `wb.*` へ過剰に混ぜず、別 facade で整理する

## 10. 参考メモ

- IndigoMovieManager 側の skin 基盤は、外部 skin 読み込みと WebView2 host、最小 API bridge まではすでに育っている
- skin 関連の最小テストは調査時点で通過しており、土台の信頼性はある程度見込める
- ただし downloader 統合はまだ未着手なので、次に進むなら `yt-dlp_loader` 側の headless 化が最初の一手になる
