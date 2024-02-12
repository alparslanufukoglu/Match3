using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridWidth;
    public int gridHeight;
    public TileView[,] Tiles;
    [SerializeField] private List<TileView> _destroyList = new ();
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
        FindMatches(new List<Tile> {tile1,tile2});
    }
    
    private void FindMatches(IReadOnlyCollection<Tile> list)
    {
        List<int> horizontalCheck = new ();
        List<int> verticalCheck = new ();
        foreach (var tile in list.Where(tile => !horizontalCheck.Contains(tile.posY)))
        {
            horizontalCheck.Add(tile.posY);
        }
        foreach (var tile in list.Where(tile => !verticalCheck.Contains(tile.posX)))
        {
            verticalCheck.Add(tile.posX);
        }
        foreach (var val in horizontalCheck)
        {
            FindHorizontalMatches(val);
        }
        foreach (var val in verticalCheck)
        {
            FindVerticalMatches(val);
        }
        DestroyMatches();
    }
    private void FindHorizontalMatches(int posY)
    {
        var horizontalMatches = new List<TileView>();
        var targetType = TileType.None;
        for (var i = 0; i < gridWidth; i++)
        {
            TileView checkTile = Tiles[i, posY];
            if (Tiles[i, posY] != null)
            {
                if (targetType == TileType.None)
                {
                    horizontalMatches.Add(checkTile);
                    targetType = checkTile.tile.tileType;
                }
                else if (checkTile.tile.tileType == targetType)
                {
                    if (!horizontalMatches.Contains(checkTile))
                    {
                        horizontalMatches.Add(checkTile);
                    }
                    if (i == gridWidth - 1)
                    {
                        if (horizontalMatches.Count >= 3)
                        {
                            _destroyList.AddRange(horizontalMatches);
                        }
                    }
                }
                else
                {
                    if (horizontalMatches.Count >= 3)
                    {
                        _destroyList.AddRange(horizontalMatches);
                    }
                    horizontalMatches.Clear();
                    horizontalMatches.Add(checkTile);
                    targetType = checkTile.tile.tileType;
                }
            }
        }
    }
    private void FindVerticalMatches(int posX)
    {
        var verticalMatches = new List<TileView>();
        var targetType = TileType.None;
        for (int i = 0; i < gridHeight; i++)
        {
            TileView checkTile = Tiles[posX, i];
            if (Tiles[posX, i] != null)
            {
                if (targetType == TileType.None)
                {
                    verticalMatches.Add(checkTile);
                    targetType = checkTile.tile.tileType;
                }
                else if (checkTile.tile.tileType == targetType)
                {
                    if (!verticalMatches.Contains(checkTile))
                    {
                        verticalMatches.Add(checkTile);
                    }

                    if (i == gridHeight - 1)
                    {
                        if (verticalMatches.Count >= 3)
                        {
                            _destroyList.AddRange(verticalMatches);
                        }
                    }
                }
                else
                {
                    if (verticalMatches.Count >= 3)
                    {
                        _destroyList.AddRange(verticalMatches);
                    }
                    verticalMatches.Clear();
                    verticalMatches.Add(checkTile);
                    targetType = checkTile.tile.tileType;
                }
            }
        }
    }
    private async void DestroyMatches()
    {
        await UniTask.Delay(400);
        foreach (var tile in _destroyList.Where(tile => tile.gameObject != null))
        {
            Tiles[tile.tile.posX, tile.tile.posY].DeactivateTile();
        }
        await UniTask.Delay(300);
        _destroyList.Clear();
    }
}
