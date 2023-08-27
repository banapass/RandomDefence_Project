using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveHomingProjectile : HomingProjectile
{

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

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (unit == null) return;
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, unit.ProjectileInfo.radius);
    }
#endif
}
