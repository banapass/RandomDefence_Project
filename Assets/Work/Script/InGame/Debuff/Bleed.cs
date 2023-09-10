using System.Collections.Generic;
using UnityEngine;

public class Bleed : Debuff
{
    private float nextAttackTime;
    private ParticleEffector bleedEffector;
    private const float ATTACK_DELAY = 1.0f;

    public override void Init(DebuffInfo _info, Unit _unit, Monster _monster)
    {
        base.Init(_info, _unit, _monster);
        nextAttackTime = elapsedTime - ATTACK_DELAY;
    }
    public override void UpdateTime(float _deltaTime)
    {
        base.UpdateTime(_deltaTime);
        if (nextAttackTime < elapsedTime)
        {
            ApplyDamageOverTime();
        }

        FollowBleedEffector();
    }

    private void ApplyDamageOverTime()
    {
        float _damage = debuffedMonster.MaxHp * 0.01f;
        debuffedMonster.TakeDamage(_damage);

        nextAttackTime -= ATTACK_DELAY;
    }
    private void FollowBleedEffector()
    {
        if (bleedEffector == null) return;
        if (debuffedMonster == null) return;
        if (debuffedMonster.IsDead)
        {
            Release();
            return;
        }

        bleedEffector.transform.position = debuffedMonster.transform.position;

    }
    public override void Release()
    {
        if (bleedEffector != null)
        {
            bleedEffector.ReturnPool();
            bleedEffector = null;
        }

        Attacker = null;
        debuffedMonster = null;

        MemoryPoolManager.Instance.Release<Debuff>(this);
    }
}
