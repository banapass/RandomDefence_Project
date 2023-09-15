using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour, IDamageable, IObjectable
{

    public string ObjectID { get; set; }

    private MonsterStatInfo statInfo;
    [SerializeField, ReadOnly] private float currHp;
    private MonsterHpSlider hpSlider;

    private List<Node> path;
    [SerializeField] private List<Debuff> debuffs;
    private List<Debuff> finishedDebuffs;

    private int currentPathIndex;
    private bool isDestination;

    public float CurrHp { get { return currHp; } }
    public float MaxHp { get { return statInfo.hp; } }

    private bool isDead;
    public bool IsDead { get { return isDead; } }


    public static event Action<Monster> OnDeath;
    public static event Action<MonsterHitInfo> OnTakeDamage;
    public static event Action OnArrivalLastDestination;


    public void Init(MonsterStatInfo _monsterStatInfo)
    {
        this.statInfo = _monsterStatInfo;
        this.currHp = statInfo.hp;
        this.path = BoardManager.Instance.GetCurrentPath();

        isDead = false;
        isDestination = false;
        currentPathIndex = 0;
        transform.position = path[currentPathIndex].worldPosition;

        if (path == null) Log.Logger.LogError("Monster Init Failed : Path Is Null");

        SetUpHpSlider();
        InitDebuffs();
    }
    private void Update()
    {
        if (path == null) return;
        if (isDestination) return;
        if (isDead) return;

        FollowPath();
        UpdateDebuff(Time.deltaTime);
        UpdateHpSliderPosition();
    }
    private void OnEnable()
    {
        GameManager.OnChangedGameState += OnChangedGameState;
    }
    private void OnDisable()
    {
        GameManager.OnChangedGameState -= OnChangedGameState;
        ReturnAllDebuff();
    }
    private void OnChangedGameState(GameState _state)
    {
        if (_state != GameState.GameOver) return;
        OnDie();
        if (hpSlider != null) Destroy(hpSlider.gameObject);
    }


    private void SetUpHpSlider()
    {
        if (hpSlider != null) return;

        ObjectPoolManager.Instance.GetParts<MonsterHpSlider>(Constants.MONSTER_HPBAR, true, _slider =>
        {
            hpSlider = _slider;
            hpSlider.UpdateSlider(GetHPPercent());
            hpSlider.UpdatePosition(GetHPBarPosition());
        });

    }
    private void InitDebuffs()
    {
        if (debuffs == null) debuffs = new List<Debuff>();
        else debuffs.Clear();

        if (finishedDebuffs == null) finishedDebuffs = new List<Debuff>();
        else finishedDebuffs.Clear();
    }
    private void UpdateHpSliderPosition()
    {
        if (hpSlider == null) return;
        hpSlider.UpdatePosition(GetHPBarPosition());
    }
    private void UpdateHpSliderValue()
    {
        if (hpSlider == null) return;
        hpSlider.UpdateSlider(GetHPPercent());
    }
    public void TakeDamage(float _damage)
    {
        if (isDead) return;

        currHp -= _damage;
        UpdateHpSliderValue();

        if (currHp <= 0)
            OnDie();

        //ObjectPoolManager.Instance.GetParts<TextEffector>(Constants.FLOATING_TEXT, _onComplete: _text =>
        //{
        //    _text.transform.position = transform.position;
        //    _text.Play(_damage);
        //});

        OnTakeDamage?.Invoke(new MonsterHitInfo((int)_damage, transform.position));
    }
    private void OnDie()
    {
        if (isDead) return;
        isDead = true;

        if (!isDestination)
        {
            Log.Logger.Log("몬스터를 처치했습니다");
            OnDeath(this);
        }
        else
        {
            Log.Logger.Log("몬스터가 최종 목적지에 도착했습니다");

            OnDeath(this);
            OnArrivalLastDestination?.Invoke();
        }

        ObjectPoolManager.Instance.GetParts<ParticleEffector>(Constants.MONSTER_DEAD_KEY, _onComplete: _effector =>
        {
            _effector.transform.position = transform.position;
        });

        if (hpSlider != null)
        {
            hpSlider.ReturnPool();
            hpSlider = null;
        }

        ReturnPool();
    }

    #region Move 관련
    public void FollowPath()
    {

        if (IsArrivalNextDestination(path[currentPathIndex].worldPosition))
        {
            currentPathIndex++;
            isDestination = path.Count <= currentPathIndex;

            if (isDestination) OnDie();
        }
        else
        {
            MoveToPoint(path[currentPathIndex].worldPosition);
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

    private float GetHPPercent()
    {
        return currHp / this.statInfo.hp;
    }

    private Vector2 GetHPBarPosition()
    {
        Vector2 _currPos = transform.position;
        _currPos.y = _currPos.y - transform.localScale.y * 0.5f;

        return _currPos;
    }
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

        return statInfo.speed - statInfo.speed * _slowIntensity;
    }

    #region Debuff 관련

    private void UpdateDebuff(float _deltaTime)
    {
        //int i = 0; i < debuffs.Count; i++

        // using (var _em = debuffs.GetEnumerator())
        // {
        //     while (_em.MoveNext())
        //     {
        //         Debuff _debuff = _em.Current;
        //         _debuff.UpdateTime(Time.deltaTime);

        //         if (!_debuff.IsDubuffFinished()) continue;
        //         if (finishedDebuffs.Contains(_debuff)) continue;

        //         finishedDebuffs.Add(_debuff);
        //     }

        //     for (int i = 0; i < finishedDebuffs.Count; i++)
        //         RemoveDebuff(finishedDebuffs[i]);

        //     if (finishedDebuffs.Count > 0)
        //         finishedDebuffs.Clear();
        // }
        for (int i = 0; i < debuffs.Count; i++)
        {
            Debuff _debuff = debuffs[i];
            if (_debuff == null) continue;

            _debuff.UpdateTime(Time.deltaTime);

            if (!_debuff.IsDubuffFinished()) continue;
            if (finishedDebuffs.Contains(_debuff)) continue;

            finishedDebuffs.Add(_debuff);
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
    }
    public void RemoveDebuff(Debuff _debuff)
    {
        if (!debuffs.Contains(_debuff)) return;

        _debuff.Release();
        debuffs.Remove(_debuff);
    }

    private void ReturnAllDebuff()
    {
        if (debuffs.Count <= 0) return;

        for (int i = 0; i < debuffs.Count; i++)
        {
            debuffs[i].Release();
        }

        debuffs.Clear();
        finishedDebuffs.Clear();
    }

    #endregion

    public void ReturnPool()
    {
        ObjectPoolManager.Instance.ReturnParts(this, ObjectID);
    }
}
