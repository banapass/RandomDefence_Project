using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class Unit : MonoBehaviour
{
    public UnitInfo Info { get; protected set; }
    protected float currentCoolTime;
    [SerializeField] protected LayerMask targetLayer;

    public void Init(UnitInfo _unitInfo)
    {
        Info = _unitInfo;
        currentCoolTime = Info.coolTime;
    }
    public virtual bool OnAttack()
    {
        Collider2D _col = Physics2D.OverlapCircle(transform.position, Info.range, targetLayer);

        if (_col == null) return false;

        if (_col.TryGetComponent<Monster>(out var _monster))
        {
            ProjectileBase _projectile = ObjectPoolManager.Instance.GetParts<ProjectileBase>(Info.projectileInfo.prefab);
            _projectile.Init(this, _monster);
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
}
