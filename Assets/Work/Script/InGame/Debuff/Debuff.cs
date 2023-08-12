using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Debuff
{
    [field: SerializeField]
    public DebuffInfo Info { get; protected set; }
    public Unit Attacker { get; protected set; }

    [SerializeField] protected float elapsedTime;

    // public virtual DebuffType DebuffType { get; }
    // protected float duration;
    // protected float intensity;
    // protected float chanceOfDebuff;

    public Debuff()
    {

    }
    public void Init(DebuffInfo _info, Unit _unit)
    {
        Info = _info;
        Attacker = _unit;
        elapsedTime = Info.duration;
    }
    public virtual void UpdateDuration(float _deltaTime)
    {
        elapsedTime -= _deltaTime;
    }
    public bool IsDubuffFinished() => elapsedTime <= 0;
    public void ResetTime()
    {
        elapsedTime = Info.duration;
    }
}
