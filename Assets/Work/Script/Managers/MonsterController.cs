using System.Collections;
using UnityEngine;
using framework;

public class MonsterController : Singleton<MonsterController>
{
    private BoardManager boardManager;
    [SerializeField] Monster monsterPrefab;

    public void Init(BoardManager _board)
    {
        this.boardManager = _board;
        monsterPrefab = framework.ResourceStorage.GetResource<Monster>("Prefab/Monster");

        StartCoroutine(Spawning());
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
        //FIXME: 이후 몬스터 데이터 연동 필요
        Monster _newMonster = Instantiate(monsterPrefab);
        _newMonster.Init(new MonsterInfo(), boardManager.GetCurrentPath());
    }
}
