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
}
