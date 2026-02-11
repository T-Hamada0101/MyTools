# 動画ファイルへの直リンクページで自動的に音量をミュートにするChrome拡張機能の実装方法

ご提示いただいたURL（video.twimg.com）のような「動画ファイルへの直リンク」ページで、自動的に音量をミュートにするChrome拡張機能の実装方法を解説します。

このページは通常のHTMLページとは異なり、ブラウザが標準で持っているメディアプレーヤー機能によって `<video>` タグが自動生成されて再生されています。そのため、Content Script（コンテンツスクリプト） を使用して、その `<video>` タグを操作するのが最もシンプルで確実です。

以下に必要最小限のファイル構成とコードを示します。

## 1. ファイル構成
任意のフォルダ（例: `TwimgMuter`）を作成し、その中に以下の2つのファイルを保存してください。

- `manifest.json` （設定ファイル）
- `content.js` （実行するスクリプト）

## 2. コード内容

### manifest.json
Manifest V3に対応した記述です。`matches` で対象のドメインを指定しています。

```json
{
  "manifest_version": 3,
  "name": "Twimg Muter",
  "version": "1.0",
  "description": "video.twimg.comの動画を自動的にミュートします。",
  "content_scripts": [
    {
      "matches": ["https://video.twimg.com/*"],
      "js": ["content.js"],
      "run_at": "document_idle"
    }
  ]
}
```

### content.js
動画タグを見つけて `muted` 属性を付与します。直リンクの場合、DOMの生成タイミングがブラウザの実装に依存するため、念のため `MutationObserver` を使って確実に要素を捕捉するようにします。

```javascript
function muteAllVideos() {
    const videos = document.querySelectorAll('video');
    videos.forEach(video => {
        // すでにミュートでなければミュートにする
        if (!video.muted) {
            video.muted = true;
            console.log('Video muted automatically.');
        }
    });
}

// 1. 読み込み直後に実行
muteAllVideos();

// 2. DOMの変化を監視して、後から追加された要素（またはロード完了時）にも適用
const observer = new MutationObserver((mutations) => {
    muteAllVideos();
});

// 監視を開始
observer.observe(document.body, {
    childList: true,
    subtree: true
});
```

## 3. インストール手順
1. Chromeブラウザを開き、アドレスバーに `chrome://extensions/` と入力します。
2. 右上の 「デベロッパー モード」 をオンにします。
3. 左上に現れる 「パッケージ化されていない拡張機能を読み込む」 ボタンをクリックします。
4. 先ほど作成したフォルダ（`TwimgMuter`）を選択します。

これで、 `https://video.twimg.com/...` のURLを開くと、動画が自動的にミュート状態で開始されるようになります。

## (参考) 既存のツールを使う場合
