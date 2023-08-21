using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class DirectProjectile : ProjectileBase
{
    private Vector3 direction;
    private float speed = 3;
    private float addTime = 0;
    private List<Monster> hitMonsters;
    private System.IDisposable updateObserver;

    public override void Init(Unit _unit, Monster _monster)
    {
        base.Init(_unit, _monster);
        if (hitMonsters == null) hitMonsters = new List<Monster>();

        direction = (_monster.transform.position - _unit.transform.position).normalized;
        transform.position = _unit.transform.position;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

        addTime = 0;

        updateObserver = this.UpdateAsObservable()
        .Subscribe(_ => UpdateProjectile());
    }

    private void OnDisable()
    {
        if (updateObserver == null) return;

        updateObserver.Dispose();
        updateObserver = null;
    }
    private void UpdateProjectile()
    {
        if (addTime > Constants.PROJECTILE_LIFETIME)
        {
            ReturnPool();
            return;
        }

        transform.position += direction * unit.Info.projectileInfo.speed * Time.deltaTime;
        addTime += Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<Monster>(out var _hitMonster)) return;
        if (hitMonsters.Contains(_hitMonster)) return;

        hitMonsters.Add(_hitMonster);
        _hitMonster.TakeDamage(unit.CalculateDamage());
        TryApplyDebuff(_hitMonster);
    }
}