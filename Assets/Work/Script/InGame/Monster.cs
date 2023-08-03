using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour, IDamageble
{
    private MonsterInfo inMonsterInfo;
    private float currHp;
    private float maxHp;
    private float speed = 3;

    private List<Vector3> path;

    public void Init(MonsterInfo _monsterInfo, List<Vector3> _path)
    {
        this.inMonsterInfo = _monsterInfo;
        this.path = _path;
        if (path == null) Debug.LogError("Monster Init Failed : Path Is Null");

    }

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
