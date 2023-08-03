using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] Transform boardArea;
    [SerializeField] Tile tilePrefab;
    [SerializeField] Tile[,] tileMap;
    AStarPathfinding pathfinding;

    List<Vector3> path;

    public bool IsBreakTime { get { return CurrentGameState == GameState.BreakTime; } }

    [field: SerializeField, ReadOnly]
    public GameState CurrentGameState { get; private set; }
    public static event Action<GameState> OnChangedGameState;

    void Start()
    {
        CreateBoard(Constants.MAP_SIZE);
        pathfinding = new AStarPathfinding();
        pathfinding.Init();
        pathfinding.InitializeGrid(tileMap);
        path = pathfinding.FindPath();

        MonsterController.Instance.Init(this);
    }

    private void CreateBoard(Vector2 _tileSize)
    {
        bool _isOddNumberX = _tileSize.x % 2 != 0;
        bool _isOddNumberY = _tileSize.y % 2 != 0;
        Vector2 _start = _tileSize / 2;

        tileMap = new Tile[(int)_tileSize.y, (int)_tileSize.x];


        Vector2 _resolusionSize = (Vector2)boardArea.localScale / _tileSize;
        Vector2 _startPos = (Vector2)boardArea.transform.position + (-_tileSize * 0.5f) * _resolusionSize;

        for (int y = 0; y < _tileSize.y; y++)
        {
            for (int x = 0; x < _tileSize.x; x++)
            {
                var _tile = Instantiate(tilePrefab, transform);
                _tile.transform.localScale = _resolusionSize;

                Vector2 _nextPos = new Vector2(x, y) * _resolusionSize;
                if (!_isOddNumberX || !_isOddNumberY) _nextPos += Vector2.one * (_resolusionSize * 0.5f);

                // _tile.SetTilePos(x, y);
                _tile.name = string.Format($"({x} / {y})");
                _tile.transform.position = _startPos + _nextPos;
                tileMap[y, x] = _tile;
            }
        }
    }
    public void ChangeGameState(GameState _gameState)
    {
        if (CurrentGameState == _gameState) return;

        CurrentGameState = _gameState;
        OnChangedGameState?.Invoke(_gameState);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        if (pathfinding == null) return;
        if (pathfinding.Path == null) return;

        for (int i = 0; i < pathfinding.Path.Count; i++)
        {
            Gizmos.DrawWireCube(pathfinding.Path[i], Vector3.one);
        }
    }
    public List<Vector3> GetCurrentPath() => pathfinding.Path;
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