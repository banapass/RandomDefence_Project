using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ProjectileBase : MonoBehaviour, IObjectable
{
    public string ObjectID { get; set; }
    protected Unit unit;
    protected Monster target;

    public virtual void Init(Unit _unit, Monster _monster)
    {
        unit = _unit;
        target = _monster;
        CheckValidProjectile();
    }
    public virtual void Init(Unit _unit, Monster _monster, Vector2 _dir)
    {
        unit = _unit;
        target = _monster;
        CheckValidProjectile();
    }
    protected virtual bool CheckValidProjectile()
    {
        if (unit.enabled && !target.IsDead) return true;

        ReturnPool();
        return false;
    }


    public void ReturnPool()
    {
        ObjectPoolManager.Instance.ReturnParts(this, ObjectID);
    }
    protected void TryApplyDebuff(Monster _target)
    {
        if (_target.IsDead) return;
        if (!unit.HasDebuff) return;

        DebuffInfo _debuffInfo = unit.Info.projectileInfo.debuffInfo.Value;
        bool _isSuccess = _debuffInfo.TryApplyDebuff();

        if (!_isSuccess) return;

        Debuff _appledDebuff = _target.HasSameAttacker(unit);

        if (_appledDebuff != null)
        {
            _appledDebuff.ResetTime();
        }
        else
        {
            Debuff _debuff = MemoryPoolManager.Instance.GetDebuff(_debuffInfo.debuffType);
            _debuff.Init(_debuffInfo, unit, _target);
            _target.AddDebuff(_debuff);
        }
    }
    protected virtual void OnCollisionMonster(Monster _hitMonster)
    {
        _hitMonster.TakeDamage(unit.CalculateDamage());
        TryApplyDebuff(_hitMonster);
        TryShowEffect();
    }
    protected virtual void TryShowEffect()
    {
        if (unit == null) return;
        if (string.IsNullOrEmpty(unit.ProjectileInfo.effector)) return;

        ObjectPoolManager.Instance.GetParts<Effector>(unit.ProjectileInfo.effector, _onComplete: _effect =>
        {
            _effect.transform.position = transform.position;
        });
    }

    protected float GetProjectileSpeed()
    {
        return unit.ProjectileInfo.speed;
    }

    protected virtual void OnBecameInvisible()
    {
        Log.Logger.Log("Return Projectile");
        ReturnPool();
    }
}
