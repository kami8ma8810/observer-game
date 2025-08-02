using UnityEngine;

namespace JusticeManGO.NPC
{
    public class NPCBehavior : MonoBehaviour
    {
        public enum MovementPattern
        {
            Stationary,
            Patrol,
            Random
        }

        private MovementPattern movementPattern = MovementPattern.Stationary;
        private float moveSpeed = 1f;
        private float patrolRange = 5f;
        private Vector3 patrolStartPosition;
        private int patrolDirection = 1;
        
        private float violationDuration = 3f;
        private float violationMinInterval = 5f;
        private float violationMaxInterval = 10f;
        private bool autoViolation = false;
        
        private float violationTimer = 0f;
        private float nextViolationTime = 0f;
        private bool isInViolation = false;
        private bool wasViolatingLastFrame = false;
        
        private float randomMoveTimer = 0f;
        private Vector3 randomMoveDirection;

        private NPCController npcController;

        private void Start()
        {
            npcController = GetComponent<NPCController>();
            patrolStartPosition = transform.position;
            ScheduleNextViolation();
        }

        private void Update()
        {
            UpdateBehavior(Time.deltaTime);
        }

        public void UpdateBehavior(float deltaTime)
        {
            UpdateMovement(deltaTime);
            
            if (autoViolation)
            {
                UpdateAutoViolation(deltaTime);
            }
            
            UpdateViolationTimer(deltaTime);
        }

        public void SetMovementPattern(MovementPattern pattern)
        {
            movementPattern = pattern;
            if (pattern == MovementPattern.Patrol)
            {
                patrolStartPosition = transform.position;
            }
        }

        public void SetMoveSpeed(float speed)
        {
            moveSpeed = speed;
        }

        public void SetPatrolRange(float range)
        {
            patrolRange = range;
        }

        public void SetViolationDuration(float duration)
        {
            violationDuration = duration;
        }

        public void SetViolationInterval(float min, float max)
        {
            violationMinInterval = min;
            violationMaxInterval = max;
        }

        public void EnableAutoViolation(bool enable)
        {
            autoViolation = enable;
            if (enable)
            {
                ScheduleNextViolation();
            }
        }

        public void StartViolationBehavior()
        {
            if (npcController != null)
            {
                npcController.StartViolation();
                isInViolation = true;
                violationTimer = violationDuration;
            }
        }

        public void UpdateMovement(float deltaTime)
        {
            switch (movementPattern)
            {
                case MovementPattern.Stationary:
                    break;
                    
                case MovementPattern.Patrol:
                    UpdatePatrolMovement(deltaTime);
                    break;
                    
                case MovementPattern.Random:
                    UpdateRandomMovement(deltaTime);
                    break;
            }
        }

        private void UpdatePatrolMovement(float deltaTime)
        {
            Vector3 currentPos = transform.position;
            float distanceFromStart = currentPos.x - patrolStartPosition.x;
            
            if (Mathf.Abs(distanceFromStart) >= patrolRange)
            {
                patrolDirection *= -1;
            }
            
            Vector3 movement = new Vector3(patrolDirection * moveSpeed * deltaTime, 0, 0);
            transform.position += movement;
        }

        private void UpdateRandomMovement(float deltaTime)
        {
            randomMoveTimer -= deltaTime;
            
            if (randomMoveTimer <= 0)
            {
                randomMoveDirection = new Vector3(Random.Range(-1f, 1f), 0, 0).normalized;
                randomMoveTimer = Random.Range(1f, 3f);
            }
            
            transform.position += randomMoveDirection * moveSpeed * deltaTime;
        }

        public void UpdateViolationTimer(float deltaTime)
        {
            if (isInViolation)
            {
                violationTimer -= deltaTime;
                if (violationTimer <= 0)
                {
                    if (npcController != null)
                    {
                        npcController.StopViolation();
                    }
                    isInViolation = false;
                    
                    if (autoViolation)
                    {
                        ScheduleNextViolation();
                    }
                }
            }
            
            wasViolatingLastFrame = isInViolation;
        }

        private void UpdateAutoViolation(float deltaTime)
        {
            if (!isInViolation && Time.time >= nextViolationTime)
            {
                StartViolationBehavior();
            }
        }

        private void ScheduleNextViolation()
        {
            float interval = Random.Range(violationMinInterval, violationMaxInterval);
            nextViolationTime = Time.time + interval;
        }

        public bool WasViolatingLastFrame()
        {
            return wasViolatingLastFrame;
        }
    }
}