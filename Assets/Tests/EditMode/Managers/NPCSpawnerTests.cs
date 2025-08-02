using NUnit.Framework;
using UnityEngine;
using JusticeManGO.Managers;
using JusticeManGO.NPC;
using System.Collections.Generic;

namespace JusticeManGO.Tests.EditMode
{
    public class NPCSpawnerTests
    {
        private NPCSpawner spawner;
        private GameObject spawnerObject;

        [SetUp]
        public void SetUp()
        {
            spawnerObject = new GameObject("TestSpawner");
            spawner = spawnerObject.AddComponent<NPCSpawner>();
        }

        [TearDown]
        public void TearDown()
        {
            foreach (var npc in GameObject.FindGameObjectsWithTag("NPC"))
            {
                Object.DestroyImmediate(npc);
            }
            Object.DestroyImmediate(spawnerObject);
        }

        [Test]
        public void SpawnNPC_ShouldCreateNPCWithCorrectComponents()
        {
            var spawnConfig = new NPCSpawner.SpawnConfiguration
            {
                npcId = "street_smoker",
                position = Vector3.zero,
                movementPattern = NPCBehavior.MovementPattern.Stationary
            };
            
            GameObject npc = spawner.SpawnNPC(spawnConfig);
            
            Assert.IsNotNull(npc);
            Assert.IsNotNull(npc.GetComponent<NPCController>());
            Assert.IsNotNull(npc.GetComponent<NPCBehavior>());
            Assert.AreEqual("NPC", npc.tag);
        }

        [Test]
        public void SpawnNPC_ShouldInitializeNPCController()
        {
            var spawnConfig = new NPCSpawner.SpawnConfiguration
            {
                npcId = "street_smoker",
                violationType = "smoking_outside_area",
                position = Vector3.zero
            };
            
            GameObject npc = spawner.SpawnNPC(spawnConfig);
            var controller = npc.GetComponent<NPCController>();
            
            Assert.AreEqual("street_smoker", controller.NPCId);
            Assert.AreEqual("smoking_outside_area", controller.ViolationType);
        }

        [Test]
        public void SpawnNPC_ShouldConfigureBehavior()
        {
            var spawnConfig = new NPCSpawner.SpawnConfiguration
            {
                npcId = "delivery_driver",
                position = new Vector3(5, 0, 0),
                movementPattern = NPCBehavior.MovementPattern.Patrol,
                moveSpeed = 2f,
                patrolRange = 10f
            };
            
            GameObject npc = spawner.SpawnNPC(spawnConfig);
            
            Assert.AreEqual(new Vector3(5, 0, 0), npc.transform.position);
        }

        [Test]
        public void SetMaxNPCs_ShouldLimitSpawning()
        {
            spawner.SetMaxNPCs(2);
            
            var config = new NPCSpawner.SpawnConfiguration { npcId = "test" };
            
            spawner.SpawnNPC(config);
            spawner.SpawnNPC(config);
            GameObject thirdNPC = spawner.SpawnNPC(config);
            
            Assert.IsNull(thirdNPC);
            Assert.AreEqual(2, spawner.GetActiveNPCCount());
        }

        [Test]
        public void GetActiveNPCs_ShouldReturnAllSpawnedNPCs()
        {
            var config = new NPCSpawner.SpawnConfiguration { npcId = "test" };
            
            spawner.SpawnNPC(config);
            spawner.SpawnNPC(config);
            
            List<GameObject> activeNPCs = spawner.GetActiveNPCs();
            
            Assert.AreEqual(2, activeNPCs.Count);
        }

        [Test]
        public void RemoveNPC_ShouldDecreaseActiveCount()
        {
            var config = new NPCSpawner.SpawnConfiguration { npcId = "test" };
            GameObject npc = spawner.SpawnNPC(config);
            
            Assert.AreEqual(1, spawner.GetActiveNPCCount());
            
            spawner.RemoveNPC(npc);
            
            Assert.AreEqual(0, spawner.GetActiveNPCCount());
        }

        [Test]
        public void SpawnRandomNPC_ShouldUseRandomConfiguration()
        {
            spawner.AddSpawnPreset("street_smoker", new NPCSpawner.SpawnConfiguration
            {
                npcId = "street_smoker",
                violationType = "smoking_outside_area",
                movementPattern = NPCBehavior.MovementPattern.Stationary
            });
            
            spawner.AddSpawnPreset("delivery_driver", new NPCSpawner.SpawnConfiguration
            {
                npcId = "delivery_driver",
                violationType = "temporary_parking",
                movementPattern = NPCBehavior.MovementPattern.Patrol
            });
            
            GameObject npc = spawner.SpawnRandomNPC(Vector3.zero);
            
            Assert.IsNotNull(npc);
            var controller = npc.GetComponent<NPCController>();
            Assert.IsTrue(controller.NPCId == "street_smoker" || controller.NPCId == "delivery_driver");
        }
    }
}