using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Debuff : IMemoryPool
{
    public string Key { get; set; }

    [field: SerializeField]
    public DebuffInfo Info { get; protected set; }
    public Unit Attacker { get; protected set; }
    protected Monster debuffedMonster;

    [SerializeField] protected float elapsedTime;

    public Debuff()
    {

    }
    public virtual void Init(DebuffInfo _info, Unit _unit, Monster _monster)
    {
        Info = _info;
        Attacker = _unit;
        debuffedMonster = _monster;
        elapsedTime = Info.duration;
    }
    public virtual void UpdateTime(float _deltaTime)
    {
        elapsedTime -= _deltaTime;
    }
    public bool IsDubuffFinished() => elapsedTime <= 0;
    public void ResetTime()
    {
        elapsedTime = Info.duration;
    }
    public virtual void Release()
    {
        MemoryPoolManager.Instance.Release<Debuff>(this);
    }
}
