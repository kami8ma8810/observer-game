using NUnit.Framework;
using UnityEngine;
using JusticeManGO.Player;
using JusticeManGO.NPC;
using System.Collections.Generic;

namespace JusticeManGO.Tests.EditMode
{
    public class PhotoCaptureControllerTests
    {
        private PhotoCaptureController captureController;
        private GameObject playerGameObject;
        private GameObject npcGameObject;

        [SetUp]
        public void SetUp()
        {
            playerGameObject = new GameObject("TestPlayer");
            captureController = playerGameObject.AddComponent<PhotoCaptureController>();
            
            npcGameObject = new GameObject("TestNPC");
            npcGameObject.tag = "NPC";
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(playerGameObject);
            Object.DestroyImmediate(npcGameObject);
        }

        [Test]
        public void CanCapture_WhenNotOnCooldown_ShouldReturnTrue()
        {
            Assert.IsTrue(captureController.CanCapture());
        }

        [Test]
        public void CanCapture_WhenOnCooldown_ShouldReturnFalse()
        {
            captureController.SetCooldownTime(1f);
            captureController.TriggerCapture();
            
            Assert.IsFalse(captureController.CanCapture());
        }

        [Test]
        public void GetTargetsInRange_WithNPCInRange_ShouldReturnNPC()
        {
            npcGameObject.transform.position = new Vector3(2f, 0, 0);
            playerGameObject.transform.position = Vector3.zero;
            captureController.SetCaptureRange(5f);
            
            var targets = captureController.GetTargetsInRange();
            
            Assert.AreEqual(1, targets.Count);
            Assert.AreEqual(npcGameObject, targets[0]);
        }

        [Test]
        public void GetTargetsInRange_WithNPCOutOfRange_ShouldReturnEmpty()
        {
            npcGameObject.transform.position = new Vector3(10f, 0, 0);
            playerGameObject.transform.position = Vector3.zero;
            captureController.SetCaptureRange(5f);
            
            var targets = captureController.GetTargetsInRange();
            
            Assert.AreEqual(0, targets.Count);
        }

        [Test]
        public void TriggerCapture_ShouldReturnCaptureResult()
        {
            npcGameObject.AddComponent<NPCController>();
            npcGameObject.transform.position = new Vector3(2f, 0, 0);
            playerGameObject.transform.position = Vector3.zero;
            captureController.SetCaptureRange(5f);
            
            var result = captureController.TriggerCapture();
            
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(1, result.CapturedTargets.Count);
        }

        [Test]
        public void TriggerCapture_WithNoTargets_ShouldReturnFailedResult()
        {
            npcGameObject.transform.position = new Vector3(10f, 0, 0);
            playerGameObject.transform.position = Vector3.zero;
            captureController.SetCaptureRange(5f);
            
            var result = captureController.TriggerCapture();
            
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(0, result.CapturedTargets.Count);
        }

        [Test]
        public void SetCaptureAngle_ShouldLimitTargetsToAngle()
        {
            captureController.SetCaptureRange(5f);
            captureController.SetCaptureAngle(45f);
            
            npcGameObject.transform.position = new Vector3(3f, 0, 0);
            playerGameObject.transform.position = Vector3.zero;
            
            var playerMovement = playerGameObject.AddComponent<PlayerMovementController>();
            playerMovement.Move(1f, 0.1f);
            
            var targets = captureController.GetTargetsInRange();
            Assert.AreEqual(1, targets.Count);
            
            npcGameObject.transform.position = new Vector3(0, 3f, 0);
            targets = captureController.GetTargetsInRange();
            Assert.AreEqual(0, targets.Count);
        }

        [Test]
        public void GetCooldownRemaining_AfterCapture_ShouldReturnPositiveValue()
        {
            captureController.SetCooldownTime(2f);
            captureController.TriggerCapture();
            
            float remaining = captureController.GetCooldownRemaining();
            
            Assert.Greater(remaining, 0f);
            Assert.LessOrEqual(remaining, 2f);
        }
    }
}