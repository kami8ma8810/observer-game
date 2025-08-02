using NUnit.Framework;
using UnityEngine;
using JusticeManGO.NPC;

namespace JusticeManGO.Tests.EditMode
{
    public class NPCBehaviorTests
    {
        private GameObject npcObject;
        private NPCBehavior npcBehavior;

        [SetUp]
        public void SetUp()
        {
            npcObject = new GameObject("TestNPC");
            npcBehavior = npcObject.AddComponent<NPCBehavior>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(npcObject);
        }

        [Test]
        public void SetMovementPattern_Stationary_ShouldNotMove()
        {
            npcBehavior.SetMovementPattern(NPCBehavior.MovementPattern.Stationary);
            Vector3 initialPosition = npcObject.transform.position;
            
            npcBehavior.UpdateMovement(0.1f);
            
            Assert.AreEqual(initialPosition, npcObject.transform.position);
        }

        [Test]
        public void SetMovementPattern_Patrol_ShouldMoveBackAndForth()
        {
            npcBehavior.SetMovementPattern(NPCBehavior.MovementPattern.Patrol);
            npcBehavior.SetPatrolRange(5f);
            npcBehavior.SetMoveSpeed(2f);
            
            Vector3 initialPosition = npcObject.transform.position;
            
            for (int i = 0; i < 10; i++)
            {
                npcBehavior.UpdateMovement(0.5f);
            }
            
            Assert.AreNotEqual(initialPosition, npcObject.transform.position);
        }

        [Test]
        public void SetMovementPattern_Random_ShouldMoveRandomly()
        {
            npcBehavior.SetMovementPattern(NPCBehavior.MovementPattern.Random);
            npcBehavior.SetMoveSpeed(2f);
            
            Vector3 position1 = npcObject.transform.position;
            npcBehavior.UpdateMovement(1f);
            Vector3 position2 = npcObject.transform.position;
            npcBehavior.UpdateMovement(1f);
            Vector3 position3 = npcObject.transform.position;
            
            bool moved = position1 != position2 || position2 != position3;
            Assert.IsTrue(moved);
        }

        [Test]
        public void StartViolationBehavior_ShouldTriggerViolation()
        {
            var npcController = npcObject.AddComponent<NPCController>();
            npcController.Initialize("test_npc", "test_violation");
            
            npcBehavior.SetViolationDuration(2f);
            npcBehavior.StartViolationBehavior();
            
            Assert.IsTrue(npcController.IsViolating);
        }

        [Test]
        public void UpdateViolationTimer_ShouldStopViolationAfterDuration()
        {
            var npcController = npcObject.AddComponent<NPCController>();
            npcController.Initialize("test_npc", "test_violation");
            
            npcBehavior.SetViolationDuration(1f);
            npcBehavior.StartViolationBehavior();
            
            Assert.IsTrue(npcController.IsViolating);
            
            npcBehavior.UpdateViolationTimer(1.5f);
            
            Assert.IsFalse(npcController.IsViolating);
        }

        [Test]
        public void SetViolationInterval_ShouldRepeatViolations()
        {
            var npcController = npcObject.AddComponent<NPCController>();
            npcController.Initialize("test_npc", "test_violation");
            
            npcBehavior.SetViolationDuration(0.5f);
            npcBehavior.SetViolationInterval(1f, 1f);
            npcBehavior.EnableAutoViolation(true);
            
            int violationCount = 0;
            float totalTime = 0f;
            
            while (totalTime < 3f)
            {
                npcBehavior.UpdateBehavior(0.1f);
                if (npcController.IsViolating && !npcBehavior.WasViolatingLastFrame())
                {
                    violationCount++;
                }
                totalTime += 0.1f;
            }
            
            Assert.GreaterOrEqual(violationCount, 2);
        }
    }
}