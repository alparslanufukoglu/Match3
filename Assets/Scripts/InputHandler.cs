using System;
using DG.Tweening;
using UnityEngine;

public enum Direction
{
    None,
    Right,
    Left,
    Up,
    Down
}

public class InputHandler : MonoBehaviour
{
    private GameObject _selected;
    private readonly float _offSet = 10f;
    private Vector2 _endTouchPoint;
    private Direction _swipeDirection;
    
    private void OnMouseDown()
    {
        if (Camera.main != null)
        {
            var hit = Physics2D.Raycast(
                Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _offSet)),
                Vector2.zero);
            _selected = hit.collider != null ? hit.collider.gameObject : null;
        }
    }
    private void OnMouseUp()
    {
        if (Camera.main != null && _selected != null)
        { 
            _endTouchPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0,0,_offSet));
            SwapTiles(SetDirection());
        }
    }
    
    private Direction SetDirection()
    {
        const float swipeThreshold = 0.3f;
        if (_selected == null) return Direction.None;
        Vector2 startTouchPoint = _selected.transform.position;
        float swipeDistance = Vector2.Distance(startTouchPoint, _endTouchPoint);
        if (swipeDistance > swipeThreshold)
        {
            float horizontalMove = _endTouchPoint.x - startTouchPoint.x;
            float verticalMove = _endTouchPoint.y - startTouchPoint.y;
            if (Math.Abs(horizontalMove) > Math.Abs(verticalMove))
            {
                _swipeDirection = (horizontalMove > 0) ? Direction.Right : Direction.Left;
            }
            else
            {
                _swipeDirection = (verticalMove > 0) ? Direction.Up : Direction.Down;
            }
        }
        return _swipeDirection;
    }
    private void SwapTiles(Direction direction)
    {
        if (direction == Direction.None) return;
        var selectedTile = _selected.GetComponent<TileView>().tile;
        TileView targetTile;
        switch (direction)
        {
            case Direction.Right when selectedTile.posX < GridManager.Instance.gridWidth - 1:
                targetTile = GridManager.Instance.Tiles[selectedTile.posX + 1, selectedTile.posY];
                break;
            case Direction.Left when selectedTile.posX > 0:
                targetTile = GridManager.Instance.Tiles[selectedTile.posX - 1, selectedTile.posY];
                break;
            case Direction.Up when selectedTile.posY < GridManager.Instance.gridHeight - 1:
                targetTile = GridManager.Instance.Tiles[selectedTile.posX, selectedTile.posY + 1];
                break;
            case Direction.Down when selectedTile.posY > 0:
                targetTile = GridManager.Instance.Tiles[selectedTile.posX, selectedTile.posY - 1];
                break;
            default:
                return;
        }
        var position1 = _selected.transform.position;
        var position2 = targetTile.transform.position;
        GridManager.Instance.SwapArrayPosition(selectedTile, targetTile.tile);
        (position1, position2) = (position2, position1);
        (selectedTile.posX, targetTile.tile.posX) = (targetTile.tile.posX,selectedTile.posX);
        (selectedTile.posY, targetTile.tile.posY) = (targetTile.tile.posY,selectedTile.posY);
        _selected.transform.DOMove(position1, 0.2f);
        targetTile.transform.DOMove(position2, 0.2f);
        _selected = null;
    }
}
