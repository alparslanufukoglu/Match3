using System.Security.Cryptography.X509Certificates;
using UnityEditor.U2D.Sprites;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridWidth;
    public int gridHeight;
    public TileView[,] Tiles;
    public static GridManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SetGrid(LevelManager.Instance.GetCurrentLevel());
    }
    private void SetGrid(Level level)
    {
        gridWidth = level.gridWidth;
        gridHeight = level.gridHeight;
        if (Tiles == null)
        {
            Tiles = new TileView[gridWidth, gridHeight];
            TilePooler.Instance.InitializeTilePool(gridWidth * gridHeight);
        }
        else
        {
            for (var i = 0; i < gridHeight; i++)
            {
                for (var j = 0; j < gridWidth; j++)
                {
                    Tiles[j, i] = null;
                }
            }
            TilePooler.Instance.ResetPool();
        }
        
        for (var i = 0; i < gridHeight; i++)
        {
            for (var j = 0; j < gridWidth; j++)
            {
                var tile = TilePooler.Instance.GetPooledTile();
                tile.SetTile(j,i,level.gridLayout[j+ j*i], transform);
                Tiles[j, i] = tile;
            }
        }
    }
    public void SwapArrayPosition(Tile tile1, Tile tile2)
    {
        (Tiles[tile1.posX, tile1.posY], Tiles[tile2.posX,tile2.posY]) = (Tiles[tile2.posX,tile2.posY], Tiles[tile1.posX, tile1.posY]);
    }
    
}
