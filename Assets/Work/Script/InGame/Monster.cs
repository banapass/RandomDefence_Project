using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour, IDamageable, IObjectable
{

    public string ObjectID { get; set; }

    private MonsterInfo inMonsterInfo;
    private float currHp;
    private float maxHp;
    private float speed = 10;
    private List<Vector3> path;

    private int currentPathIndex;
    private bool isDestination;

    public event Action<Monster> OnDeath;


    public void Init(MonsterInfo _monsterInfo, List<Vector3> _path, Action<Monster> _onDeath)
    {

        this.inMonsterInfo = _monsterInfo;
        this.path = _path;
        currentPathIndex = 0;
        transform.position = path[currentPathIndex];

        if (OnDeath == null)
            OnDeath = _onDeath;

        if (path == null) Debug.LogError("Monster Init Failed : Path Is Null");
    }
    private void OnEnable()
    {

    }
    private void OnDisable()
    {
    }
    private void Update()
    {
        if (path == null) return;
        if (isDestination) return;
        FollowPath();
    }
    public void TakeDamage(float _damage)
    {
        currHp -= _damage;

        if (currHp <= 0)
            OnDie();

    }
    private void OnDie()
    {
        // Debug.Log("Monster Is Dead");
        OnDeath(this);

        ObjectPoolManager.Instance.ReturnParts(this, ObjectID);
        Destroy(this.gameObject);
    }
    public void FollowPath()
    {

        if (IsArrivalNextDestination(path[currentPathIndex]))
        {
            currentPathIndex++;
            isDestination = path.Count <= currentPathIndex;

            if (isDestination) OnDie();
        }
        else
        {
            MoveToPoint(path[currentPathIndex]);
        }

    }
    private bool IsArrivalNextDestination(Vector3 _nextPos)
    {
        return GetDistance(_nextPos) <= 0.1f;
    }
    private float GetDistance(Vector3 _nextPos)
    {
        return (_nextPos - transform.position).sqrMagnitude;
    }
    public void MoveToPoint(Vector3 _target)
    {
        transform.position = Vector3.MoveTowards(transform.position, _target, Time.deltaTime * speed);
    }
}
