using System;
using UnityEngine;

abstract public class BaseTile : MonoBehaviour
{
    public virtual bool IsWalkable { get { return true; } }

    public void SetTilePos(int x, int y)
    {
        transform.position = new Vector2(x, y);
    }
}