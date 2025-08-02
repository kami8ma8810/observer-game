using NUnit.Framework;
using UnityEngine;
using JusticeManGO.Managers;
using JusticeManGO.Player;
using JusticeManGO.Camera;

namespace JusticeManGO.Tests.EditMode
{
    public class GameManagerTests
    {
        private GameManager gameManager;
        private GameObject gameManagerObject;

        [SetUp]
        public void SetUp()
        {
            gameManagerObject = new GameObject("TestGameManager");
            gameManager = gameManagerObject.AddComponent<GameManager>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(gameManagerObject);
        }

        [Test]
        public void Initialize_ShouldCreatePlayerAndCamera()
        {
            gameManager.Initialize();
            
            Assert.IsNotNull(gameManager.Player);
            Assert.IsNotNull(gameManager.MainCamera);
            Assert.IsNotNull(gameManager.Player.GetComponent<PlayerMovementController>());
            Assert.IsNotNull(gameManager.MainCamera.GetComponent<CameraFollowController>());
        }

        [Test]
        public void Initialize_ShouldSetupCameraToFollowPlayer()
        {
            gameManager.Initialize();
            
            var cameraController = gameManager.MainCamera.GetComponent<CameraFollowController>();
            Assert.AreEqual(gameManager.Player.transform, cameraController.Target);
        }

        [Test]
        public void SetLevelBounds_ShouldConfigureCameraAndPlayerBounds()
        {
            gameManager.Initialize();
            Bounds levelBounds = new Bounds(Vector3.zero, new Vector3(20f, 10f, 0));
            
            gameManager.SetLevelBounds(levelBounds);
            
            var playerController = gameManager.Player.GetComponent<PlayerMovementController>();
            Assert.AreEqual(-10f, playerController.GetMinBound());
            Assert.AreEqual(10f, playerController.GetMaxBound());
        }

        [Test]
        public void GetCurrentScore_ShouldReturnZeroInitially()
        {
            gameManager.Initialize();
            
            Assert.AreEqual(0, gameManager.GetCurrentScore());
        }

        [Test]
        public void AddScore_ShouldIncreaseScore()
        {
            gameManager.Initialize();
            
            gameManager.AddScore(100);
            
            Assert.AreEqual(100, gameManager.GetCurrentScore());
        }

        [Test]
        public void GetFollowerCount_ShouldReturnInitialFollowers()
        {
            gameManager.Initialize();
            
            Assert.AreEqual(0, gameManager.GetFollowerCount());
        }

        [Test]
        public void AddFollowers_ShouldIncreaseFollowerCount()
        {
            gameManager.Initialize();
            
            gameManager.AddFollowers(50);
            
            Assert.AreEqual(50, gameManager.GetFollowerCount());
        }

        [Test]
        public void GetFlameGauge_ShouldReturnZeroInitially()
        {
            gameManager.Initialize();
            
            Assert.AreEqual(0f, gameManager.GetFlameGauge());
        }

        [Test]
        public void IncreaseFlameGauge_ShouldNotExceed100()
        {
            gameManager.Initialize();
            
            gameManager.IncreaseFlameGauge(150f);
            
            Assert.AreEqual(100f, gameManager.GetFlameGauge());
        }
    }
}