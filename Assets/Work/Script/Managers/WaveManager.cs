using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using framework;

public class WaveManager : Singleton<WaveManager>
{
    // private BoardManager boardManager;
    private StageInfo currStageInfo;
    private MonsterInfo currWaveMonster;
    private List<Monster> spawnMonsters;

    private WayNavigator wayNavigator;

    private int currRound;
    private int remainSpawnCount = 0;
    IEnumerator spawning;

    public void Init(StageInfo _stageInfo)
    {
        spawnMonsters = new List<Monster>();

        // this.boardManager = _board;
        currStageInfo = _stageInfo;
        currRound = -1;

        MonsterPooling();
        StartWayNavigate();
        // MemoryPool<Debuff> _debuffPool = new MemoryPool<Debuff>();
        // _debuffPool.AddPool<SlowDebuff>(10);
    }
    public void StartWayNavigate()
    {
        ResourceStorage.GetComponentAsset<WayNavigator>("Prefab/Effect/WayNavigator", _way =>
        {
            wayNavigator = Instantiate(_way);
            wayNavigator.UpdatePath();
            wayNavigator.StartNavigate();
        });
    }
    private void OnEnable()
    {
        Monster.OnDeath += OnMonsterDeath;
    }
    private void OnDisable()
    {
        Monster.OnDeath -= OnMonsterDeath;
    }
    private void MonsterPooling()
    {
        var _monsterInfos = TableManager.Instance.GetAllMonsterInfo();

        for (int i = 0; i < _monsterInfos.Count; i++)
        {
            ResourceStorage.GetComponentAsset<Monster, string>(_monsterInfos[i].prefabPath, (_monster, _monsterId) =>
            {
                ObjectPoolManager.Instance.AddPool<Monster>(_monster, 40, _monsterId);
            }, _monsterInfos[i].monsterId);

        }
    }
    public void StartNextRound()
    {
        if (!GameManager.Instance.IsBreakTime) return;
        if (currStageInfo.rounds == null) return;
        if (spawning == null)
            spawning = Spawning();

        currRound++;
        currWaveMonster = TableManager.Instance.GetMonsterInfo(currStageInfo.rounds[currRound].monsterId);
        remainSpawnCount = currStageInfo.rounds[currRound].spawnCount;

        GameManager.Instance.ChangeGameState(GameState.Playing);
        StartCoroutine(spawning);
    }
    public void StopSpawning()
    {
        if (spawning == null) return;

        StopCoroutine(spawning);
    }

    IEnumerator Spawning()
    {
        WaitForSeconds _spawnDelay = new WaitForSeconds(1);
        while (true)
        {
            yield return _spawnDelay;
            SpawnMonster();
        }
    }
    private void SpawnMonster()
    {
        ObjectPoolManager.Instance.GetParts<Monster>(currWaveMonster.monsterId, _onComplete: _newMonster =>
        {
            _newMonster.Init(currWaveMonster);
            spawnMonsters.Add(_newMonster);
            remainSpawnCount--;

            if (remainSpawnCount <= 0)
            {
                StopSpawning();
            }
        });
    }
    private void OnMonsterDeath(Monster _monster)
    {

        if (spawnMonsters.Contains(_monster))
            spawnMonsters.Remove(_monster);
        else
            Logger.LogError("스폰된 몬스터 리스트에 추가되지않은 몬스터가 죽었습니다.");


        if (remainSpawnCount > 0) return;
        if (spawnMonsters.Count > 0) return;

        CheckGameEnd();

    }
    private void CheckGameEnd()
    {
        bool _isEnded = currStageInfo.rounds.Length <= currRound + 1;

        if (_isEnded)
        {
            Logger.Log("게임 클리어");
            GameManager.Instance.ChangeGameState(GameState.GameClear);
        }
        else
        {
            GameManager.Instance.ChangeGameState(GameState.BreakTime);
            // StartNextRound();
        }
    }
}
