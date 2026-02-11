using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCut.ffmpeg
{
    internal class ffmpegMemo
    {
        /*
         FFmpegで素早く正確に動画をカットする自分的ベストプラクティス
        https://qiita.com/kaityo256/items/2e3e9d3b8d1d6b8d5b1a
        はじめに...結論を
        はじめに結論を言ってしまうと、バージョン2.1以降であれば
            開始地点（-ss）はソースの指定（-i）より手前に置く
            必ずエンコードする（-codec copyとかやらない）
        とやるのが一番正確に（かつそこそこ速く）カットできます！例えばこんな感じです。

        ffmpeg -ss [開始地点(秒)] -i [入力する動画パス] -t [切り出す秒数] [出力する動画パス]

        //
https://qiita.com/tubo28/items/afeebd0969d9128b7a89
        //ffmpeg -ss 00:00:00 -i input.mp4 -t 00:00:10 -vcodec copy -acodec copy output.mp4





        */
    }
}
