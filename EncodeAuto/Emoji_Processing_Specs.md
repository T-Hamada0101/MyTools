# EncodeAuto 絵文字処理仕様

## 概要
`EncodeAuto` ではファイル名に絵文字や特定の記号が含まれている場合、`ffmpeg` やコマンドプロンプトでのエラーを防ぐため、「一時的な名前の変更」と「処理後の名前復元」を行っています。

## 処理フロー

1. **絵文字の除去 (`RenameEmojiFile`)**
   - `Regexs.InputValueValidate` を呼び出し、正規表現（サロゲートペアや特定記号）を用いてファイル名から絵文字を除去します。
   - 除去後の安全なファイル名で、実ファイルを一時的にリネーム(`File.Move`)します。

2. **履歴の保存**
   - 元のファイルパスと一時変更したファイルパスの対応を `Org_After_Encoded` ディクショナリリストに記録します。

3. **エンコード処理**
   - 絵文字を含まないファイル名を用いてバッチファイルを作成し、`ffmpeg` によるエンコードを実行します。

4. **処理後の復元 (`PostProcessing` / `ComebackEmojiFile`)**
   - エンコード完了後、`Org_After_Encoded` の記録を参照して、一時的なファイル名から元の絵文字を含むファイル名へリネーム(`File.Move`)して復元します。
   - その後、完了フォルダ等へ移動させます。

## 主な関連ファイル
- `EncodeDeta.cs`: 処理フローの制御(`RenameEmojiFile`, `PostProcessing`, `ComebackEmojiFile` など)
- `Regexs.cs`: 絵文字を判定・除去する正規表現リスト(`InputBlockRegexList` など)
