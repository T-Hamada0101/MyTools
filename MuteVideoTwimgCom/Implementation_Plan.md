# Chrome拡張機能 MuteVideoTwimgCom 実装計画

## 概要
`https://video.twimg.com/` にアクセスした際に、自動的にビデオプレイヤーをミュートにするChrome拡張機能を作成します。

## 提案する変更

### ディレクトリ構成
`C:\Users\na6ce\source\repos\T-Hamada0101\MyTools\MuteVideoTwimgCom\`

### [NEW] manifest.json
拡張機能の設定ファイル。
- `manifest_version`: 3
- `name`: Mute Video Twimg
- `version`: 1.0
- `content_scripts`: 
    - `matches`: ["https://video.twimg.com/*"]
    - `js`: ["content.js"]

### [NEW] content.js
実際にページ上で動作するスクリプト。
- `video`タグを検索する。
- 見つかった場合、`muted = true` に設定する。
- ページ読み込み完了時および動的な要素追加に対応するため、`MutationObserver` または定期チェックを使用する。

## 検証計画
### 手動検証
1. Chromeの「拡張機能の管理」を開く。
2. 「パッケージ化されていない拡張機能を読み込む」を選択。
3. 作成したディレクトリを指定して読み込む。
4. `https://video.twimg.com/` 以下の動画URLにアクセスし、音が鳴らないことを確認する。
