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
        ObjectPoolManager.Instance.GetParts<ParticleEffector>(Constants.BLEED_EFFECT, _onComplete: _effect =>
        {
            bleedEffector = _effect;
        });
    }
    public override void UpdateTime(float _deltaTime)
    {
        base.UpdateTime(_deltaTime);
        if (nextAttackTime > elapsedTime)
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

#if UNITY_EDITOR
        ObjectPoolManager.Instance.GetParts<TextEffector>(Constants.FLOATING_TEXT, _onComplete: _txt =>
        {
            if (debuffedMonster == null)
            {
                _txt.ReturnPool();
                return;
            }

            TextEffector _floating = _txt;
            _floating.transform.position = debuffedMonster.transform.position;
            _floating.Play(_damage);
        });
#endif

    }
    private void FollowBleedEffector()
    {
        if (bleedEffector == null) return;
        if (debuffedMonster == null) return;
        // if (debuffedMonster.IsDead)
        // {
        //     Release();
        //     return;
        // }

        bleedEffector.transform.position = debuffedMonster.transform.position;

    }
    public override void Release()
    {
        if (bleedEffector != null) bleedEffector.ReturnPool();

        MemoryPoolManager.Instance.Release<Debuff>(this);

        Attacker = null;
        debuffedMonster = null;
        bleedEffector = null;
    }
}
