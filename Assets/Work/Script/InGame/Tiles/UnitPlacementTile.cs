using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPlacementTile : BaseTile
{
    public override bool IsWalkable => false;
    public bool HasUnit => unit != null;
    public Unit InUnit => unit;
    private Unit unit;
    private SpriteRenderer sp;

    public static event System.Action<UnitPlacementTile> OnPlacedNewUnit;
    private void Awake()
    {
        TryGetComponent<SpriteRenderer>(out sp);
    }

    public void SetUnit(Unit _unit)
    {
        if (unit != null) return;

        unit = _unit;
        unit.transform.position = transform.position;
        unit.ResetCoolTime();

        SetTileDisplaySprite(unit.Info.rarity);
        OnPlacedNewUnit?.Invoke(this);
    }
    private void SetTileDisplaySprite(UnitRarity _rarity)
    {
        Sprite _sprite = AtlasManager.Instance.GetUnitRarityTileSprite(_rarity);
        sp.sprite = _sprite;
    }
    public void DeleteUnit()
    {
        if (!HasUnit) return;

        SetTileDisplaySprite(UnitRarity.None);
        Destroy(InUnit.gameObject);
        unit = null;
    }
    public Vector2 GetUnitSize()
    {
        return transform.localScale * 0.8f;
    }

    private void Update()
    {
        if (unit == null) return;

        unit.Cooldown(Time.deltaTime);

        if (unit.IsReadyToAttack())
        {
            if (unit.OnAttack())
                unit.ResetCoolTime();
        }
    }
}
