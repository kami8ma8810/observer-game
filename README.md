# 正義マンGO（Justice Man GO）

SNS時代の"正義中毒"を風刺した2D監視アクションゲーム

## 概要

「正義マンGO」は、SNSでの過剰な正義感や監視社会を皮肉った2Dサイドスクロールゲームです。プレイヤーは街を歩きながら「違反行為」をしているNPCを撮影し、SNSに投稿することでフォロワーを獲得していきます。しかし、投稿内容によっては炎上したり、アカウントが凍結される危険もあります。

## ゲームの特徴

- 🚶 横スクロールで街を探索
- 📷 違反行為を発見してカメラで撮影
- 📱 SNS風のUIで投稿とリアクション
- 🔥 炎上システムとアカウント凍結のリスク
- 🎭 5種類の個性的なNPCと予測不能な反応

## 技術スタック

- **エンジン**: Unity 2D (2022.3 LTS推奨)
- **プラットフォーム**: WebGL
- **言語**: C#
- **テストフレームワーク**: Unity Test Framework (TDD)

## 開発方針

本プロジェクトはテスト駆動開発（TDD）で進めています。新機能の実装前に必ずテストを書き、Red-Green-Refactorのサイクルを守ります。

## プロジェクト構造

```
Assets/
├── Scripts/          # ゲームスクリプト
│   ├── Player/       # プレイヤー関連
│   ├── NPC/          # NPC関連
│   ├── UI/           # UI関連
│   ├── Managers/     # マネージャークラス
│   ├── Data/         # データ構造
│   └── Utils/        # ユーティリティ
├── Tests/            # テストコード
│   ├── EditMode/     # エディタテスト
│   └── PlayMode/     # プレイモードテスト
├── Prefabs/          # プレハブ
├── Materials/        # マテリアル
├── Sprites/          # スプライト素材
├── Audio/            # 音声素材
├── UI/               # UI素材
└── Data/             # 設定データ（JSON等）
```

## セットアップ

1. Unity 2022.3 LTS以降をインストール
2. プロジェクトをクローン
3. Unityでプロジェクトを開く
4. WebGLビルド設定を確認

## テストの実行

- Unity Editor: Window > General > Test Runner
- コマンドライン: `Unity -runTests -projectPath . -testResults results.xml`

## ビルド

1. File > Build Settings
2. Platform: WebGL を選択
3. Player Settings で最適化設定を確認
4. Build

## ライセンス

このプロジェクトは教育・風刺目的で作成されています。

## クレジット

- 開発: [Your Name]
- アセット: Kenney Pack（予定）
- 音楽: DOVA-SYNDROME（予定）
- 効果音: OtoLogic（予定）