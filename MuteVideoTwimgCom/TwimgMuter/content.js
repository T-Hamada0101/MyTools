
function muteVideo(video) {
    // すでにミュートかつ音量0でなければ設定する
    if (!video.muted || video.volume !== 0) {
        video.muted = true;
        video.volume = 0;
        // console.log('Video muted and volume set to 0.');
    }
}

function muteAllVideos() {
    const videos = document.querySelectorAll('video');
    videos.forEach(muteVideo);
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

// 3. 動画再生開始 (play events) をキャプチャして強制ミュート
document.addEventListener('play', (e) => {
    if (e.target.tagName === 'VIDEO') {
        muteVideo(e.target);
    }
}, true);

// 4. 何かをクリックした際も念のためチェック (再生ボタンクリック等対策)
document.addEventListener('click', (e) => {
    muteAllVideos();
}, true);

