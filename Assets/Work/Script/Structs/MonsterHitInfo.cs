using UnityEngine;
public struct MonsterHitInfo
{
    public int takeDamage;
    public Vector3 hitPosition;

    public MonsterHitInfo(int _damage, Vector3 _hitPosition)
    {
        this.takeDamage = _damage;
        this.hitPosition = _hitPosition;
    }
}