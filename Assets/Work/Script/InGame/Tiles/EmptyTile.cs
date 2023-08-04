using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyTile : BaseTile
{
    private BaseTile inTile;
    public override bool IsWalkable => inTile == null || inTile.IsWalkable;
    public bool IsCanBatch => inTile == null;

    public void SetInnerTile(BaseTile _tile)
    {
        if (!IsCanBatch) return;
        if (inTile != null) Destroy(inTile.gameObject);

        _tile.transform.position = transform.position;
        _tile.transform.localScale = transform.localScale;
        inTile = _tile;
    }

}
