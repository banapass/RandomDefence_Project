using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementTile : BaseTile, IObjectable, ISellable
{
    public virtual PlacementTileType Type => PlacementTileType.None;
    public override bool IsWalkable => false;
    public int Price { get; set ; }
    public string ObjectID { get ; set ; }

    public EmptyTile parentTile;


    public virtual void Init(EmptyTile _parent)
    {
        parentTile = _parent;
    }
    public void ReturnPool()
    {
        ObjectPoolManager.Instance.ReturnParts(this, ObjectID);
    }

    public void SetPrice(int _price)
    {
        Price = _price;
    }
    public virtual void Sell()
    {
        
    }

}
