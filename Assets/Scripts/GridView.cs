using Managers;
using TMPro;
using UnityEngine;

public class GridView : MonoBehaviour
{
    public static GridView Instance;
    [SerializeField] private TextMeshProUGUI levelNumber;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI moveCountText;
    [SerializeField] private TextMeshProUGUI highScoreText;
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

    public void SetNewHighScoreText(int score)
    {
        var highScore = PlayerPrefs.GetInt("Level" + _level.levelNumber, 0);
        if (score > highScore)
        {
            PlayerPrefs.SetInt("Level" + _level.levelNumber, score);
            highScoreText.text = "NEW HIGH SCORE" + "\n\n" + score;
        }
        else
        {
            highScoreText.text = "SCORE" + "\n\n" + score;
        }
    }
}
