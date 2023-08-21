using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : ProjectileBase
{
    public override void Init(Unit _unit, Monster _monster)
    {
        base.Init(_unit, _monster);
        transform.position = _unit.transform.position;
    }

    private void Update()
    {
        if (target.IsDead)
        {
            ReturnPool();
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, GetProjectileSpeed() * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<Monster>(out var _hitMonster)) return;

        _hitMonster.TakeDamage(unit.CalculateDamage());
        TryApplyDebuff(_hitMonster);
        ReturnPool();
    }
}
