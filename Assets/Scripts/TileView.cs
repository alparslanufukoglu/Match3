using DG.Tweening;
using UnityEngine;
public enum TileType
{
    None = 0,
    Blue = 1,
    Green = 2,
    Red = 3,
    Pink = 4,
    Purple  = 5,
    Yellow = 6,
    RowBooster = 7,
    ColumnBooster = 8
}
public class TileView : MonoBehaviour
{
    public Sprite[] sprites;
    public SpriteRenderer spriteRenderer;
    public Tile tile;
    private Vector2 _fromPosition;
    private Vector2 _target;

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
            'r' => TileType.Red,
            's' => TileType.Purple,
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

    private TileType GetTileType()
    {
        return tile.tileType;
    }
    public void DeactivateTile()
    {
        tile = null;
        gameObject.SetActive(false);
    }

    private void ActivateTile()
    {
        gameObject.SetActive(true);
    }
    public string GiveName(int x, int y)
    {
        return "(" + x + "," + y + ")" + GetTileType();
    }
    public bool IsBooster()
    {
        return tile.tileType == TileType.RowBooster || tile.tileType == TileType.ColumnBooster;
    }
    
    public int GiveBoosterTypeIndex()
    {
        return Random.Range(sprites.Length - 2, sprites.Length);
    }
    
    public int GiveTileTypeIndex(int boardSize)
    {
        return boardSize switch
        {
            < 35 => Random.Range(1, sprites.Length - 5),
            > 35 and < 45 => Random.Range(1, sprites.Length - 4),
            _ => Random.Range(1, sprites.Length - 3)
        };
    }
    
    public Color GetColor()
    {
        switch (tile.tileType)
        {
            case TileType.None:
                return Color.white;
            case TileType.Blue:
                return new Color32(114, 140, 233,255);
            case TileType.Green:
                return new Color32(98, 190, 116, 140);
            case TileType.Pink:
                return new Color32(255, 136, 204, 255);
            case TileType.Purple:
                return new Color32(114, 90, 228, 255);
            case TileType.Red:
                return new Color32(218, 82, 100, 255);
            case TileType.Yellow:
                return new Color32(250, 184, 58, 255);
            case TileType.RowBooster:
                return new Color32(255, 134, 0, 255);
            case TileType.ColumnBooster:
                return new Color32(252, 0, 0, 255);
            default:
                return Color.white;
        }
    }
}
