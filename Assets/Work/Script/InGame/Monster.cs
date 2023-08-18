using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour, IDamageable, IObjectable
{

    public string ObjectID { get; set; }

    private MonsterInfo inMonsterInfo;
    [SerializeField, ReadOnly] private float currHp;

    private List<Vector3> path;
    [SerializeField] private List<Debuff> debuffs;
    private List<Debuff> finishedDebuffs;

    private int currentPathIndex;
    private bool isDestination;

    private bool isDead;

    public event Action<Monster> OnDeath;
    public static event Action<MonsterHitInfo> OnTakeDamage;
    public static event Action OnArrivalLastDestination;


    public void Init(MonsterInfo _monsterInfo, List<Vector3> _path, Action<Monster> _onDeath)
    {
        this.inMonsterInfo = _monsterInfo;
        this.currHp = inMonsterInfo.hp;
        this.path = _path;
        currentPathIndex = 0;
        transform.position = path[currentPathIndex];
        isDead = false;

        if (path == null) Debug.LogError("Monster Init Failed : Path Is Null");

        if (OnDeath == null)
            OnDeath = _onDeath;


        if (debuffs == null) debuffs = new List<Debuff>();
        else debuffs.Clear();

        if (finishedDebuffs == null) finishedDebuffs = new List<Debuff>();
        else finishedDebuffs.Clear();
    }
    private void Update()
    {
        if (path == null) return;
        if (isDestination) return;
        FollowPath();
        UpdateDebuff(Time.deltaTime);
    }
    public void TakeDamage(float _damage)
    {
        if (isDead) return;

        currHp -= _damage;

        if (currHp <= 0)
            OnDie();

        OnTakeDamage?.Invoke(new MonsterHitInfo((int)_damage, transform.position));
    }
    private void OnDie()
    {
        if (isDead) return;
        isDead = true;

        if (!isDestination)
            OnDeath(this);

        else OnArrivalLastDestination?.Invoke();

        Effector _effector = ObjectPoolManager.Instance.GetParts<Effector>("Die");
        _effector.transform.position = transform.position;

        ReturnAllDebuff();
        ObjectPoolManager.Instance.ReturnParts(this, ObjectID);

    }
    #region Move 관련
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
        return GetDistance(_nextPos) <= 0.001f;
    }
    private float GetDistance(Vector3 _nextPos)
    {
        return (_nextPos - transform.position).sqrMagnitude;
    }
    public void MoveToPoint(Vector3 _target)
    {
        transform.position = Vector2.MoveTowards(transform.position, _target, Time.deltaTime * CalculateSpeed());
    }
    #endregion

    private float CalculateSpeed()
    {
        float _slowIntensity = 0;
        for (int i = 0; i < debuffs.Count; i++)
        {
            Debuff _debuff = debuffs[i];
            if (_debuff == null) continue;
            if (_debuff.Info.debuffType != DebuffType.Slow) continue;

            _slowIntensity += _debuff.Info.intensity;
        }

        _slowIntensity = _slowIntensity > Constants.MAX_SLOW ? Constants.MAX_SLOW : _slowIntensity;

        return inMonsterInfo.speed - inMonsterInfo.speed * _slowIntensity;
    }

    #region Debuff 관련

    private void UpdateDebuff(float _deltaTime)
    {
        for (int i = 0; i < debuffs.Count; i++)
        {
            debuffs[i].UpdateDuration(Time.deltaTime);

            if (!debuffs[i].IsDubuffFinished()) continue;
            if (finishedDebuffs.Contains(debuffs[i])) continue;

            finishedDebuffs.Add(debuffs[i]);
        }

        for (int i = 0; i < finishedDebuffs.Count; i++)
            RemoveDebuff(finishedDebuffs[i]);

        if (finishedDebuffs.Count > 0)
            finishedDebuffs.Clear();
    }
    public Debuff HasSameAttacker(Unit _attacker)
    {
        for (int i = 0; i < debuffs.Count; i++)
        {
            if (debuffs[i].Attacker != _attacker) continue;
            return debuffs[i];
        }
        return null;
    }
    public void AddDebuff(Debuff _debuff)
    {
        if (debuffs.Contains(_debuff)) return;

        debuffs.Add(_debuff);
        // Debuff _sameDebuff = HasSameAttacker(_debuff);
        // if (_sameDebuff != null)
        //     _sameDebuff.ResetTime();
        // else
        //     MemoryPoolManager.Instance.ReleaseDebuff(_debuff);



    }
    public void RemoveDebuff(Debuff _debuff)
    {
        if (!debuffs.Contains(_debuff)) return;

        debuffs.Remove(_debuff);
    }

    private void ReturnAllDebuff()
    {
        if (debuffs.Count <= 0) return;

        for (int i = 0; i < debuffs.Count; i++)
        {
            MemoryPoolManager.Instance.ReleaseDebuff(debuffs[i]);
        }

        debuffs.Clear();
        finishedDebuffs.Clear();
    }

    #endregion

}
