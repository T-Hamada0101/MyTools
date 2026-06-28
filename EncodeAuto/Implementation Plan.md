# EncodeAuto プリセット24枠化 実装計画

## 目的

EncodeAutoのプリセット数を12枠から24枠へ増やし、画面上でも24枠を選択できるようにする。

## 方針

- `PresetClass.PresetCount` を24にして、保存データの標準枠数を増やす。
- プリセット選択UIは固定配置の12個ではなく、`PresetCount` から動的に生成する。
- 既存の `Preset.xml` が12枠でも、読み込み時に各配列を24枠へ拡張して既存値を保持する。
- `shortFileName` と `audioNormalize` もプリセットごとの保存・復元対象に含める。

## 確認

- `dotnet build EncodeAuto\EncodeAuto.csproj` でビルド確認する。
