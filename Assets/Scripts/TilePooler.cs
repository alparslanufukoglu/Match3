using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TilePooler : MonoBehaviour
{
    public static TilePooler Instance;
    public readonly List<TileView> TilePool = new();
    public GameObject _tileViewPrefab;

    private void Awake()
    {
        Instance = this;
    }
    
    public void InitializeTilePool(int tileCount)
    {
        for (var i = 0; i < tileCount; i++)
        {
            var tileObject = Instantiate(_tileViewPrefab, Vector3.zero, Quaternion.identity);
            tileObject.transform.parent = transform;
            var tileView = tileObject.GetComponent<TileView>();
            TilePool.Add(tileView);
        }
    }
    public TileView GetPooledTile()
    {
        return TilePool.FirstOrDefault(tileView => tileView.IsEmpty());
    }

    public void ResetPool()
    {
        foreach (var tile in TilePool)
        {
            tile.DeactivateTile();
        }
    }
}
