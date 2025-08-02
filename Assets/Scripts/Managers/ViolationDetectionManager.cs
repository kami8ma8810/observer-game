using UnityEngine;
using System.Collections.Generic;
using JusticeManGO.Player;
using JusticeManGO.NPC;
using JusticeManGO.Data;
using System.Linq;

namespace JusticeManGO.Managers
{
    public class ViolationDetectionManager : MonoBehaviour
    {
        private NPCDataContainer npcData;

        public class DetectionResult
        {
            public bool IsValidReport { get; set; }
            public List<ViolationInfo> DetectedViolations { get; set; } = new List<ViolationInfo>();
            public bool IsFalseReport => !IsValidReport && DetectedViolations.Count == 0;
        }

        public class ViolationInfo
        {
            public string NPCId { get; set; }
            public string ViolationType { get; set; }
            public GameObject NPCObject { get; set; }
        }

        public class ReactionData
        {
            public Vector2 LikeRange { get; set; }
            public float RetweetProbability { get; set; }
            public float BacklashProbability { get; set; }
            public string Message { get; set; }
        }

        private void Start()
        {
            LoadNPCDataFromFile();
        }

        private void LoadNPCDataFromFile()
        {
            TextAsset jsonFile = Resources.Load<TextAsset>("Data/NPCData");
            if (jsonFile != null)
            {
                LoadNPCData(jsonFile.text);
            }
        }

        public void LoadNPCData(string jsonData)
        {
            npcData = JsonUtility.FromJson<NPCDataContainer>(jsonData);
        }

        public DetectionResult ProcessCapture(PhotoCaptureController.CaptureResult captureResult)
        {
            DetectionResult result = new DetectionResult();

            if (!captureResult.Success || captureResult.CapturedTargets.Count == 0)
            {
                result.IsValidReport = false;
                return result;
            }

            foreach (GameObject target in captureResult.CapturedTargets)
            {
                NPCController npcController = target.GetComponent<NPCController>();
                if (npcController != null && npcController.CanBeCaptured())
                {
                    result.DetectedViolations.Add(new ViolationInfo
                    {
                        NPCId = npcController.NPCId,
                        ViolationType = npcController.ViolationType,
                        NPCObject = target
                    });
                }
            }

            result.IsValidReport = result.DetectedViolations.Count > 0;
            return result;
        }

        public ReactionData GetReactionForViolation(string npcId, string violationType)
        {
            if (npcData == null) return GetDefaultReaction();

            var npc = npcData.npcs.FirstOrDefault(n => n.id == npcId);
            if (npc == null) return GetDefaultReaction();

            float randomValue = Random.value;
            NPCReaction selectedReaction = null;

            if (npc.reactions.report_success != null && randomValue < npc.reactions.report_success.probability)
            {
                selectedReaction = npc.reactions.report_success;
            }
            else if (npc.reactions.excessive_justice != null)
            {
                selectedReaction = npc.reactions.excessive_justice;
            }

            if (selectedReaction != null)
            {
                return new ReactionData
                {
                    LikeRange = new Vector2(selectedReaction.like_range[0], selectedReaction.like_range[1]),
                    RetweetProbability = selectedReaction.retweet_probability,
                    BacklashProbability = selectedReaction.backlash_probability,
                    Message = selectedReaction.messages != null && selectedReaction.messages.Length > 0 
                        ? selectedReaction.messages[Random.Range(0, selectedReaction.messages.Length)]
                        : "違反行為を通報しました"
                };
            }

            return GetDefaultReaction();
        }

        private ReactionData GetDefaultReaction()
        {
            return new ReactionData
            {
                LikeRange = new Vector2(5, 15),
                RetweetProbability = 0.3f,
                BacklashProbability = 0.1f,
                Message = "通報しました"
            };
        }
    }
}