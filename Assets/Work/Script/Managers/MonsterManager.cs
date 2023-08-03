using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    [SerializeField] Monster monsterPrefab;
    public void Init()
    {

    }
    private void SpawnMonster()
    {
        Monster _newMonster = Instantiate(monsterPrefab);
        _newMonster.Init(new MonsterInfo(), null);
    }
}
