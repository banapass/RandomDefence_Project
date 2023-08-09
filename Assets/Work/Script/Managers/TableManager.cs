using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class TableManager : framework.Singleton<TableManager>
{
    private Dictionary<string, MonsterInfo> monsterDict;
    private Dictionary<string, StageInfo> stageDict;

    private readonly string STAGEDATA_PATH = "Data/StageData";
    private readonly string MONSTERDATA_PATH = "Data/MonsterData";

    [SerializeField] StageInfo stageinfo;
    public void Init()
    {

        // StageInfo 파싱

        StageInfo[] _stages = JsonWrapper.FromJson<StageInfo>(Resources.Load<TextAsset>(STAGEDATA_PATH).text);
        stageDict = new Dictionary<string, StageInfo>();

        for (int i = 0; i < _stages.Length; i++)
        {
            if (stageDict.ContainsKey(_stages[i].stageId))
            {
                Debug.LogError("동일한 스테이지 아이디가 존재합니다. 확인 필요");
                continue;
            }
            else
            {
                stageDict.Add(_stages[i].stageId, _stages[i]);
            }

        }


        // MonsterInfo 파싱
        MonsterInfo[] _monsterInfos = Parse<MonsterInfo>(MONSTERDATA_PATH);
        monsterDict = new Dictionary<string, MonsterInfo>();

        for (int i = 0; i < _monsterInfos.Length; i++)
        {
            MonsterInfo _monsterInfo = _monsterInfos[i];

            if (monsterDict.ContainsKey(_monsterInfo.monsterId))
            {
                Debug.LogError("동일한 몬스터 아이디가 존재합니다. 확인 필요");
                continue;
            }
            else
            {
                monsterDict.Add(_monsterInfo.monsterId, _monsterInfo);
            }

        }


        Monster _monster = framework.ResourceStorage.GetResource<Monster>(monsterDict["1001"].prefabPath);
        Debug.Log(_monster);

    }

    private T[] Parse<T>(string _path)
    {
        T[] _parse = JsonWrapper.FromJson<T>(Resources.Load<TextAsset>(_path).text);
        return _parse;
    }

    public List<MonsterInfo> GetAllMonsterInfo()
    {
        List<MonsterInfo> _result = new List<MonsterInfo>();

        foreach (var _monster in monsterDict)
            _result.Add(_monster.Value);


        return _result;
    }

    public StageInfo GetStageInfo(string _stageId)
    {
        return stageDict[_stageId];
    }
    public MonsterInfo GetMonsterInfo(string _monsterId)
    {
        return monsterDict[_monsterId];
    }
}
