using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool IsWalkable { get { return true; } }
    public void SetTilePos(int x, int y)
    {
        transform.position = new Vector2(x, y);
    }
}