using UnityEngine;
using JusticeManGO.Managers;

namespace JusticeManGO.Utils
{
    public class SceneInitializer : MonoBehaviour
    {
        [SerializeField] private Bounds levelBounds = new Bounds(Vector3.zero, new Vector3(30f, 10f, 0));
        [SerializeField] private float playerMoveSpeed = 5f;

        private void Start()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            var gameManager = GameManager.Instance;
            gameManager.Initialize();
            gameManager.SetLevelBounds(levelBounds);

            var playerController = gameManager.Player.GetComponent<Player.PlayerMovementController>();
            playerController.SetMoveSpeed(playerMoveSpeed);

            Debug.Log("Game initialized successfully");
        }
    }
}