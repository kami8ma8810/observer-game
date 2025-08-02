# 正義マンGO - プレイ方法

## Unityでの実行手順

### 1. シーンのセットアップ

1. Unityで新しいシーンを作成（File > New Scene）
2. シーンを保存（File > Save As... → `Assets/Scenes/MainScene.unity`）

### 2. ゲームの開始

1. 空のGameObjectを作成（GameObject > Create Empty）
2. 名前を「GameStarter」に変更
3. `MainSceneController`スクリプトをアタッチ
4. インスペクターで以下の設定が可能：
   - Game Duration: 180（秒）
   - Max NPCs: 8
   - Player Move Speed: 5
   - Capture Range: 5
   - Capture Cooldown: 1

### 3. 操作方法

- **移動**: 左右矢印キー または A/Dキー
- **撮影**: スペースキー または マウスクリック（画面下部のカメラボタン）
- **一時停止**: ESCキー
- **リトライ**: ゲームオーバー後にRキー

### 4. ゲームの流れ

1. 街を横移動しながらNPCを探す
2. 違反行為をしているNPCを見つけて撮影
3. SNSに投稿されて「いいね」やフォロワーを獲得
4. ただし誤った通報は炎上リスク！
5. 炎上ゲージが100%になるとアカウント凍結でゲームオーバー

### 5. WebGLビルド手順

1. File > Build Settings
2. Platform: WebGLを選択
3. Add Open Scenesで現在のシーンを追加
4. Player Settings:
   - Resolution and Presentation > Default Canvas Width: 960
   - Resolution and Presentation > Default Canvas Height: 600
5. Build And Run

### トラブルシューティング

- **NPCが表示されない場合**: NPCSpawnerが正しく初期化されているか確認
- **UIが表示されない場合**: UIManagerの初期化を確認
- **操作が効かない場合**: Time.timeScaleが0になっていないか確認

### デバッグモード

Consoleウィンドウ（Window > General > Console）で以下の情報を確認可能：
- ゲーム開始メッセージ
- NPC生成ログ
- 撮影結果ログ