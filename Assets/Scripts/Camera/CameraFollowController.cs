using UnityEngine;

namespace JusticeManGO.Camera
{
    public class CameraFollowController : MonoBehaviour
    {
        private Transform target;
        private Vector3 followOffset = new Vector3(0, 0, -10);
        private Bounds cameraBounds;
        private bool useBounds = false;
        private bool useSmoothing = false;
        private float smoothingSpeed = 0.1f;

        public Transform Target => target;

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }

        public void SetFollowOffset(Vector3 offset)
        {
            followOffset = offset;
        }

        public void SetBounds(Bounds bounds)
        {
            cameraBounds = bounds;
            useBounds = true;
        }

        public void EnableSmoothing(bool enable, float speed)
        {
            useSmoothing = enable;
            smoothingSpeed = speed;
        }

        public void UpdateCameraPosition()
        {
            if (target == null) return;

            Vector3 desiredPosition = new Vector3(
                target.position.x + followOffset.x,
                followOffset.y,
                followOffset.z
            );

            if (useBounds)
            {
                float halfWidth = UnityEngine.Camera.main.orthographicSize * UnityEngine.Camera.main.aspect;
                float halfHeight = UnityEngine.Camera.main.orthographicSize;

                desiredPosition.x = Mathf.Clamp(
                    desiredPosition.x,
                    cameraBounds.min.x + halfWidth,
                    cameraBounds.max.x - halfWidth
                );
            }

            if (useSmoothing)
            {
                transform.position = Vector3.Lerp(
                    transform.position,
                    desiredPosition,
                    smoothingSpeed
                );
            }
            else
            {
                transform.position = desiredPosition;
            }
        }

        private void LateUpdate()
        {
            UpdateCameraPosition();
        }
    }
}