# 実装計画: X.com 動画プレイヤーの完全ミュート

## 概要
X.com (旧Twitter) の動画プレイヤーに対して、`muted` プロパティだけでなく `volume` (音量) も 0 に設定するように `content.js` を修正します。
これにより、ユーザーが提示したプレイヤーを含むサイト内の動画が確実にミュートされるようにします。

## 変更内容
- **ファイル**: `TwimgMuter/content.js`
- **変更点**:
    - `muteAllVideos` 関数内で、`video.muted = true` に加え、`video.volume = 0` を設定するロジックを追加します。
    - ログ出力を少し詳細にします。

## 手順
1. `content.js` を編集し、音量0の設定を追加。
2. `manifest.json` の `matches` に `https://x.com/*` と `https://twitter.com/*` を追加し、X(Twitter)上でスクリプトが動作するようにする。
3. `content.js` に `play` イベントと `click` イベントのリスナーを追加し、ユーザー操作や再生開始のタイミングでも音量0とミュートを強制する。
