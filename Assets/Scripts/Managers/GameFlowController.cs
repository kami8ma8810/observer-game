using UnityEngine;
using JusticeManGO.Player;
using JusticeManGO.UI;
using JusticeManGO.NPC;
using System.Collections;
using System.Collections.Generic;

namespace JusticeManGO.Managers
{
    public class GameFlowController : MonoBehaviour
    {
        [System.Serializable]
        public class GameStats
        {
            public int Score;
            public int Followers;
            public float FlameGauge;
            public float TimeRemaining;
            public int PhotosTaken;
            public int SuccessfulReports;
            public int FalseReports;
        }

        [System.Serializable]
        public class ReactionOutcome
        {
            public int LikesGained;
            public int FollowersGained;
            public float FlameGaugeChange;
            public bool IsBacklash;
            public string Message;
        }

        private GameManager gameManager;
        private UIManager uiManager;
        private NPCSpawner npcSpawner;
        private ViolationDetectionManager violationDetectionManager;
        private SNSPostEffect snsPostEffect;
        private PhotoCaptureController photoCaptureController;

        private float gameDuration = 180f;
        private float gameTimer = 0f;
        private bool isGameRunning = false;
        private bool isPaused = false;
        private bool isGameEnded = false;

        private GameStats currentStats;
        
        private const int BASE_SCORE_PER_PHOTO = 100;
        private const int BASE_FOLLOWERS_PER_SUCCESS = 10;
        private const float BASE_FLAME_PER_FALSE_REPORT = 15f;
        private const float BASE_FLAME_PER_BACKLASH = 10f;

        private void Awake()
        {
            currentStats = new GameStats();
        }

        public void StartGame()
        {
            InitializeSystems();
            SetupGameplay();
            isGameRunning = true;
            isGameEnded = false;
            gameTimer = gameDuration;
            
            StartCoroutine(GameLoop());
            StartCoroutine(NPCSpawnLoop());
        }

        private void InitializeSystems()
        {
            gameManager = GameManager.Instance;
            gameManager.Initialize();
            
            GameObject uiObject = new GameObject("UIManager");
            uiManager = uiObject.AddComponent<UIManager>();
            uiManager.Initialize();
            
            GameObject spawnerObject = new GameObject("NPCSpawner");
            npcSpawner = spawnerObject.AddComponent<NPCSpawner>();
            
            GameObject detectionObject = new GameObject("ViolationDetectionManager");
            violationDetectionManager = detectionObject.AddComponent<ViolationDetectionManager>();
            
            GameObject snsObject = new GameObject("SNSPostEffect");
            snsPostEffect = snsObject.AddComponent<SNSPostEffect>();
            snsPostEffect.Initialize();
        }

        private void SetupGameplay()
        {
            if (gameManager.Player != null)
            {
                photoCaptureController = gameManager.Player.GetComponent<PhotoCaptureController>();
                if (photoCaptureController == null)
                {
                    photoCaptureController = gameManager.Player.AddComponent<PhotoCaptureController>();
                }
            }
            
            gameManager.SetLevelBounds(new Bounds(Vector3.zero, new Vector3(50f, 10f, 0)));
            
            if (uiManager.GetCameraButton() != null)
            {
                uiManager.GetCameraButton().onClick.AddListener(OnCameraButtonClick);
            }
            
            UpdateUI();
        }

        private IEnumerator GameLoop()
        {
            while (isGameRunning && !isGameEnded)
            {
                if (!isPaused)
                {
                    UpdateGameTimer(Time.deltaTime);
                    UpdateUI();
                    
                    if (CheckGameOver())
                    {
                        EndGame();
                    }
                }
                
                yield return null;
            }
        }

        private IEnumerator NPCSpawnLoop()
        {
            yield return new WaitForSeconds(2f);
            
            while (isGameRunning && !isGameEnded)
            {
                if (!isPaused && npcSpawner.GetActiveNPCCount() < 5)
                {
                    SpawnNPCWave(Random.Range(1, 3));
                }
                
                yield return new WaitForSeconds(Random.Range(5f, 10f));
            }
        }

        private void OnCameraButtonClick()
        {
            if (photoCaptureController != null && photoCaptureController.CanCapture())
            {
                var captureResult = photoCaptureController.TriggerCapture();
                ProcessPhotoCapture(captureResult);
            }
        }

        public void ProcessPhotoCapture(PhotoCaptureController.CaptureResult captureResult, bool forceValidReport = false)
        {
            currentStats.PhotosTaken++;
            
            if (!captureResult.Success)
            {
                uiManager.ShowNotification("撮影に失敗しました", 2f);
                return;
            }
            
            var detectionResult = violationDetectionManager.ProcessCapture(captureResult);
            
            if (detectionResult.IsValidReport || forceValidReport)
            {
                ProcessSuccessfulReport(detectionResult.DetectedViolations);
            }
            else
            {
                ProcessFalseReport();
            }
        }

        private void ProcessSuccessfulReport(List<ViolationDetectionManager.ViolationInfo> violations)
        {
            currentStats.SuccessfulReports++;
            
            string npcId = violations.Count > 0 ? violations[0].NPCId : "unknown";
            var outcome = CalculateReactionOutcome(npcId, true);
            
            ApplyReactionOutcome(outcome);
            
            var captureResultData = new UIManager.CaptureResultData
            {
                Success = true,
                Message = outcome.Message,
                LikesGained = outcome.LikesGained,
                FollowersGained = outcome.FollowersGained,
                Backlash = outcome.IsBacklash
            };
            
            uiManager.ShowCaptureResult(captureResultData);
            
            var postConfig = new SNSPostEffect.PostConfiguration
            {
                Message = outcome.Message,
                Tags = snsPostEffect.GetTrendingHashtags()
            };
            snsPostEffect.PlayPostAnimation(postConfig, 2f);
        }

        private void ProcessFalseReport()
        {
            currentStats.FalseReports++;
            
            var outcome = CalculateReactionOutcome("false_report", false);
            ApplyReactionOutcome(outcome);
            
            var captureResultData = new UIManager.CaptureResultData
            {
                Success = false,
                Message = "誤報です！炎上リスク上昇",
                LikesGained = outcome.LikesGained,
                FollowersGained = outcome.FollowersGained,
                Backlash = true
            };
            
            uiManager.ShowCaptureResult(captureResultData);
        }

        public ReactionOutcome CalculateReactionOutcome(string npcId, bool isValidReport)
        {
            var outcome = new ReactionOutcome();
            
            if (isValidReport)
            {
                var reactionData = violationDetectionManager.GetReactionForViolation(npcId, "");
                
                outcome.LikesGained = Random.Range((int)reactionData.LikeRange.x, (int)reactionData.LikeRange.y);
                outcome.Message = reactionData.Message;
                
                if (Random.value < reactionData.BacklashProbability)
                {
                    outcome.IsBacklash = true;
                    outcome.FlameGaugeChange = BASE_FLAME_PER_BACKLASH;
                    outcome.FollowersGained = -Random.Range(5, 15);
                }
                else
                {
                    outcome.FollowersGained = Random.Range(5, BASE_FOLLOWERS_PER_SUCCESS);
                    outcome.FlameGaugeChange = -5f;
                }
            }
            else
            {
                outcome.LikesGained = -Random.Range(10, 30);
                outcome.FollowersGained = -Random.Range(10, 20);
                outcome.FlameGaugeChange = BASE_FLAME_PER_FALSE_REPORT;
                outcome.IsBacklash = true;
                outcome.Message = "誤った通報";
            }
            
            return outcome;
        }

        private void ApplyReactionOutcome(ReactionOutcome outcome)
        {
            AddScore(BASE_SCORE_PER_PHOTO + outcome.LikesGained);
            AddFollowers(outcome.FollowersGained);
            gameManager.IncreaseFlameGauge(outcome.FlameGaugeChange);
        }

        public void UpdateGameTimer(float deltaTime)
        {
            gameTimer -= deltaTime;
            if (gameTimer <= 0)
            {
                gameTimer = 0;
                isGameEnded = true;
            }
        }

        public bool CheckGameOver()
        {
            return gameTimer <= 0 || gameManager.IsAccountSuspended();
        }

        private void EndGame()
        {
            isGameRunning = false;
            isGameEnded = true;
            
            bool suspended = gameManager.IsAccountSuspended();
            uiManager.ShowGameOver(suspended, currentStats.Score, currentStats.Followers);
            
            Time.timeScale = 0f;
        }

        private void UpdateUI()
        {
            uiManager.UpdateScore(gameManager.GetCurrentScore());
            uiManager.UpdateFollowers(gameManager.GetFollowerCount());
            uiManager.UpdateFlameGauge(gameManager.GetFlameGauge());
        }

        public int SpawnNPCWave(int count)
        {
            int spawned = 0;
            for (int i = 0; i < count; i++)
            {
                float xPos = Random.Range(-20f, 20f);
                Vector3 spawnPos = new Vector3(xPos, 0, 0);
                
                GameObject npc = npcSpawner.SpawnRandomNPC(spawnPos);
                if (npc != null)
                {
                    spawned++;
                }
            }
            return spawned;
        }

        public void PauseGame()
        {
            isPaused = true;
            Time.timeScale = 0f;
        }

        public void ResumeGame()
        {
            isPaused = false;
            Time.timeScale = 1f;
        }

        public void AddScore(int points)
        {
            currentStats.Score += points;
            gameManager.AddScore(points);
        }

        public void AddFollowers(int count)
        {
            currentStats.Followers += count;
            if (count > 0)
            {
                gameManager.AddFollowers(count);
            }
            else
            {
                gameManager.RemoveFollowers(-count);
            }
        }

        public void SetFlameGauge(float value)
        {
            gameManager.IncreaseFlameGauge(value - gameManager.GetFlameGauge());
        }

        public void SetGameDuration(float duration)
        {
            gameDuration = duration;
            gameTimer = duration;
        }

        public GameStats GetGameStats()
        {
            currentStats.Score = gameManager.GetCurrentScore();
            currentStats.Followers = gameManager.GetFollowerCount();
            currentStats.FlameGauge = gameManager.GetFlameGauge();
            currentStats.TimeRemaining = gameTimer;
            return currentStats;
        }

        public int GetCurrentScore() => gameManager.GetCurrentScore();
        public float GetFlameGauge() => gameManager.GetFlameGauge();
        public bool IsGameEnded() => isGameEnded;
        public bool IsPaused() => isPaused;
        
        public GameManager GetGameManager() => gameManager;
        public UIManager GetUIManager() => uiManager;
        public NPCSpawner GetNPCSpawner() => npcSpawner;
        public ViolationDetectionManager GetViolationDetectionManager() => violationDetectionManager;
    }
}