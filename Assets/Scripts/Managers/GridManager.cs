using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;
namespace Managers
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance;
        public int gridWidth;
        public int gridHeight;
        public TileView[,] Tiles;
        public List<TileView> destroyList = new ();
        [SerializeField] private GameObject destroyEffectPrefab;
        [SerializeField] private List<GameObject> particlePool = new ();
        private const int DestroyThreshold = 3;
        private const int TileScore = 50;
        public void Awake()
        {
            Instance = this;
        }
        public void SetGrid(Level level)
        {
            gridWidth = level.gridWidth;
            gridHeight = level.gridHeight;
            int rowNumber = 0;
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
                    tile.SetTile(j,i,level.gridLayout[j + rowNumber], transform);
                    Tiles[j, i] = tile;
                }
                rowNumber += gridWidth;
            }
            InitializedParticlePool();
        }
        private void InitializedParticlePool()
        {
            const int poolSize = 20;
            for (int i = 0; i < poolSize; i++)
            {
                var particleObject = Instantiate(destroyEffectPrefab, transform);
                particleObject.SetActive(false);
                particlePool.Add(particleObject);
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
            if (destroyList.Count >= DestroyThreshold)
            {
                DestroyMatches();
            }
            else
            {
                GameManager.Instance.currentState = GameState.ready;
            }
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
                                destroyList.AddRange(horizontalMatches);
                            }
                        }
                    }
                    else
                    {
                        if (horizontalMatches.Count >= 3)
                        {
                            destroyList.AddRange(horizontalMatches);
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
                                destroyList.AddRange(verticalMatches);
                            }
                        }
                    }
                    else
                    {
                        if (verticalMatches.Count >= 3)
                        {
                            destroyList.AddRange(verticalMatches);
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
            await UniTask.Delay(200);
            foreach (var tile in destroyList.Where(tile =>!tile.IsEmpty()))
            {
                ActivateParticle(tile);
                tile.AnimateTileDestroy();
            }
            await UniTask.Delay(400);
            ClearDestroyList();
            destroyList.Clear();
            DropTile();
        }
        
        private void ClearDestroyList()
        {
            foreach (var tile in destroyList.Where(tile =>!tile.IsEmpty()))
            {
                Tiles[tile.tile.posX, tile.tile.posY].DeactivateTile();
            }
        }
        private async void ActivateParticle(TileView tile)
        {
            var particle = particlePool.FirstOrDefault(particleObject => !particleObject.activeInHierarchy);
            if (particle != null)
            {
                SetParticle(tile, particle);
                particle.SetActive(true);
                await UniTask.Delay(500);
                particle.SetActive(false);
            }
        }
        private void SetParticle(TileView tile, GameObject particle)
        {
            var tileParticles = particle.GetComponentsInChildren<ParticleSystem>(); 
            particle.transform.SetParent(transform);
            particle.transform.position = tile.transform.position;
            foreach (var tileParticle in tileParticles)
            {
                var tileParticleMain = tileParticle.main;
                tileParticleMain.startColor = tile.GetColor();
            }
        }
        private async void DropTile()
        {
            await UniTask.Delay(100);
            int emptyTileCount = 0;
            for (int i = 0; i < gridWidth; i++)
            {
                for (int j = 0; j < gridHeight; j++)
                {
                    if (Tiles[i, j].IsEmpty())
                    {
                        emptyTileCount++;
                        GameManager.Instance.IncreaseScore(TileScore);
                    }
                    else if (emptyTileCount > 0)
                    {
                        (Tiles[i, j - emptyTileCount], Tiles[i, j]) = (Tiles[i, j],Tiles[i, j - emptyTileCount]);
                        Tiles[i,j-emptyTileCount].UpdatePosition(i,j-emptyTileCount);
                        Tiles[i, j].transform.DOMove(new Vector3(i, j),0.1f);
                        Tiles[i, j - emptyTileCount].gameObject.name = Tiles[i, j - emptyTileCount].GiveName(i,j-emptyTileCount);
                    }
                }
                emptyTileCount = 0;
            }
            RefillGrid();
        }
        private async void RefillGrid()
        {
            await UniTask.Delay(250);
            var gridSize = gridWidth * gridHeight; 
            for (int i = 0; i < gridWidth; i++)
            {
                for (int j = 0; j < gridHeight; j++)
                {
                    if (Tiles[i, j].IsEmpty())
                    {
                        var tileView = Tiles[i, j]; 
                        if (IsBoosterRound(LevelManager.Instance.GetCurrentLevel()))
                        {
                            tileView.UpdateTile(tileView,(TileType)tileView.GiveBoosterTypeIndex(), transform);
                        }
                        else
                        {
                            tileView.UpdateTile(tileView,(TileType)tileView.GiveTileTypeIndex(gridSize), transform);
                        }
                    }
                }
            }
            FindAllMatches();
        }
        public async void FindAllMatches() 
        { 
            await UniTask.Delay(400);
            for (int x = 0; x < gridWidth; x++)
            { 
                FindVerticalMatches(x);
            }
            for (int y = 0; y < gridHeight; y++)
            { 
                FindHorizontalMatches(y);
            }
            if (destroyList.Count < DestroyThreshold)
            {
                if (GameManager.Instance.moveCount <= 0)
                {
                    await UniTask.Delay(750);
                    GameManager.Instance.HandleLevelEnd();
                }
                GameManager.Instance.currentState = GameState.ready;
            }
            else
            {
                await UniTask.Delay(100);
                DestroyMatches();
            }
        }
        public void ActivateBooster(TileView booster)
        {
            switch (booster.tile.tileType)
            { 
                case TileType.RowBooster:
                    ActivateRowBooster(booster);
                    break;
                case TileType.ColumnBooster:
                    ActivateColumnBooster(booster);
                    break;
            }
            DestroyMatches();
        }
        private void ActivateRowBooster(TileView booster)
        {
            for (int i = 0; i < gridWidth; i++)
            {
                destroyList.Add(Tiles[i, booster.tile.posY]);
            }
        }
        private void ActivateColumnBooster(TileView booster)
        {
            for (int i = 0; i < gridHeight; i++)
            {
                destroyList.Add(Tiles[booster.tile.posX, i]);
            }
        }
        private bool IsBoosterRound(Level level)
        {
            var round = Random.Range(0, level.moveCount - 7); 
            return GameManager.Instance.moveCount == round;
        }
        
    }
}
