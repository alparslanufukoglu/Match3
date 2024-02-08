using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum TileType
{
    None = -1,
    Blue = 0,
    Green = 1,
    Red = 2,
    Pink = 3,
    Purple = 4,
    Yellow = 5,
    RowBooster = 6,
    ColumnBooster = 7
}

public class TileView : MonoBehaviour
{
    public Sprite[] sprites;
    public List<Tile> Tiles = new List<Tile>();
    
    void Start()
    {
        
    }
    void Update()
    {
        
    }

    public Tile InitTile(TileType tileType,Transform transform)
    {
        Tile tile = new Tile();
        tile.tileType = (int)tileType;
        tile.tileSprite = sprites[(int)tileType];
        tile.posX = (int)transform.position.x;
        tile.posY = (int)transform.position.y;
        return tile;
    }
    
    public bool IsEmpty (Tile tile)
    {
        return tile == null;
    }
}
