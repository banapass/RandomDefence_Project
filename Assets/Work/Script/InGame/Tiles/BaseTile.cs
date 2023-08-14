using System;
using UnityEngine;

abstract public class BaseTile : MonoBehaviour
{
    public virtual bool IsWalkable { get { return true; } }
}