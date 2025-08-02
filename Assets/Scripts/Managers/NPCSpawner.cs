using UnityEngine;
using System.Collections.Generic;
using JusticeManGO.NPC;

namespace JusticeManGO.Managers
{
    public class NPCSpawner : MonoBehaviour
    {
        [System.Serializable]
        public class SpawnConfiguration
        {
            public string npcId;
            public string violationType;
            public Vector3 position;
            public NPCBehavior.MovementPattern movementPattern = NPCBehavior.MovementPattern.Stationary;
            public float moveSpeed = 1f;
            public float patrolRange = 5f;
            public float violationDuration = 3f;
            public float violationMinInterval = 5f;
            public float violationMaxInterval = 10f;
            public bool autoViolation = true;

            public SpawnConfiguration() { }

            public SpawnConfiguration(SpawnConfiguration other)
            {
                npcId = other.npcId;
                violationType = other.violationType;
                position = other.position;
                movementPattern = other.movementPattern;
                moveSpeed = other.moveSpeed;
                patrolRange = other.patrolRange;
                violationDuration = other.violationDuration;
                violationMinInterval = other.violationMinInterval;
                violationMaxInterval = other.violationMaxInterval;
                autoViolation = other.autoViolation;
            }
        }

        private int maxNPCs = 10;
        private List<GameObject> activeNPCs = new List<GameObject>();
        private Dictionary<string, SpawnConfiguration> spawnPresets = new Dictionary<string, SpawnConfiguration>();

        private void Start()
        {
            InitializeDefaultPresets();
        }

        private void InitializeDefaultPresets()
        {
            AddSpawnPreset("street_smoker", new SpawnConfiguration
            {
                npcId = "street_smoker",
                violationType = "smoking_outside_area",
                movementPattern = NPCBehavior.MovementPattern.Stationary,
                violationDuration = 5f,
                violationMinInterval = 10f,
                violationMaxInterval = 20f
            });

            AddSpawnPreset("stroller_mother", new SpawnConfiguration
            {
                npcId = "stroller_mother",
                violationType = "blocking_sidewalk",
                movementPattern = NPCBehavior.MovementPattern.Patrol,
                moveSpeed = 0.5f,
                patrolRange = 8f,
                autoViolation = false
            });

            AddSpawnPreset("pigeon_feeder", new SpawnConfiguration
            {
                npcId = "pigeon_feeder",
                violationType = "feeding_pigeons",
                movementPattern = NPCBehavior.MovementPattern.Stationary,
                violationDuration = 10f,
                violationMinInterval = 15f,
                violationMaxInterval = 30f
            });

            AddSpawnPreset("delivery_driver", new SpawnConfiguration
            {
                npcId = "delivery_driver",
                violationType = "temporary_parking",
                movementPattern = NPCBehavior.MovementPattern.Stationary,
                violationDuration = 15f,
                violationMinInterval = 5f,
                violationMaxInterval = 10f
            });

            AddSpawnPreset("cyclist_student", new SpawnConfiguration
            {
                npcId = "cyclist_student",
                violationType = "wrong_way_cycling",
                movementPattern = NPCBehavior.MovementPattern.Patrol,
                moveSpeed = 3f,
                patrolRange = 15f,
                autoViolation = false
            });
        }

        public void SetMaxNPCs(int max)
        {
            maxNPCs = max;
        }

        public void AddSpawnPreset(string presetName, SpawnConfiguration config)
        {
            spawnPresets[presetName] = config;
        }

        public GameObject SpawnNPC(SpawnConfiguration config)
        {
            if (activeNPCs.Count >= maxNPCs)
            {
                Debug.LogWarning("Maximum NPC count reached");
                return null;
            }

            GameObject npcObject = new GameObject($"NPC_{config.npcId}");
            npcObject.tag = "NPC";
            npcObject.transform.position = config.position;

            NPCController controller = npcObject.AddComponent<NPCController>();
            controller.Initialize(config.npcId, config.violationType);

            NPCBehavior behavior = npcObject.AddComponent<NPCBehavior>();
            ConfigureBehavior(behavior, config);

            SpriteRenderer renderer = npcObject.AddComponent<SpriteRenderer>();
            renderer.sortingLayerName = "Default";
            renderer.sortingOrder = 1;

            activeNPCs.Add(npcObject);
            return npcObject;
        }

        private void ConfigureBehavior(NPCBehavior behavior, SpawnConfiguration config)
        {
            behavior.SetMovementPattern(config.movementPattern);
            behavior.SetMoveSpeed(config.moveSpeed);
            behavior.SetPatrolRange(config.patrolRange);
            behavior.SetViolationDuration(config.violationDuration);
            behavior.SetViolationInterval(config.violationMinInterval, config.violationMaxInterval);
            behavior.EnableAutoViolation(config.autoViolation);

            if (config.autoViolation)
            {
                behavior.StartViolationBehavior();
            }
        }

        public GameObject SpawnRandomNPC(Vector3 position)
        {
            if (spawnPresets.Count == 0) return null;

            List<string> presetKeys = new List<string>(spawnPresets.Keys);
            string randomKey = presetKeys[Random.Range(0, presetKeys.Count)];
            
            SpawnConfiguration config = new SpawnConfiguration(spawnPresets[randomKey]);
            config.position = position;
            
            return SpawnNPC(config);
        }

        public void RemoveNPC(GameObject npc)
        {
            if (activeNPCs.Contains(npc))
            {
                activeNPCs.Remove(npc);
                Destroy(npc);
            }
        }

        public int GetActiveNPCCount()
        {
            activeNPCs.RemoveAll(npc => npc == null);
            return activeNPCs.Count;
        }

        public List<GameObject> GetActiveNPCs()
        {
            activeNPCs.RemoveAll(npc => npc == null);
            return new List<GameObject>(activeNPCs);
        }

        private void OnDestroy()
        {
            foreach (var npc in activeNPCs)
            {
                if (npc != null)
                {
                    Destroy(npc);
                }
            }
            activeNPCs.Clear();
        }
    }
}