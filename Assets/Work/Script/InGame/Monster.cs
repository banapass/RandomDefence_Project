using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour, IDamageble
{
    private float currHp;
    private float maxHp;
    private float speed = 3;

    public void TakeDamage(float _damage)
    {
        currHp -= _damage;

        if (currHp <= 0)
            OnDie();

    }
    private void OnDie()
    {
        Debug.Log("Monster Is Dead");
    }

    public void MoveToPoint(Vector3 _target)
    {
        transform.position = Vector3.MoveTowards(transform.position, _target, Time.deltaTime * speed);
    }
}
