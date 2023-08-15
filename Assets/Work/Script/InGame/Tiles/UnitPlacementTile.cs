using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPlacementTile : BaseTile
{
    public override bool IsWalkable => false;
    public bool HasUnit => unit != null;
    private Unit unit;
    private SpriteRenderer sp;
    private void Awake()
    {
        TryGetComponent<SpriteRenderer>(out sp);
    }

    public void Init(Unit _unit)
    {
        if (unit != null) return;
        unit = _unit;
        unit.transform.position = transform.position;
        unit.ResetCoolTime();

        SetTileDisplaySprite(unit.Info.rarity);

    }
    private void SetTileDisplaySprite(UnitRarity _rarity)
    {
        Sprite _sprite = AtlasManager.Instance.GetUnitRarityTileSprite(_rarity);
        sp.sprite = _sprite;
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
