using UnityEngine;

namespace JusticeManGO.NPC
{
    public class NPCController : MonoBehaviour
    {
        private string npcId;
        private string violationType;
        private bool isViolating = false;

        public string NPCId => npcId;
        public string ViolationType => violationType;
        public bool IsViolating => isViolating;

        public void Initialize(string id, string violation)
        {
            npcId = id;
            violationType = violation;
        }

        public void StartViolation()
        {
            isViolating = true;
        }

        public void StopViolation()
        {
            isViolating = false;
        }

        public bool CanBeCaptured()
        {
            return isViolating;
        }
    }
}