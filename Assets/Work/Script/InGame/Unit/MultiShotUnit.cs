using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiShotUnit : Unit
{
    [SerializeField] float angle;


    public override bool OnAttack()
    {
        Collider2D _col = Physics2D.OverlapBox(transform.position, Vector2.one * Info.CalculateRange(), 0, targetLayer);

        if (_col == null) return false;

        if (_col.TryGetComponent<Monster>(out var _monster))
        {
            LaunchProjectile(_monster);
        }

        return _col != null;
    }
    public void LaunchProjectile(Monster _target)
    {
        Vector2 _dir = (_target.transform.position - transform.position).normalized;
        float angleStep = angle / (ProjectileInfo.count - 1);
        float halfAngle = angle / 2;

        for (int i = 0; i < ProjectileInfo.count; i++)
        {
            float currentAngle = -halfAngle + i * angleStep;
            Vector2 _calculateDir= Quaternion.Euler(0f, 0f, currentAngle) * _dir;

            ObjectPoolManager.Instance.GetParts<ProjectileBase>(Info.projectileInfo.prefab, _onComplete: _projectile =>
            {
                _projectile.Init(this, _target, _calculateDir);
            });
        }
    }
}
