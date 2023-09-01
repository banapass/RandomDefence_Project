using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class Unit : MonoBehaviour , ISellable , IObjectable
{
    [field: SerializeField]
    public UnitInfo Info { get; protected set; }
    public ProjectileInfo ProjectileInfo => Info.projectileInfo;

    protected UnitPlacementTile placedTile;
    public bool HasDebuff => ProjectileInfo.debuffInfo != null;
    protected float currentCoolTime;

    [SerializeField, ReadOnly] protected LayerMask targetLayer;
    public LayerMask TargetLayer => targetLayer;

    public string ObjectID { get; set; }
    public int Price { get; set; } = 50;


    protected void Awake()
    {
        targetLayer = 1 << LayerMask.NameToLayer("Monster");
    }

    public void Init(UnitInfo _unitInfo,UnitPlacementTile _placedTile)
    {
        Info = _unitInfo;
        currentCoolTime = 0;
        placedTile = _placedTile;
    }
    public void SetScale(Vector2 _size)
    {
        transform.localScale = _size;
    }
    public virtual bool OnAttack()
    {
        Collider2D _col = Physics2D.OverlapBox(transform.position, Vector2.one * Info.CalculateRange(), 0, targetLayer);

        if (_col == null) return false;

        if (_col.TryGetComponent<Monster>(out var _monster))
        {
            ObjectPoolManager.Instance.GetParts<ProjectileBase>(Info.projectileInfo.prefab, _onComplete: _projectile =>
            {
                _projectile.Init(this, _monster);
            });
        }

        return _col != null;
    }
    public void Cooldown(float _deltaTime)
    {
        if (IsReadyToAttack()) return;
        currentCoolTime -= _deltaTime;
    }
    public bool IsReadyToAttack()
    {
        return currentCoolTime <= 0;
    }
    public void ResetCoolTime()
    {
        currentCoolTime = Info.coolTime;
    }
    public float CalculateDamage()
    {
        return Info.atk;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, Vector2.one * Info.CalculateRange());
    }

    public void Sell()
    {
        if (placedTile == null) return;

        placedTile.DeleteUnit();
        placedTile = null;

        GameManager.Instance.GainGold(Price);
    }

    public void ReturnPool()
    {
        ObjectPoolManager.Instance.ReturnParts(this, ObjectID);
    }
#endif
}
