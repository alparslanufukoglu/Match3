using System;
using DG.Tweening;
using UnityEngine;
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
    public SpriteRenderer spriteRenderer;
    private Tile _tile;
    private Vector2 fromPosition;
    private Vector2 target;

    private void Awake()
    {
        DeactivateTile();
    }

    public void SetTile(int x, int y, char tileType, Transform parent)
    {
        _tile = new Tile(x, y, GetTileType(tileType));
        transform.SetParent(parent);
        gameObject.name = "(" + x + "," + y + ")" + GetTileType();
        SetSprite();
        AnimateTile();
    }

    private void AnimateTile()
    {
        transform.position = new Vector3(_tile.posX, _tile.posY + 8);
        ActivateTile();
        transform.DOMove(new Vector2(_tile.posX, _tile.posY), 0.8f).SetEase(Ease.InOutBack);
    }

    private void SetSprite()
    { 
        spriteRenderer.sprite = sprites[(int) _tile.tileType];
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
        return _tile == null;
    }
    
    public TileType GetTileType()
    {
        return _tile.tileType;
    }

    public void DeactivateTile()
    {
        _tile = null;
        gameObject.SetActive(false);
    }
    
    public void ActivateTile()
    {
        gameObject.SetActive(true);
    }
}
