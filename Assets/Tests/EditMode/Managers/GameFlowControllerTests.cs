using NUnit.Framework;
using UnityEngine;
using JusticeManGO.Managers;
using JusticeManGO.Player;
using JusticeManGO.UI;

namespace JusticeManGO.Tests.EditMode
{
    public class GameFlowControllerTests
    {
        private GameFlowController flowController;
        private GameObject controllerObject;

        [SetUp]
        public void SetUp()
        {
            controllerObject = new GameObject("TestFlowController");
            flowController = controllerObject.AddComponent<GameFlowController>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(controllerObject);
            if (GameManager.Instance != null)
            {
                Object.DestroyImmediate(GameManager.Instance.gameObject);
            }
        }

        [Test]
        public void StartGame_ShouldInitializeAllSystems()
        {
            flowController.StartGame();
            
            Assert.IsNotNull(flowController.GetGameManager());
            Assert.IsNotNull(flowController.GetUIManager());
            Assert.IsNotNull(flowController.GetNPCSpawner());
            Assert.IsNotNull(flowController.GetViolationDetectionManager());
        }

        [Test]
        public void ProcessPhotoCapture_WithValidCapture_ShouldUpdateScore()
        {
            flowController.StartGame();
            int initialScore = flowController.GetCurrentScore();
            
            var captureResult = new PhotoCaptureController.CaptureResult
            {
                Success = true,
                CapturedTargets = new System.Collections.Generic.List<GameObject>()
            };
            
            flowController.ProcessPhotoCapture(captureResult, true);
            
            Assert.Greater(flowController.GetCurrentScore(), initialScore);
        }

        [Test]
        public void ProcessPhotoCapture_WithFalseReport_ShouldIncreaseFlameGauge()
        {
            flowController.StartGame();
            float initialFlame = flowController.GetFlameGauge();
            
            var captureResult = new PhotoCaptureController.CaptureResult
            {
                Success = true,
                CapturedTargets = new System.Collections.Generic.List<GameObject>()
            };
            
            flowController.ProcessPhotoCapture(captureResult, false);
            
            Assert.Greater(flowController.GetFlameGauge(), initialFlame);
        }

        [Test]
        public void CheckGameOver_WithHighFlameGauge_ShouldEndGame()
        {
            flowController.StartGame();
            flowController.SetFlameGauge(100f);
            
            bool isGameOver = flowController.CheckGameOver();
            
            Assert.IsTrue(isGameOver);
            Assert.IsTrue(flowController.IsGameEnded());
        }

        [Test]
        public void UpdateGameTimer_AfterTimeLimit_ShouldEndGame()
        {
            flowController.StartGame();
            flowController.SetGameDuration(1f);
            
            flowController.UpdateGameTimer(1.5f);
            
            Assert.IsTrue(flowController.IsGameEnded());
        }

        [Test]
        public void SpawnNPCWave_ShouldCreateMultipleNPCs()
        {
            flowController.StartGame();
            
            int spawnCount = flowController.SpawnNPCWave(3);
            
            Assert.AreEqual(3, spawnCount);
        }

        [Test]
        public void CalculateReactionOutcome_ShouldReturnReactionData()
        {
            flowController.StartGame();
            
            var outcome = flowController.CalculateReactionOutcome("street_smoker", true);
            
            Assert.IsNotNull(outcome);
            Assert.GreaterOrEqual(outcome.LikesGained, 0);
            Assert.GreaterOrEqual(outcome.FollowersGained, 0);
        }

        [Test]
        public void GetGameStats_ShouldReturnCurrentStats()
        {
            flowController.StartGame();
            flowController.AddScore(100);
            flowController.AddFollowers(50);
            
            var stats = flowController.GetGameStats();
            
            Assert.AreEqual(100, stats.Score);
            Assert.AreEqual(50, stats.Followers);
            Assert.GreaterOrEqual(stats.TimeRemaining, 0);
        }

        [Test]
        public void PauseGame_ShouldStopGameFlow()
        {
            flowController.StartGame();
            
            flowController.PauseGame();
            
            Assert.IsTrue(flowController.IsPaused());
        }

        [Test]
        public void ResumeGame_ShouldContinueGameFlow()
        {
            flowController.StartGame();
            flowController.PauseGame();
            
            flowController.ResumeGame();
            
            Assert.IsFalse(flowController.IsPaused());
        }
    }
}