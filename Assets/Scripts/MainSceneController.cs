using UnityEngine;
using JusticeManGO.Managers;

namespace JusticeManGO
{
    public class MainSceneController : MonoBehaviour
    {
        [Header("Game Configuration")]
        [SerializeField] private float gameDuration = 180f;
        [SerializeField] private int maxNPCs = 8;
        [SerializeField] private float playerMoveSpeed = 5f;
        [SerializeField] private float captureRange = 5f;
        [SerializeField] private float captureCooldown = 1f;
        
        [Header("Level Bounds")]
        [SerializeField] private Vector3 levelCenter = Vector3.zero;
        [SerializeField] private Vector3 levelSize = new Vector3(50f, 10f, 0f);
        
        private GameFlowController gameFlowController;
        
        private void Start()
        {
            InitializeGame();
        }
        
        private void InitializeGame()
        {
            GameObject flowControllerObject = new GameObject("GameFlowController");
            gameFlowController = flowControllerObject.AddComponent<GameFlowController>();
            
            ConfigureGame();
            
            gameFlowController.StartGame();
            
            Debug.Log("正義マンGO - ゲーム開始！");
        }
        
        private void ConfigureGame()
        {
            if (gameDuration > 0)
            {
                gameFlowController.SetGameDuration(gameDuration);
            }
            
            StartCoroutine(ConfigureSystemsAfterInit());
        }
        
        private System.Collections.IEnumerator ConfigureSystemsAfterInit()
        {
            yield return null;
            
            var gameManager = gameFlowController.GetGameManager();
            if (gameManager != null)
            {
                gameManager.SetLevelBounds(new Bounds(levelCenter, levelSize));
                
                var player = gameManager.Player;
                if (player != null)
                {
                    var playerMovement = player.GetComponent<JusticeManGO.Player.PlayerMovementController>();
                    if (playerMovement != null)
                    {
                        playerMovement.SetMoveSpeed(playerMoveSpeed);
                    }
                    
                    var photoCaptureController = player.GetComponent<JusticeManGO.Player.PhotoCaptureController>();
                    if (photoCaptureController != null)
                    {
                        photoCaptureController.SetCaptureRange(captureRange);
                        photoCaptureController.SetCooldownTime(captureCooldown);
                    }
                }
            }
            
            var npcSpawner = gameFlowController.GetNPCSpawner();
            if (npcSpawner != null)
            {
                npcSpawner.SetMaxNPCs(maxNPCs);
            }
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
            
            if (Input.GetKeyDown(KeyCode.R) && gameFlowController.IsGameEnded())
            {
                RestartGame();
            }
        }
        
        private void TogglePause()
        {
            if (gameFlowController != null)
            {
                if (gameFlowController.IsPaused())
                {
                    gameFlowController.ResumeGame();
                }
                else
                {
                    gameFlowController.PauseGame();
                }
            }
        }
        
        private void RestartGame()
        {
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
            );
        }
    }
}