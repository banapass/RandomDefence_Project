using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EmptyTile : BaseTile
{
    private PlacementTile inTile;
    public Coord TileCoord { get; private set; }
    public override bool IsWalkable => inTile == null || inTile.IsWalkable;
    public bool IsPlaceable => inTile == null;
    private Vector2 defaultSize;

    public void SetInnerTile(PlacementTile _tile)
    {
        if (!IsPlaceable) return;
        if (inTile != null) inTile.ReturnPool();

        _tile.Init(this);
        _tile.transform.position = transform.position - Vector3.forward;
        _tile.transform.localScale = transform.localScale;

        inTile = _tile;
    }
    public void RemoveInnerTile()
    {
        if (IsPlaceable) return;

        BoardManager.Instance.RemovePlacementTile(inTile);
        inTile = null;

        if (BoardManager.Instance.CheckPathImpactOnTileRemoval(this))
            BoardManager.Instance.UpdatePath();
    }
    public void SetCoord(Coord _coord)
    {
        TileCoord = _coord;
    }
    public void SetDefaultSize(Vector2 _defaultSize)
    {
        defaultSize = _defaultSize;
    }

    public void PlayFadeOutTween()
    {
        transform.DOScale(Vector2.zero, 1.0f)
        .SetEase(Ease.OutElastic);
    }
    public Tween PlayFadeInTween(Ease _ease = Ease.InElastic)
    {
        return transform.DOScale(defaultSize, 1.0f)
               .SetEase(_ease);
    }

}
