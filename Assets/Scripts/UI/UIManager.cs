using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace JusticeManGO.UI
{
    public class UIManager : MonoBehaviour
    {
        [System.Serializable]
        public class CaptureResultData
        {
            public bool Success;
            public string Message;
            public int LikesGained;
            public int FollowersGained;
            public bool Backlash;
        }

        private GameObject canvas;
        private Text scoreText;
        private Text followerText;
        private Slider flameGauge;
        private Button cameraButton;
        
        private GameObject captureResultPopup;
        private Text popupMessageText;
        private Text popupStatsText;
        
        private GameObject gameOverScreen;
        private Text gameOverMessage;
        private Text finalScoreText;
        
        private GameObject notificationPanel;
        private Text notificationText;

        private bool isInitialized = false;

        public void Initialize()
        {
            CreateCanvas();
            CreateTopBar();
            CreateCameraButton();
            CreateCaptureResultPopup();
            CreateGameOverScreen();
            CreateNotificationPanel();
            
            isInitialized = true;
        }

        private void CreateCanvas()
        {
            canvas = new GameObject("UICanvas");
            Canvas canvasComponent = canvas.AddComponent<Canvas>();
            canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.AddComponent<CanvasScaler>();
            canvas.AddComponent<GraphicRaycaster>();
        }

        private void CreateTopBar()
        {
            GameObject topBar = new GameObject("TopBar");
            topBar.transform.SetParent(canvas.transform, false);
            
            RectTransform topBarRect = topBar.AddComponent<RectTransform>();
            topBarRect.anchorMin = new Vector2(0, 0.9f);
            topBarRect.anchorMax = new Vector2(1, 1);
            topBarRect.offsetMin = Vector2.zero;
            topBarRect.offsetMax = Vector2.zero;
            
            Image topBarBg = topBar.AddComponent<Image>();
            topBarBg.color = new Color(0.1f, 0.1f, 0.1f, 0.9f);
            
            CreateScoreDisplay(topBar.transform);
            CreateFollowerDisplay(topBar.transform);
            CreateFlameGauge(topBar.transform);
        }

        private void CreateScoreDisplay(Transform parent)
        {
            GameObject scoreObj = new GameObject("Score");
            scoreObj.transform.SetParent(parent, false);
            
            RectTransform scoreRect = scoreObj.AddComponent<RectTransform>();
            scoreRect.anchorMin = new Vector2(0, 0);
            scoreRect.anchorMax = new Vector2(0.3f, 1);
            scoreRect.offsetMin = new Vector2(10, 0);
            scoreRect.offsetMax = new Vector2(-10, 0);
            
            scoreText = scoreObj.AddComponent<Text>();
            scoreText.text = "Score: 0";
            scoreText.font = Font.CreateDynamicFontFromOSFont("Arial", 14);
            scoreText.color = Color.white;
            scoreText.alignment = TextAnchor.MiddleLeft;
        }

        private void CreateFollowerDisplay(Transform parent)
        {
            GameObject followerObj = new GameObject("Followers");
            followerObj.transform.SetParent(parent, false);
            
            RectTransform followerRect = followerObj.AddComponent<RectTransform>();
            followerRect.anchorMin = new Vector2(0.3f, 0);
            followerRect.anchorMax = new Vector2(0.6f, 1);
            followerRect.offsetMin = new Vector2(10, 0);
            followerRect.offsetMax = new Vector2(-10, 0);
            
            followerText = followerObj.AddComponent<Text>();
            followerText.text = "Followers: 0";
            followerText.font = Font.CreateDynamicFontFromOSFont("Arial", 14);
            followerText.color = Color.white;
            followerText.alignment = TextAnchor.MiddleCenter;
        }

        private void CreateFlameGauge(Transform parent)
        {
            GameObject gaugeObj = new GameObject("FlameGauge");
            gaugeObj.transform.SetParent(parent, false);
            
            RectTransform gaugeRect = gaugeObj.AddComponent<RectTransform>();
            gaugeRect.anchorMin = new Vector2(0.6f, 0.2f);
            gaugeRect.anchorMax = new Vector2(0.95f, 0.8f);
            gaugeRect.offsetMin = Vector2.zero;
            gaugeRect.offsetMax = Vector2.zero;
            
            flameGauge = gaugeObj.AddComponent<Slider>();
            flameGauge.minValue = 0;
            flameGauge.maxValue = 1;
            flameGauge.value = 0;
            
            GameObject fillArea = new GameObject("Fill Area");
            fillArea.transform.SetParent(gaugeObj.transform, false);
            RectTransform fillAreaRect = fillArea.AddComponent<RectTransform>();
            fillAreaRect.anchorMin = Vector2.zero;
            fillAreaRect.anchorMax = Vector2.one;
            fillAreaRect.offsetMin = Vector2.zero;
            fillAreaRect.offsetMax = Vector2.zero;
            
            GameObject fill = new GameObject("Fill");
            fill.transform.SetParent(fillArea.transform, false);
            Image fillImage = fill.AddComponent<Image>();
            fillImage.color = Color.red;
            RectTransform fillRect = fill.AddComponent<RectTransform>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = new Vector2(1, 1);
            fillRect.offsetMin = Vector2.zero;
            fillRect.offsetMax = Vector2.zero;
            
            flameGauge.fillRect = fillRect;
        }

        private void CreateCameraButton()
        {
            GameObject buttonObj = new GameObject("CameraButton");
            buttonObj.transform.SetParent(canvas.transform, false);
            
            RectTransform buttonRect = buttonObj.AddComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0.4f, 0.05f);
            buttonRect.anchorMax = new Vector2(0.6f, 0.15f);
            buttonRect.offsetMin = Vector2.zero;
            buttonRect.offsetMax = Vector2.zero;
            
            cameraButton = buttonObj.AddComponent<Button>();
            Image buttonImage = buttonObj.AddComponent<Image>();
            buttonImage.color = new Color(0.2f, 0.7f, 0.9f);
            
            GameObject buttonText = new GameObject("Text");
            buttonText.transform.SetParent(buttonObj.transform, false);
            RectTransform textRect = buttonText.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;
            
            Text text = buttonText.AddComponent<Text>();
            text.text = "📷";
            text.font = Font.CreateDynamicFontFromOSFont("Arial", 24);
            text.alignment = TextAnchor.MiddleCenter;
        }

        private void CreateCaptureResultPopup()
        {
            captureResultPopup = new GameObject("CaptureResultPopup");
            captureResultPopup.transform.SetParent(canvas.transform, false);
            
            RectTransform popupRect = captureResultPopup.AddComponent<RectTransform>();
            popupRect.anchorMin = new Vector2(0.2f, 0.3f);
            popupRect.anchorMax = new Vector2(0.8f, 0.7f);
            popupRect.offsetMin = Vector2.zero;
            popupRect.offsetMax = Vector2.zero;
            
            Image popupBg = captureResultPopup.AddComponent<Image>();
            popupBg.color = new Color(1, 1, 1, 0.95f);
            
            GameObject messageObj = new GameObject("Message");
            messageObj.transform.SetParent(captureResultPopup.transform, false);
            RectTransform messageRect = messageObj.AddComponent<RectTransform>();
            messageRect.anchorMin = new Vector2(0.1f, 0.5f);
            messageRect.anchorMax = new Vector2(0.9f, 0.9f);
            messageRect.offsetMin = Vector2.zero;
            messageRect.offsetMax = Vector2.zero;
            
            popupMessageText = messageObj.AddComponent<Text>();
            popupMessageText.font = Font.CreateDynamicFontFromOSFont("Arial", 16);
            popupMessageText.color = Color.black;
            popupMessageText.alignment = TextAnchor.MiddleCenter;
            
            GameObject statsObj = new GameObject("Stats");
            statsObj.transform.SetParent(captureResultPopup.transform, false);
            RectTransform statsRect = statsObj.AddComponent<RectTransform>();
            statsRect.anchorMin = new Vector2(0.1f, 0.1f);
            statsRect.anchorMax = new Vector2(0.9f, 0.5f);
            statsRect.offsetMin = Vector2.zero;
            statsRect.offsetMax = Vector2.zero;
            
            popupStatsText = statsObj.AddComponent<Text>();
            popupStatsText.font = Font.CreateDynamicFontFromOSFont("Arial", 14);
            popupStatsText.color = Color.gray;
            popupStatsText.alignment = TextAnchor.MiddleCenter;
            
            captureResultPopup.SetActive(false);
        }

        private void CreateGameOverScreen()
        {
            gameOverScreen = new GameObject("GameOverScreen");
            gameOverScreen.transform.SetParent(canvas.transform, false);
            
            RectTransform screenRect = gameOverScreen.AddComponent<RectTransform>();
            screenRect.anchorMin = Vector2.zero;
            screenRect.anchorMax = Vector2.one;
            screenRect.offsetMin = Vector2.zero;
            screenRect.offsetMax = Vector2.zero;
            
            Image screenBg = gameOverScreen.AddComponent<Image>();
            screenBg.color = new Color(0, 0, 0, 0.9f);
            
            GameObject messageObj = new GameObject("GameOverMessage");
            messageObj.transform.SetParent(gameOverScreen.transform, false);
            RectTransform messageRect = messageObj.AddComponent<RectTransform>();
            messageRect.anchorMin = new Vector2(0.1f, 0.5f);
            messageRect.anchorMax = new Vector2(0.9f, 0.7f);
            messageRect.offsetMin = Vector2.zero;
            messageRect.offsetMax = Vector2.zero;
            
            gameOverMessage = messageObj.AddComponent<Text>();
            gameOverMessage.font = Font.CreateDynamicFontFromOSFont("Arial", 24);
            gameOverMessage.color = Color.white;
            gameOverMessage.alignment = TextAnchor.MiddleCenter;
            
            GameObject scoreObj = new GameObject("FinalScore");
            scoreObj.transform.SetParent(gameOverScreen.transform, false);
            RectTransform scoreRect = scoreObj.AddComponent<RectTransform>();
            scoreRect.anchorMin = new Vector2(0.1f, 0.3f);
            scoreRect.anchorMax = new Vector2(0.9f, 0.5f);
            scoreRect.offsetMin = Vector2.zero;
            scoreRect.offsetMax = Vector2.zero;
            
            finalScoreText = scoreObj.AddComponent<Text>();
            finalScoreText.font = Font.CreateDynamicFontFromOSFont("Arial", 18);
            finalScoreText.color = Color.white;
            finalScoreText.alignment = TextAnchor.MiddleCenter;
            
            gameOverScreen.SetActive(false);
        }

        private void CreateNotificationPanel()
        {
            notificationPanel = new GameObject("NotificationPanel");
            notificationPanel.transform.SetParent(canvas.transform, false);
            
            RectTransform notifRect = notificationPanel.AddComponent<RectTransform>();
            notifRect.anchorMin = new Vector2(0.3f, 0.8f);
            notifRect.anchorMax = new Vector2(0.7f, 0.85f);
            notifRect.offsetMin = Vector2.zero;
            notifRect.offsetMax = Vector2.zero;
            
            Image notifBg = notificationPanel.AddComponent<Image>();
            notifBg.color = new Color(0, 0, 0, 0.8f);
            
            notificationText = notificationPanel.AddComponent<Text>();
            notificationText.font = Font.CreateDynamicFontFromOSFont("Arial", 14);
            notificationText.color = Color.white;
            notificationText.alignment = TextAnchor.MiddleCenter;
            
            notificationPanel.SetActive(false);
        }

        public void UpdateScore(int score)
        {
            if (scoreText != null)
                scoreText.text = $"Score: {score:N0}";
        }

        public void UpdateFollowers(int followers)
        {
            if (followerText != null)
                followerText.text = $"Followers: {followers:N0}";
        }

        public void UpdateFlameGauge(float percentage)
        {
            if (flameGauge != null)
                flameGauge.value = percentage / 100f;
        }

        public void ShowCaptureResult(CaptureResultData result)
        {
            if (captureResultPopup == null) return;
            
            popupMessageText.text = result.Message;
            
            string stats = "";
            if (result.LikesGained > 0)
                stats += $"いいね +{result.LikesGained}\n";
            if (result.FollowersGained > 0)
                stats += $"フォロワー +{result.FollowersGained}";
            if (result.Backlash)
                stats += "\n⚠️ 炎上リスク！";
                
            popupStatsText.text = stats;
            captureResultPopup.SetActive(true);
            
            StartCoroutine(HidePopupAfterDelay(3f));
        }

        private IEnumerator HidePopupAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            HideCaptureResult();
        }

        public void HideCaptureResult()
        {
            if (captureResultPopup != null)
                captureResultPopup.SetActive(false);
        }

        public void SetCameraButtonInteractable(bool interactable)
        {
            if (cameraButton != null)
                cameraButton.interactable = interactable;
        }

        public void ShowGameOver(bool suspended, int finalScore, int finalFollowers)
        {
            if (gameOverScreen == null) return;
            
            if (suspended)
            {
                gameOverMessage.text = "アカウントが凍結されました";
            }
            else
            {
                gameOverMessage.text = "ゲーム終了";
            }
            
            finalScoreText.text = $"最終スコア: {finalScore:N0}\nフォロワー: {finalFollowers:N0}";
            gameOverScreen.SetActive(true);
        }

        public void ShowNotification(string message, float duration)
        {
            if (notificationPanel == null) return;
            
            notificationText.text = message;
            notificationPanel.SetActive(true);
            
            StartCoroutine(HideNotificationAfterDelay(duration));
        }

        private IEnumerator HideNotificationAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (notificationPanel != null)
                notificationPanel.SetActive(false);
        }

        public string GetScoreText() => scoreText?.text ?? "";
        public string GetFollowerText() => followerText?.text ?? "";
        public float GetFlameGaugeValue() => flameGauge?.value ?? 0f;
        public bool IsPopupActive() => captureResultPopup?.activeSelf ?? false;
        public bool IsCameraButtonInteractable() => cameraButton?.interactable ?? false;
        public bool IsGameOverScreenActive() => gameOverScreen?.activeSelf ?? false;
        public string GetGameOverMessage() => gameOverMessage?.text ?? "";
        public bool IsNotificationActive() => notificationPanel?.activeSelf ?? false;
        public Button GetCameraButton() => cameraButton;
        public Slider GetFlameGauge() => flameGauge;
    }
}