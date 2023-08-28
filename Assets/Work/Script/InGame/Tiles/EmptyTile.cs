using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EmptyTile : BaseTile
{
    private BaseTile inTile;
    public Coord TileCoord { get; private set; }
    public override bool IsWalkable => inTile == null || inTile.IsWalkable;
    public bool IsPlaceable => inTile == null;
    private Vector2 defaultSize;

    private void Awake()
    {
        defaultSize = transform.localScale;
    }

    public void SetInnerTile(BaseTile _tile)
    {
        if (!IsPlaceable) return;
        if (inTile != null) Destroy(inTile.gameObject);

        _tile.transform.position = transform.position;
        _tile.transform.localScale = transform.localScale;
        inTile = _tile;
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
