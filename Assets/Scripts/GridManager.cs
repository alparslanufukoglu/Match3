using UnityEngine;

public class GridManager : MonoBehaviour
{
    private int boardWidth;
    private int boardHeight;
    private TileView[,] _tiles;
    void Start()
    {
        SetGrid(LevelManager.Instance.GetCurrentLevel());
    }
    private void SetGrid(Level level)
    {
        boardWidth = level.gridWidth;
        boardHeight = level.gridHeight;
        if (_tiles == null)
        {
            _tiles = new TileView[boardWidth, boardHeight];
            TilePooler.Instance.InitializeTilePool(boardWidth * boardHeight);
        }
        else
        {
            for (var i = 0; i < boardHeight; i++)
            {
                for (var j = 0; j < boardWidth; j++)
                {
                    _tiles[j, i] = null;
                }
            }
            TilePooler.Instance.ResetPool();
        }
        
        for (var i = 0; i < boardHeight; i++)
        {
            for (var j = 0; j < boardWidth; j++)
            {
                var tile = TilePooler.Instance.GetPooledTile();
                tile.SetTile(j,i,level.gridLayout[j+ j*i], transform);
                _tiles[j, i] = tile;
            }
        }
    }
}
