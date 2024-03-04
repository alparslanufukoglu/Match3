using UnityEngine;
public class Level
{
    public int levelNumber;
    public int moveCount;
    public int gridWidth;
    public int gridHeight;
    public string gridLayout;
    public Level() { }
    
    public Level(int levelNumber, int moveCount, int gridWidth, int gridHeight, string gridLayout)
    {
        this.levelNumber = levelNumber;
        this.moveCount = moveCount;
        this.gridWidth = gridWidth;
        this.gridHeight = gridHeight;
        this.gridLayout = gridLayout;

    }
    public bool IsLocked()
    {
        return !PlayerPrefs.HasKey("Level" + levelNumber);
    }

    public int GetScore()
    {
        return PlayerPrefs.GetInt("Level" + levelNumber, 0);
    }
}
