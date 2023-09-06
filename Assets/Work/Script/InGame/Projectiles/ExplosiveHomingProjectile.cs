using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveHomingProjectile : ProjectileBase// HomingProjectile
{
    private Vector2 direction;
    private float addTime;


    public override void Init(Unit _unit, Monster _monster)
    {
        unit = _unit;
        target = _monster;
        
        transform.position = _unit.transform.position;
    }

    public override void Init(Unit _unit, Monster _monster, Vector2 _dir)
    {
        unit = _unit;
        target = _monster;

        transform.position = _unit.transform.position;
        
        direction = _dir;
        
    }
    private void OnDisable()
    {
        addTime = 0;
    }
    protected override void OnCollisionMonster(Monster _hitMonster)
    {
        Collider2D[] _hitMonsters = Physics2D.OverlapCircleAll(transform.position, unit.ProjectileInfo.radius, unit.TargetLayer);

        if (_hitMonsters == null) return;

        for (int i = 0; i < _hitMonsters.Length; i++)
        {
            if (!_hitMonsters[i].TryGetComponent<Monster>(out var _hit)) continue;

            _hit.TakeDamage(unit.CalculateDamage());
            TryApplyDebuff(_hit);
        }

        TryShowEffect();
    }
    private void Update()
    {
        if (!CheckValidProjectile())
        {
            OnCollisionMonster(null);
            ReturnPool();
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, GetProjectileSpeed() * Time.deltaTime);
    }
    protected override bool CheckValidProjectile()
    {
        if (unit.enabled && !target.IsDead) return true;

        return false;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<Monster>(out var _hitMonster)) return;
        OnCollisionMonster(_hitMonster);
        ReturnPool();
    }
    protected override void OnBecameInvisible()
    {
        
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (unit == null) return;
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, unit.ProjectileInfo.radius);
    }
#endif
}
