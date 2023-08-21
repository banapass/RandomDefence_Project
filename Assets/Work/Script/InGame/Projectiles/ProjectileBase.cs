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
    }

    protected void ReturnPool()
    {
        ObjectPoolManager.Instance.ReturnParts(this, ObjectID);
    }
    protected void TryApplyDebuff(Monster _target)
    {
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
            Debuff _debuff = MemoryPoolManager.Instance.GetDebuff();
            _debuff.Init(_debuffInfo, unit);
            _target.AddDebuff(_debuff);
        }
    }

    protected float GetProjectileSpeed()
    {
        return unit.ProjectileInfo.speed;
    }
}
