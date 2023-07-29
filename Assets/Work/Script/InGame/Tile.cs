using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public void SetTilePos(int x, int y)
    {
        transform.position = new Vector2(x, y);
    }
}