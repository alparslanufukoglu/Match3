using UnityEngine;

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
            Debug.Log("Score is " + score);
        }

        public void DecreaseMoveCount(int amount)
        {
            moveCount -= amount;
            GridView.Instance.UpdateMoveText(moveCount);
            Debug.Log("MoveCount is " + moveCount);
        }
        
    }
}
