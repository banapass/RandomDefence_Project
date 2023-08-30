using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPlacementTile : PlacementTile
{
    public bool HasUnit => unit != null;
    public Unit InUnit => unit;


    private Unit unit;
    private SpriteRenderer sp;

    public static event System.Action<UnitPlacementTile> OnPlacedNewUnit;

    private void Awake()
    {
        TryGetComponent<SpriteRenderer>(out sp); 
    }

    public override void Init(EmptyTile _parent)
    {
        base.Init(_parent);
        SetPrice(Constants.UNITPLACEMENT_PRICE);
    }

    public void SetUnit(Unit _unit)
    {
        if (unit != null)
        {
            Destroy(_unit.gameObject);
            return;
        }
        unit = _unit;
        unit.transform.position = transform.position - Vector3.forward;
        unit.ResetCoolTime();

        SetTileDisplaySprite(unit.Info.rarity);
        OnPlacedNewUnit?.Invoke(this);

        GameManager.Instance.UseGold(Constants.UNIT_PRICE);
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

    public override void Sell()
    {
        if (HasUnit) return;

        Logger.Log($"유닛 설치 타일 판매 : {Price}");
        GameManager.Instance.GainGold(Price);
        ReturnPool();

        parentTile.RemoveInnerTile();
    }
    public Coord GetCoord()
    {
        return parentTile.TileCoord;
    }
}
