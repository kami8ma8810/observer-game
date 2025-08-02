using UnityEngine;

namespace JusticeManGO.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        private float moveSpeed = 5f;
        private float minX = float.MinValue;
        private float maxX = float.MaxValue;
        private bool isMoving = false;
        private int facingDirection = 1;

        public float MoveSpeed => moveSpeed;
        public bool IsMoving => isMoving;
        public int FacingDirection => facingDirection;

        public float GetMinBound() => minX;
        public float GetMaxBound() => maxX;

        public void SetMoveSpeed(float speed)
        {
            moveSpeed = speed;
        }

        public void SetMovementBounds(float min, float max)
        {
            minX = min;
            maxX = max;
        }

        public void Move(float horizontalInput, float deltaTime)
        {
            if (Mathf.Abs(horizontalInput) < 0.01f)
            {
                isMoving = false;
                return;
            }

            isMoving = true;

            if (horizontalInput > 0)
            {
                facingDirection = 1;
            }
            else if (horizontalInput < 0)
            {
                facingDirection = -1;
            }

            float movement = horizontalInput * moveSpeed * deltaTime;
            Vector3 newPosition = transform.position + new Vector3(movement, 0, 0);
            
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            
            transform.position = newPosition;
        }

        private void Update()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            Move(horizontalInput, Time.deltaTime);
        }
    }
}