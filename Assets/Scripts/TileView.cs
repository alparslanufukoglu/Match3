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
        gameObject.name = GiveName(tile.posX,tile.posY);
        SetSprite();
        AnimateTile();
    }
    public void UpdatePosition(int x, int y)
    {
        tile.posX = x;
        tile.posY = y;
        transform.DOMove(new Vector3(tile.posX, tile.posY), 0.2f).SetEase(Ease.InOutBack);
    }

    public void UpdateTile(TileView tileView, TileType tileType, Transform parent)
    {
        var position = tileView.transform.position;
        tile = new Tile((int)position.x, (int)position.y, tileType);
        transform.SetParent(parent);
        gameObject.name = GiveName(tile.posX,tile.posY);
        SetSprite();
        AnimateTile();
    }
    private void AnimateTile()
    {
        transform.position = new Vector3(tile.posX, tile.posY + 8);
        ActivateTile();
        transform.DOMove(new Vector2(tile.posX, tile.posY), 0.5f).SetEase(Ease.InOutBack);
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
    public String GiveName(int x, int y)
    {
        return "(" + x + "," + y + ")" + GetTileType();
    }
    
    public bool IsBooster()
    {
        return tile.tileType == TileType.RowBooster || tile.tileType == TileType.ColumnBooster;
    }
}
