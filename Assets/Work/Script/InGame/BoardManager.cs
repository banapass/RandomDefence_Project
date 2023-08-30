using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using framework;

public class BoardManager : Singleton<BoardManager>
{
    [SerializeField] Transform boardArea;
    [SerializeField] EmptyTile[,] tileMap;
    private List<UnitPlacementTile> unitPlacementTiles;
    private List<UnitPlacementTile> upgradeTargetUnitTiles;
    private AStarPathfinding pathfinding;

    public static event Action OnChangedPath;


    public void Init()
    {
        unitPlacementTiles = new List<UnitPlacementTile>();

        ResourceStorage.GetComponentAsset<EmptyTile>("Prefab/Tile", _tile =>
        {
            CreateBoard(Constants.MAP_SIZE, _tile);
            pathfinding = new AStarPathfinding();
            pathfinding.Init();
            pathfinding.SynchronizeAt(tileMap, true, true);
            pathfinding.FindPath();


            PlayBoardSequences(() =>
            {
                WaveManager.Instance.Init(TableManager.Instance.GetStageInfo("stage01"));
                // WaveManager.Instance.StartWayNavigate();
            });

        });
        // ChangeGameState(GameState.BreakTime);

    }
    private void OnEnable()
    {
        GameManager.OnChangedGameState += OnChangedGameState;
        InputController.OnTriedPlaceNewTile += OnTriedPlaceNewTile;
        InputController.OnTriedNewUnit += CreateNewUnit;
        InputController.OnTriedSell += OnTriedSell;
        UnitPlacementTile.OnPlacedNewUnit += TryUpgradeUnit;


    }
    private void OnDisable()
    {
        GameManager.OnChangedGameState -= OnChangedGameState;
        InputController.OnTriedPlaceNewTile -= OnTriedPlaceNewTile;
        InputController.OnTriedNewUnit -= CreateNewUnit;
        InputController.OnTriedSell -= OnTriedSell;
        UnitPlacementTile.OnPlacedNewUnit -= TryUpgradeUnit;
    }

    private void OnTriedPlaceNewTile(EmptyTile _tile)
    {
        if (!pathfinding.IsCanPlaceTile(_tile)) return;
        if (GameManager.Instance.IsBreakTime)
        {
            CreateUnitPlacementTile(_tile);
        }
        else
        {
            if (pathfinding.IsContainPath(_tile.TileCoord)) return;
            CreateUnitPlacementTile(_tile);
        }

        GameManager.Instance.UseGold(Constants.UNITPLACEMENT_PRICE);
    }
    private void OnChangedGameState(GameState _state)
    {
        FadeInOutPath(_state == GameState.Playing);
    }
    private void FadeInOutPath(bool _fadeOut)
    {
        if (pathfinding == null) return;

        List<Node> _currPath = pathfinding.Path;
        for (int i = 0; i < _currPath.Count; i++)
        {
            Node _node = _currPath[i];
            EmptyTile _tile = tileMap[_node.coord.y, _node.coord.x];

            if (_fadeOut) _tile.PlayFadeOutTween();
            else _tile.PlayFadeInTween();

        }
    }

    private void TryUpgradeUnit(UnitPlacementTile _placedTile)
    {
        if (!IsUpgradableUnit(_placedTile.InUnit.Info.rarity)) return;

        if (upgradeTargetUnitTiles == null) upgradeTargetUnitTiles = new List<UnitPlacementTile>();
        else upgradeTargetUnitTiles.Clear();

        upgradeTargetUnitTiles.Add(_placedTile);

        UnitRarity _nextRarity = _placedTile.InUnit.Info.rarity + 1;


        for (int i = 0; i < unitPlacementTiles.Count; i++)
        {
            UnitPlacementTile _tile = unitPlacementTiles[i];

            if (!_tile.HasUnit) continue;
            if (upgradeTargetUnitTiles.Contains(_tile)) continue;

            bool _isSameUnit = _placedTile.InUnit.Info.unitId == _tile.InUnit.Info.unitId;

            if (_isSameUnit)
                upgradeTargetUnitTiles.Add(_tile);
        }

        if (upgradeTargetUnitTiles.Count >= Constants.UNIT_UPGRADE_COUNT)
        {
            for (int i = 0; i < upgradeTargetUnitTiles.Count; i++)
                upgradeTargetUnitTiles[i].DeleteUnit();

            CreateNewUnit(_placedTile, _nextRarity);
        }

        upgradeTargetUnitTiles.Clear();
    }
    private bool IsUpgradableUnit(UnitRarity _rarity)
    {
        return _rarity + 1 <= UnitRarity.Legendary;
    }
    private void CreateNewUnit(UnitPlacementTile _tile)
    {
        UnitInfo _selectedUnit = TableManager.Instance.GetRandomUnitInfo();

        ResourceStorage.GetComponentAsset<Unit>(_selectedUnit.unitId, _rawUnit =>
        {
            Unit _instUnit = Instantiate(_rawUnit);
            _instUnit.Init(_selectedUnit,_tile);
            _instUnit.SetScale(_tile.GetUnitSize());
            _tile.SetUnit(_instUnit);
        });

    }
    private void CreateNewUnit(UnitPlacementTile _tile, UnitRarity _rarity)
    {
        UnitInfo _selectedUnit = TableManager.Instance.GetRandomUnitInfoByRarity(_rarity);

        ResourceStorage.GetComponentAsset<Unit>(_selectedUnit.unitId, _rawUnit =>
        {
            Unit _instUnit = Instantiate(_rawUnit);
            _instUnit.Init(_selectedUnit,_tile);
            _instUnit.SetScale(_tile.GetUnitSize());
            _tile.SetUnit(_instUnit);

        });
    }
    private void CreateBoard(Vector2 _tileSize, EmptyTile _tileRes)
    {
        bool _isOddNumberX = _tileSize.x % 2 != 0;
        bool _isOddNumberY = _tileSize.y % 2 != 0;
        Vector2 _start = _tileSize / 2;

        tileMap = new EmptyTile[(int)_tileSize.y, (int)_tileSize.x];


        Vector2 _resolusionSize = (Vector2)boardArea.localScale / _tileSize;
        Vector2 _startPos = (Vector2)boardArea.transform.position + (-_tileSize * 0.5f) * _resolusionSize;

        // EmptyTile _rawTile = ResourceStorage.GetResource<EmptyTile>("Prefab/Tile");

        int _createCount = 0;



        for (int y = 0; y < _tileSize.y; y++)
        {
            for (int x = 0; x < _tileSize.x; x++)
            {
                var _tile = Instantiate(_tileRes, transform);
                _tile.transform.localScale = Vector2.zero;

                Vector2 _nextPos = new Vector2(x, y) * _resolusionSize;
                if (!_isOddNumberX || !_isOddNumberY) _nextPos += Vector2.one * (_resolusionSize * 0.5f);

                // _tile.SetTilePos(x, y);
                _tile.name = string.Format($"({x} / {y})");
                _tile.transform.position = _startPos + _nextPos;
                _tile.SetCoord(new Coord(x, y));
                _tile.SetDefaultSize(_resolusionSize);



                _createCount++;

                tileMap[y, x] = _tile;
            }
        }


    }
    private void PlayBoardSequences(Action _onComplete = null)
    {
        Sequence _boardSeq = DOTween.Sequence();

        int _count = 0;
        for (int y = 0; y < tileMap.GetLength(0); y++)
        {
            for (int x = 0; x < tileMap.GetLength(1); x++)
            {
                EmptyTile _tile = tileMap[y, x];
                _boardSeq.Insert(0.01f * _count, _tile.PlayFadeInTween(Ease.OutCubic));
                _count++;
            }
        }

        _boardSeq.OnComplete(() => _onComplete?.Invoke());
        _boardSeq.Play();
    }
    private void CreateUnitPlacementTile(EmptyTile _tile)
    {
        framework.ResourceStorage.GetComponentAsset<UnitPlacementTile>(Constants.UNITPLACEMENT_PATH, _unitTile =>
        {
            bool _isContainPath = pathfinding.IsContainPath(_tile.TileCoord);

            ObjectPoolManager.Instance.GetParts<UnitPlacementTile>(Constants.UNITPLACEMENT_PATH, _onComplete:_placementTile =>
            {
                _tile.SetInnerTile(_placementTile);
                unitPlacementTiles.Add(_placementTile);

                pathfinding.SynchronizeAt(tileMap, true);
                pathfinding.FindPath();

                if (_isContainPath)
                    OnChangedPath?.Invoke();
            });
        });
    }

    private bool CanPlaceUnit()
    {
        for (int i = unitPlacementTiles.Count - 1; i >= 0; i--)
        {
            if (unitPlacementTiles[i].HasUnit) continue;
            return true;
        }

        return false;
    }
    public bool CheckPathImpactOnTileRemoval(EmptyTile _tile)
    {
        List<Node> _path = pathfinding.Path;

        for(int i = _path.Count - 1; i >= 0; i--) 
        {
            for (int j = 0; j < Coord.DIRECTIONS.Length; j++)
            {
                Coord _removeCoord = _tile.TileCoord;
                Coord _nextCoord = _removeCoord + Coord.DIRECTIONS[j];

                if (_path[i].coord == _nextCoord) return true;
            }
            //bool _isContainsCoord = _removeCoord.x == _path[i].coord.x && _removeCoord.y == _path[i].coord.y;
        }
        //for (int i = 0; i  < unitPlacementTiles.Count; i++)
        //{
        //    for(int j = 0; j < Coord.DIRECTIONS.Length;j++)
        //    {
        //        Coord _currCoord = unitPlacementTiles[i].GetCoord();
        //        Coord _nextCoord = _currCoord + Coord.DIRECTIONS[j];

        //        if (_nextCoord == _tile.TileCoord) return true;
        //    }
        //}
        return false;
    }
    public void OnTriedSell(ISellable _sellable)
    {
        _sellable.Sell();
    }
    public void UpdatePath()
    {
        pathfinding.SynchronizeAt(tileMap, true);
        pathfinding.FindPath();

        OnChangedPath?.Invoke();
    }
    public List<Node> GetCurrentPath() => pathfinding.Path;


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        if (pathfinding == null) return;
        if (pathfinding.Path == null) return;

        for (int i = 0; i < pathfinding.Path.Count; i++)
        {
            Gizmos.DrawWireCube(pathfinding.Path[i].worldPosition, Vector3.one);
        }
    }
#endif
    // private void OnDrawGizmos()
    // {
    //     Gizmos.DrawWireCube(transform.position, Vector2.one);
    // }
    // private void CameraResizing(float _count)
    // {
    //     Vector2 spriteSize = tilePrefab.GetTileSize() * (_count + cameraSpecing);

    //     // SpriteRenderer의 가로 크기를 기준으로 orthographic size 계산
    //     float orthoSize = spriteSize.x * 0.5f / GameCamera.aspect;

    //     // 카메라의 orthographic size 설정
    //     GameCamera.orthographicSize = orthoSize;
    // }
}