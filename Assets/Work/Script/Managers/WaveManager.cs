using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using framework;
using System;

public class WaveManager : Singleton<WaveManager>
{
    private StageInfo currStageInfo;
    private MonsterInfo currWaveMonsterInfo;
    private MonsterStatInfo currWaveStatInfo;
    private List<Monster> spawnMonsters;

    private WayNavigator wayNavigator;

    private int currRound;
    private int remainSpawnCount = 0;
    IEnumerator spawning;

    public static Action<int> OnChangedRound;

    public void Init(StageInfo _stageInfo)
    {
        spawnMonsters = new List<Monster>();

        currStageInfo = _stageInfo;
        currRound = -1;

        StartWayNavigate();
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
        GameManager.OnChangedGameState += OnChangedGameState;
        Monster.OnDeath += OnMonsterDeath;
    }

    private void OnDisable()
    {
        GameManager.OnChangedGameState -= OnChangedGameState;
        Monster.OnDeath -= OnMonsterDeath;
    }

    private void OnChangedGameState(GameState _state)
    {
        if (_state != GameState.GameOver) return;

        StopCoroutine(spawning);
    }

    public void StartNextRound()
    {
        if (!GameManager.Instance.IsBreakTime) return;
        if (currStageInfo.rounds == null) return;
        if (spawning == null)
            spawning = Spawning();

        currRound++;
        currWaveMonsterInfo = TableManager.Instance.GetMonsterInfo(currStageInfo.rounds[currRound].monsterId);
        currWaveStatInfo = currStageInfo.rounds[currRound].monsterStatInfo;
        remainSpawnCount = currStageInfo.rounds[currRound].spawnCount;

        GameManager.Instance.ChangeGameState(GameState.Playing);
        StartCoroutine(spawning);

        OnChangedRound?.Invoke(currRound + 1);
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
        ObjectPoolManager.Instance.GetParts<Monster>(currWaveMonsterInfo.prefabPath, _onComplete: _newMonster =>
        {
            _newMonster.Init(currWaveStatInfo);
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
            Log.Logger.LogError("스폰된 몬스터 리스트에 추가되지않은 몬스터가 죽었습니다.");


        if (remainSpawnCount > 0) return;
        if (spawnMonsters.Count > 0) return;

        CheckGameEnd();

    }
    private void CheckGameEnd()
    {
        bool _isEnded = currStageInfo.rounds.Length <= currRound + 1;

        if (_isEnded)
        {
            Log.Logger.Log("게임 클리어");
            UIManager.Instance.Show(UIPath.GAMECLEAR, true);
            GameManager.Instance.ChangeGameState(GameState.GameClear);
        }
        else
        {
            GameManager.Instance.ChangeGameState(GameState.BreakTime);
            // StartNextRound();
        }
    }
    public override bool IsDontDestroyOnLoad()
    {
        return false;
    }
}
