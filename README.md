# Snap Attack

SNS時代の"正義中毒"を風刺した2D監視アクションゲーム

## 概要

「Snap Attack」は、SNSでの過剰な正義感や監視社会を皮肉った2Dサイドスクロールゲームです。プレイヤーは街を歩きながら「違反行為」をしているNPCを撮影し、SNSに投稿することでフォロワーを獲得します。しかし、投稿内容によっては炎上したり、アカウントが凍結される危険もあります。

## ゲームの特徴

- 🚶 横スクロールで街を探索
- 📷 違反行為を発見してカメラで撮影
- 📱 SNS風のUIで投稿とリアクション
- 🔥 炎上システムとアカウント凍結のリスク
- 🎭 5種類の個性的なNPCと予測不能な反応

## 実装済みNPC

| NPC | 違反内容 | 通報成功率 | 炎上リスク |
|-----|---------|------------|------------|
| 路上喫煙者 | 喫煙エリア外での喫煙 | 高 | 中 |
| ベビーカーの母親 | 歩道を塞いで歩行 | 低 | 極高 |
| 鳩餌やりおばあちゃん | 公園での餌やり | 中 | 高 |
| 配達員 | 路上への一時停車 | 極低 | 極高 |
| 高校生 | 自転車の逆走 | 中 | 低 |

## 技術スタック

- **エンジン**: Unity 2D（2022.3 LTS推奨）
- **プラットフォーム**: WebGL
- **言語**: C#
- **テストフレームワーク**: Unity Test Framework（TDD）

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

### 基本セットアップ
1. Unity 2022.3 LTS以降をインストール
2. プロジェクトをクローン
   ```bash
   git clone https://github.com/yourusername/observer-game.git
   cd observer-game
   ```
3. Unityでプロジェクトを開く
4. WebGLビルド設定を確認

### ゲームの開始方法
1. 新しいシーンを作成（File > New Scene）
2. シーンを保存（`Assets/Scenes/MainScene.unity`）
3. 空のGameObjectを作成し「GameStarter」と命名
4. `MainSceneController`スクリプトをアタッチ
5. Playボタンでゲーム開始

## テストの実行

### Unity Editor
1. Window > General > Test Runner
2. EditModeタブを選択
3. Run Allでテスト実行

### テストカバレッジ
- ✅ CameraFollowController（5テスト）
- ✅ PlayerMovementController（9テスト）
- ✅ PhotoCaptureController（8テスト）
- ✅ NPCBehavior（6テスト）
- ✅ ViolationDetectionManager（4テスト）
- ✅ NPCSpawner（7テスト）
- ✅ UIManager（9テスト）
- ✅ SNSPostEffect（8テスト）
- ✅ GameFlowController（10テスト）
- ✅ GameManager（10テスト）

**合計: 76のユニットテスト**

## 操作方法

| 操作 | キー/ボタン |
|------|------------|
| 移動 | ←→ または A/D |
| 撮影 | スペース または カメラボタン |
| 一時停止 | ESC |
| リトライ | R（ゲームオーバー後） |

## ビルド

### WebGLビルド手順
1. File > Build Settings
2. Platform: WebGLを選択
3. Add Open Scenesで現在のシーンを追加
4. Player Settings:
   - Resolution: 960x600
   - WebGL Template: Default
   - Compression Format: Gzip
5. Build And Run

### 推奨ブラウザ
- Chrome（推奨）
- Firefox
- Safari
- Edge

## ゲームシステム

### スコアリング
- 写真撮影: +100ポイント
- 正当な通報: +いいね数（10-30）
- 誤報: -10～-30ポイント

### 炎上システム
- 誤報: +15%
- 炎上コメント: +10%
- 正当な通報: -5%
- 100%でアカウント凍結（ゲームオーバー）

### NPCの行動パターン
- **静止型**: 特定の場所で違反行為
- **パトロール型**: 一定範囲を往復
- **ランダム型**: 不規則な移動

## プロジェクトの特徴

- **TDD実践**: 全機能をテストファーストで開発
- **データドリブン設計**: NPCデータをJSON管理
- **モジュラー構造**: 各システムが独立して動作
- **拡張性**: 新しいNPCや違反行為を簡単に追加可能

## トラブルシューティング

| 問題 | 解決方法 |
|------|----------|
| NPCが表示されない | NPCSpawnerの初期化を確認 |
| UIが表示されない | UICanvasの作成を確認 |
| 操作が効かない | Time.timeScaleが0でないか確認 |
| テストが失敗する | Unity Test Frameworkパッケージを確認 |

## 今後の拡張案

- 🌆 複数ステージ（住宅街、オフィス街、繁華街）
- 🏆 実績システム
- 📊 オンラインランキング
- 🎭 追加NPC（歩きスマホ、ポイ捨て、騒音など）
- 🎮 難易度選択

## ライセンス

このプロジェクトは教育・風刺目的で作成されています。

## クレジット

- 開発: [Your Name]
- アセット: Kenney Pack（予定）
- 音楽: DOVA-SYNDROME（予定）
- 効果音: OtoLogic（予定）

## 関連ドキュメント

- [プレイ方法詳細](Assets/README_PLAY.md)
- [開発サマリー](DEVELOPMENT_SUMMARY.md)
- [NPCデータ仕様](Assets/Data/NPCData.json)