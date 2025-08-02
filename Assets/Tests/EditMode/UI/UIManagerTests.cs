using NUnit.Framework;
using UnityEngine;
using JusticeManGO.UI;
using JusticeManGO.Managers;

namespace JusticeManGO.Tests.EditMode
{
    public class UIManagerTests
    {
        private UIManager uiManager;
        private GameObject uiManagerObject;

        [SetUp]
        public void SetUp()
        {
            uiManagerObject = new GameObject("TestUIManager");
            uiManager = uiManagerObject.AddComponent<UIManager>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(uiManagerObject);
        }

        [Test]
        public void Initialize_ShouldCreateUIElements()
        {
            uiManager.Initialize();
            
            Assert.IsNotNull(uiManager.GetScoreText());
            Assert.IsNotNull(uiManager.GetFollowerText());
            Assert.IsNotNull(uiManager.GetFlameGauge());
            Assert.IsNotNull(uiManager.GetCameraButton());
        }

        [Test]
        public void UpdateScore_ShouldChangeScoreText()
        {
            uiManager.Initialize();
            
            uiManager.UpdateScore(1000);
            
            string scoreText = uiManager.GetScoreText();
            Assert.IsTrue(scoreText.Contains("1000") || scoreText.Contains("1,000"));
        }

        [Test]
        public void UpdateFollowers_ShouldChangeFollowerText()
        {
            uiManager.Initialize();
            
            uiManager.UpdateFollowers(500);
            
            string followerText = uiManager.GetFollowerText();
            Assert.IsTrue(followerText.Contains("500"));
        }

        [Test]
        public void UpdateFlameGauge_ShouldChangeGaugeFill()
        {
            uiManager.Initialize();
            
            uiManager.UpdateFlameGauge(50f);
            
            float gaugeValue = uiManager.GetFlameGaugeValue();
            Assert.AreEqual(0.5f, gaugeValue, 0.01f);
        }

        [Test]
        public void ShowCaptureResult_ShouldDisplayPopup()
        {
            uiManager.Initialize();
            
            var result = new UIManager.CaptureResultData
            {
                Success = true,
                Message = "路上喫煙を通報しました",
                LikesGained = 20,
                FollowersGained = 5
            };
            
            uiManager.ShowCaptureResult(result);
            
            Assert.IsTrue(uiManager.IsPopupActive());
        }

        [Test]
        public void HideCaptureResult_ShouldHidePopup()
        {
            uiManager.Initialize();
            
            var result = new UIManager.CaptureResultData { Success = true };
            uiManager.ShowCaptureResult(result);
            uiManager.HideCaptureResult();
            
            Assert.IsFalse(uiManager.IsPopupActive());
        }

        [Test]
        public void SetCameraButtonInteractable_ShouldChangeButtonState()
        {
            uiManager.Initialize();
            
            uiManager.SetCameraButtonInteractable(false);
            Assert.IsFalse(uiManager.IsCameraButtonInteractable());
            
            uiManager.SetCameraButtonInteractable(true);
            Assert.IsTrue(uiManager.IsCameraButtonInteractable());
        }

        [Test]
        public void ShowGameOver_WithSuspension_ShouldShowSuspensionMessage()
        {
            uiManager.Initialize();
            
            uiManager.ShowGameOver(true, 1000, 100);
            
            Assert.IsTrue(uiManager.IsGameOverScreenActive());
            string message = uiManager.GetGameOverMessage();
            Assert.IsTrue(message.Contains("凍結") || message.Contains("停止"));
        }

        [Test]
        public void ShowNotification_ShouldDisplayTemporaryMessage()
        {
            uiManager.Initialize();
            
            uiManager.ShowNotification("いいねが増えました！", 2f);
            
            Assert.IsTrue(uiManager.IsNotificationActive());
        }
    }
}