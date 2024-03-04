using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Managers
{
    public enum GameState
    {
        ready,
        busy,
        levelEnd
    }
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public GameState currentState;
        public int score;
        public int moveCount;
        [SerializeField] private GameObject levelEnd;

        public void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            score = 0;
        }

        public void IncreaseScore(int amount)
        {
            score += amount;
            GridView.Instance.UpdateScoreText(score);
        }
        public async void DecreaseMoveCount(int amount)
        {
            moveCount -= amount;
            GridView.Instance.UpdateMoveText(moveCount);
            if (moveCount <= 0 && GridManager.Instance.destroyList.Count == 0)
            {
                await UniTask.Delay(500);
                HandleLevelEnd();
            }
        }
        public void HandleLevelEnd()
        {
            GridView.Instance.SetNewHighScoreText(score);
            levelEnd.SetActive(true);
            currentState = GameState.levelEnd;
            LevelManager.Instance.UnlockNextLevel();
        }
        public async void ReplayClicked()
        {
            var level = LevelManager.Instance.GetCurrentLevel();
            score = 0;
            levelEnd.SetActive(false);
            GridManager.Instance.SetGrid(level);
            GridView.Instance.SetLevelUI(level);
            await UniTask.Delay(750);
            GridManager.Instance.FindAllMatches();
        }
        
        public void BackToMenuClicked()
        {
            levelEnd.SetActive(false);
            SceneManager.UnloadSceneAsync(1);
            EventManager.Instance.MainMenuActivated();
            EventManager.Instance.PlayButtonClicked(0);
        }
    }
}
