using System;
using DG.Tweening;
using UnityEngine;
public enum TileType
{
    None = 0,
    Blue = 1,
    Green = 2,
    Red = 3,
    Pink = 4,
    Purple = 5,
    Yellow = 6,
    RowBooster = 7,
    ColumnBooster = 8
}

public class TileView : MonoBehaviour
{
    public Sprite[] sprites;
    public SpriteRenderer spriteRenderer;
    public Tile tile;
    private Vector2 fromPosition;
    private Vector2 target;

    private void Awake()
    {
        DeactivateTile();
    }

    public void SetTile(int x, int y, char tileType, Transform parent)
    {
        tile = new Tile(x, y, GetTileType(tileType));
        transform.SetParent(parent);
        gameObject.name = "(" + x + "," + y + ")" + GetTileType();
        SetSprite();
        AnimateTile();
    }

    private void AnimateTile()
    {
        transform.position = new Vector3(tile.posX, tile.posY + 8);
        ActivateTile();
        transform.DOMove(new Vector2(tile.posX, tile.posY), 0.8f).SetEase(Ease.InOutBack);
    }

    private void SetSprite()
    { 
        spriteRenderer.sprite = sprites[(int) tile.tileType];
    }

    private TileType GetTileType(char tileType)
    {
        return tileType switch
        {
            'h' => TileType.Blue,
            'l' => TileType.Green,
            'a' => TileType.Pink,
            'r' => TileType.Purple,
            's' => TileType.Red,
            'c' => TileType.Yellow,
            'b' => TileType.RowBooster,
            'o' => TileType.ColumnBooster,
            _ => TileType.None
        };
    }

    public bool IsEmpty ()
    {
        return tile == null;
    }
    
    public TileType GetTileType()
    {
        return tile.tileType;
    }

    public void DeactivateTile()
    {
        tile = null;
        gameObject.SetActive(false);
    }
    
    public void ActivateTile()
    {
        gameObject.SetActive(true);
    }
}
