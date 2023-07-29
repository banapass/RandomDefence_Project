
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [field: SerializeField]
    public Camera GameCamera { get; private set; }
    [SerializeField] Transform boardArea;
    [SerializeField] Tile tilePrefab;
    [SerializeField] Vector2 tileSize;
    [SerializeField] int createCount;
    [SerializeField] int cameraSpecing;
    [SerializeField] Tile[,] tileMap;

    // Start is called before the first frame update
    void Start()
    {
        CreateBoard(tileSize);
        // CameraResizing(boardArea.localScale.x);
    }

    private void CreateBoard(Vector2 _tileSize)
    {
        bool _isOddNumberX = _tileSize.x % 2 != 0;
        bool _isOddNumberY = _tileSize.y % 2 != 0;
        Vector2 _start = _tileSize / 2;

        tileMap = new Tile[(int)_tileSize.y, (int)_tileSize.x];


        Vector2 _resolusionSize = (Vector2)boardArea.localScale / _tileSize;
        Vector2 _startPos = (Vector2)boardArea.transform.position + (-tileSize * 0.5f) * _resolusionSize;// new Vector2(-_start, -_start) * _resolusionSize;

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