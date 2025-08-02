using UnityEngine;
using System.Collections.Generic;
using JusticeManGO.NPC;

namespace JusticeManGO.Player
{
    public class PhotoCaptureController : MonoBehaviour
    {
        private float captureRange = 5f;
        private float captureAngle = 60f;
        private float cooldownTime = 1f;
        private float lastCaptureTime = -999f;

        public class CaptureResult
        {
            public bool Success { get; set; }
            public List<GameObject> CapturedTargets { get; set; } = new List<GameObject>();
            public Vector3 CapturePosition { get; set; }
            public float CaptureTime { get; set; }
        }

        public void SetCaptureRange(float range)
        {
            captureRange = range;
        }

        public void SetCaptureAngle(float angle)
        {
            captureAngle = angle;
        }

        public void SetCooldownTime(float cooldown)
        {
            cooldownTime = cooldown;
        }

        public bool CanCapture()
        {
            return Time.time - lastCaptureTime >= cooldownTime;
        }

        public float GetCooldownRemaining()
        {
            float elapsed = Time.time - lastCaptureTime;
            return Mathf.Max(0, cooldownTime - elapsed);
        }

        public List<GameObject> GetTargetsInRange()
        {
            List<GameObject> targets = new List<GameObject>();
            GameObject[] allNPCs = GameObject.FindGameObjectsWithTag("NPC");
            
            PlayerMovementController playerMovement = GetComponent<PlayerMovementController>();
            int facingDirection = playerMovement != null ? playerMovement.FacingDirection : 1;

            foreach (GameObject npc in allNPCs)
            {
                float distance = Vector3.Distance(transform.position, npc.transform.position);
                
                if (distance <= captureRange)
                {
                    if (captureAngle >= 360f)
                    {
                        targets.Add(npc);
                    }
                    else
                    {
                        Vector3 directionToNPC = (npc.transform.position - transform.position).normalized;
                        Vector3 facingVector = new Vector3(facingDirection, 0, 0);
                        float angle = Vector3.Angle(facingVector, directionToNPC);
                        
                        if (angle <= captureAngle / 2f)
                        {
                            targets.Add(npc);
                        }
                    }
                }
            }

            return targets;
        }

        public CaptureResult TriggerCapture()
        {
            CaptureResult result = new CaptureResult
            {
                CapturePosition = transform.position,
                CaptureTime = Time.time
            };

            if (!CanCapture())
            {
                result.Success = false;
                return result;
            }

            lastCaptureTime = Time.time;
            List<GameObject> targets = GetTargetsInRange();

            if (targets.Count > 0)
            {
                result.Success = true;
                result.CapturedTargets = targets;
            }
            else
            {
                result.Success = false;
            }

            return result;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                TriggerCapture();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, captureRange);
            
            PlayerMovementController playerMovement = GetComponent<PlayerMovementController>();
            if (playerMovement != null && captureAngle < 360f)
            {
                Vector3 forward = new Vector3(playerMovement.FacingDirection, 0, 0);
                float halfAngle = captureAngle / 2f;
                
                Quaternion upRotation = Quaternion.AngleAxis(halfAngle, Vector3.forward);
                Quaternion downRotation = Quaternion.AngleAxis(-halfAngle, Vector3.forward);
                
                Vector3 upDirection = upRotation * forward * captureRange;
                Vector3 downDirection = downRotation * forward * captureRange;
                
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, transform.position + upDirection);
                Gizmos.DrawLine(transform.position, transform.position + downDirection);
            }
        }
    }
}