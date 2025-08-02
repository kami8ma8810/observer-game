using NUnit.Framework;
using UnityEngine;
using JusticeManGO.Managers;
using JusticeManGO.NPC;
using JusticeManGO.Player;

namespace JusticeManGO.Tests.EditMode
{
    public class ViolationDetectionManagerTests
    {
        private ViolationDetectionManager detectionManager;
        private GameObject managerObject;
        private GameObject npcObject;
        private PhotoCaptureController captureController;

        [SetUp]
        public void SetUp()
        {
            managerObject = new GameObject("TestDetectionManager");
            detectionManager = managerObject.AddComponent<ViolationDetectionManager>();
            
            GameObject playerObject = new GameObject("TestPlayer");
            captureController = playerObject.AddComponent<PhotoCaptureController>();
            
            npcObject = new GameObject("TestNPC");
            npcObject.tag = "NPC";
            var npcController = npcObject.AddComponent<NPCController>();
            npcController.Initialize("street_smoker", "smoking_outside_area");
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(managerObject);
            Object.DestroyImmediate(npcObject);
            if (captureController != null)
                Object.DestroyImmediate(captureController.gameObject);
        }

        [Test]
        public void ProcessCapture_WithViolatingNPC_ShouldReturnSuccess()
        {
            var npcController = npcObject.GetComponent<NPCController>();
            npcController.StartViolation();
            
            var captureResult = new PhotoCaptureController.CaptureResult
            {
                Success = true,
                CapturedTargets = new System.Collections.Generic.List<GameObject> { npcObject }
            };
            
            var detectionResult = detectionManager.ProcessCapture(captureResult);
            
            Assert.IsTrue(detectionResult.IsValidReport);
            Assert.AreEqual(1, detectionResult.DetectedViolations.Count);
        }

        [Test]
        public void ProcessCapture_WithNonViolatingNPC_ShouldReturnFalseReport()
        {
            var npcController = npcObject.GetComponent<NPCController>();
            npcController.StopViolation();
            
            var captureResult = new PhotoCaptureController.CaptureResult
            {
                Success = true,
                CapturedTargets = new System.Collections.Generic.List<GameObject> { npcObject }
            };
            
            var detectionResult = detectionManager.ProcessCapture(captureResult);
            
            Assert.IsFalse(detectionResult.IsValidReport);
            Assert.AreEqual(0, detectionResult.DetectedViolations.Count);
        }

        [Test]
        public void ProcessCapture_WithMultipleNPCs_ShouldDetectAllViolations()
        {
            GameObject npc2 = new GameObject("TestNPC2");
            npc2.tag = "NPC";
            var npc2Controller = npc2.AddComponent<NPCController>();
            npc2Controller.Initialize("delivery_driver", "temporary_parking");
            npc2Controller.StartViolation();
            
            npcObject.GetComponent<NPCController>().StartViolation();
            
            var captureResult = new PhotoCaptureController.CaptureResult
            {
                Success = true,
                CapturedTargets = new System.Collections.Generic.List<GameObject> { npcObject, npc2 }
            };
            
            var detectionResult = detectionManager.ProcessCapture(captureResult);
            
            Assert.IsTrue(detectionResult.IsValidReport);
            Assert.AreEqual(2, detectionResult.DetectedViolations.Count);
            
            Object.DestroyImmediate(npc2);
        }

        [Test]
        public void GetReactionForViolation_ShouldReturnReactionData()
        {
            detectionManager.LoadNPCData(CreateTestNPCData());
            
            var reaction = detectionManager.GetReactionForViolation("street_smoker", "smoking_outside_area");
            
            Assert.IsNotNull(reaction);
            Assert.Greater(reaction.LikeRange.x, 0);
        }

        private string CreateTestNPCData()
        {
            return @"{
                'npcs': [{
                    'id': 'street_smoker',
                    'violationType': 'smoking_outside_area',
                    'reactions': {
                        'report_success': {
                            'probability': 0.7,
                            'like_range': [10, 30]
                        }
                    }
                }]
            }".Replace("'", "\"");
        }
    }
}