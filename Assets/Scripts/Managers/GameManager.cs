using UnityEngine;
using JusticeManGO.Player;
using JusticeManGO.Camera;

namespace JusticeManGO.Managers
{
    public class GameManager : MonoBehaviour
    {
        private GameObject player;
        private GameObject mainCamera;
        private int currentScore = 0;
        private int followerCount = 0;
        private float flameGauge = 0f;

        public GameObject Player => player;
        public GameObject MainCamera => mainCamera;

        private static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<GameManager>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject("GameManager");
                        instance = go.AddComponent<GameManager>();
                    }
                }
                return instance;
            }
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void Initialize()
        {
            CreatePlayer();
            CreateCamera();
            SetupCameraFollow();
        }

        private void CreatePlayer()
        {
            player = new GameObject("Player");
            player.AddComponent<PlayerMovementController>();
            player.transform.position = Vector3.zero;
        }

        private void CreateCamera()
        {
            mainCamera = new GameObject("MainCamera");
            mainCamera.AddComponent<UnityEngine.Camera>();
            mainCamera.AddComponent<CameraFollowController>();
            mainCamera.tag = "MainCamera";
            
            var camera = mainCamera.GetComponent<UnityEngine.Camera>();
            camera.orthographic = true;
            camera.orthographicSize = 5f;
            camera.transform.position = new Vector3(0, 0, -10);
        }

        private void SetupCameraFollow()
        {
            var cameraController = mainCamera.GetComponent<CameraFollowController>();
            cameraController.SetTarget(player.transform);
            cameraController.SetFollowOffset(new Vector3(0, 0, -10));
        }

        public void SetLevelBounds(Bounds bounds)
        {
            var cameraController = mainCamera.GetComponent<CameraFollowController>();
            cameraController.SetBounds(bounds);

            var playerController = player.GetComponent<PlayerMovementController>();
            playerController.SetMovementBounds(bounds.min.x, bounds.max.x);
        }

        public int GetCurrentScore()
        {
            return currentScore;
        }

        public void AddScore(int points)
        {
            currentScore += points;
        }

        public int GetFollowerCount()
        {
            return followerCount;
        }

        public void AddFollowers(int count)
        {
            followerCount += count;
        }

        public void RemoveFollowers(int count)
        {
            followerCount = Mathf.Max(0, followerCount - count);
        }

        public float GetFlameGauge()
        {
            return flameGauge;
        }

        public void IncreaseFlameGauge(float amount)
        {
            flameGauge = Mathf.Clamp(flameGauge + amount, 0f, 100f);
        }

        public void DecreaseFlameGauge(float amount)
        {
            flameGauge = Mathf.Clamp(flameGauge - amount, 0f, 100f);
        }

        public bool IsAccountSuspended()
        {
            return flameGauge >= 100f;
        }
    }
}