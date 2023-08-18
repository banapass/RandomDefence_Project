using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using framework;

public class WaveManager : Singleton<WaveManager>
{
    private BoardManager boardManager;
    private StageInfo currStageInfo;
    private MonsterInfo currWaveMonster;
    private List<Monster> spawnMonsters;

    private TrailRenderer wayEffector;

    private int currRound;
    private int remainSpawnCount = 0;
    IEnumerator spawning;

    public void Init(BoardManager _board, StageInfo _stageInfo)
    {
        spawnMonsters = new List<Monster>();

        this.boardManager = _board;
        currStageInfo = _stageInfo;
        currRound = -1;

        MonsterPooling();
        // StartNextRound();
        TrailRenderer _wayRenderer = ResourceStorage.GetResource<TrailRenderer>("Prefab/Effect/Way");
        wayEffector = Instantiate(_wayRenderer);
        wayEffector.enabled = false;

        MemoryPool<Debuff> _debuffPool = new MemoryPool<Debuff>();
        // _debuffPool.AddPool<SlowDebuff>(10);
    }
    private void MonsterPooling()
    {
        var _monsterInfos = TableManager.Instance.GetAllMonsterInfo();

        for (int i = 0; i < _monsterInfos.Count; i++)
        {
            var _rawMonster = framework.ResourceStorage.GetResource<Monster>(_monsterInfos[i].prefabPath);
            ObjectPoolManager.Instance.AddPool<Monster>(_rawMonster, 40, _monsterInfos[i].monsterId);
        }
    }
    public void StartNextRound()
    {
        if (!GameManager.Instance.IsBreakTime) return;
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
        Monster _newMonster = ObjectPoolManager.Instance.GetParts<Monster>(currWaveMonster.monsterId);
        _newMonster.Init(currWaveMonster, boardManager.GetCurrentPath(), OnMonsterDeath);
        spawnMonsters.Add(_newMonster);
        remainSpawnCount--;

        if (remainSpawnCount <= 0)
        {
            StopSpawning();
        }
    }
    private void OnMonsterDeath(Monster _monster)
    {

        if (spawnMonsters.Contains(_monster))
            spawnMonsters.Remove(_monster);
        else
            Debug.LogError("스폰된 몬스터 리스트에 추가되지않은 몬스터가 죽었습니다.");


        if (remainSpawnCount > 0) return;
        if (spawnMonsters.Count > 0) return;

        CheckGameEnd();

    }
    private void CheckGameEnd()
    {
        bool _isEnded = currStageInfo.rounds.Length <= currRound + 1;

        if (_isEnded)
        {
            Debug.Log("Game Ended");
        }
        else
        {
            GameManager.Instance.ChangeGameState(GameState.BreakTime);
            // StartNextRound();
        }
    }
}
