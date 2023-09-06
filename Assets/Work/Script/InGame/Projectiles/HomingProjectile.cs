using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : ProjectileBase
{
    public override void Init(Unit _unit, Monster _monster)
    {
        base.Init(_unit, _monster);

        //if (_unit == null ||_monster == null)
        //{
        //    ReturnPool();
        //    return;
        //}

        transform.position = _unit.transform.position;
    }
    public override void Init(Unit _unit, Monster _monster, Vector2 _dir)
    {
        base.Init(_unit, _monster, _dir);

        //if (_unit == null || _monster == null)
        //{
        //    ReturnPool();
        //    return;
        //}

        transform.position = _unit.transform.position;
    }

    protected virtual void Update()
    {
        if (!CheckValidProjectile())
        {
            ReturnPool();
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, GetProjectileSpeed() * Time.deltaTime);
    }
    protected override void OnCollisionMonster(Monster _hitMonster)
    {
        base.OnCollisionMonster(_hitMonster);
    }
    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<Monster>(out var _hitMonster)) return;

        OnCollisionMonster(_hitMonster);
        ReturnPool();
    }
}
