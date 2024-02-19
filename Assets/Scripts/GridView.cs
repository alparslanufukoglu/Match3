using System;
using Managers;
using TMPro;
using UnityEngine;

public class GridView : MonoBehaviour
{
    public static GridView Instance;
    [SerializeField] private TextMeshProUGUI levelNumber;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI moveCountText;
    private Level _level;

    public void Awake()
    {
        Instance = this;
    }
    public void SetLevelUI(Level level) 
    {
            _level = level;
            GameManager.Instance.moveCount = level.moveCount;
            levelNumber.text = "LEVEL " + _level.levelNumber;
            moveCountText.text = "Moves\n" + GameManager.Instance.moveCount;
            scoreText.text = "Score\n" + GameManager.Instance.score;
            
    }
    public void UpdateScoreText(int score)
    {
        scoreText.text = "Score\n" + score;
    }

    public void UpdateMoveText(int moves)
    {
        moveCountText.text = "Moves\n" + moves;
    }
}
