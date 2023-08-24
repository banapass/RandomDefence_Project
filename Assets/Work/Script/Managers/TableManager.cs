using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using framework;

public class TableManager : framework.Singleton<TableManager>
{
    private Dictionary<string, MonsterInfo> monsterDict;
    private Dictionary<string, StageInfo> stageDict;
    private Dictionary<UnitRarity, List<UnitInfo>> unitRarityMap;
    private UnitRarityInfo[] unitRarities;

    private const string STAGEDATA_PATH = "Data/StageData";
    private const string MONSTERDATA_PATH = "Data/MonsterData";
    private const string UNITDATA_PATH = "Data/UnitData";
    private const string UNITRARITY_PATH = "Data/UnitRarityConfig";

    [SerializeField] StageInfo stageinfo;
    public void Init()
    {

        // StageInfo 파싱
        ResourceStorage.GetObjectRes<TextAsset>(STAGEDATA_PATH, _textAsset =>
        {
            StageInfo[] _stages = Parse<StageInfo>(_textAsset.text);
            stageDict = new Dictionary<string, StageInfo>();

            for (int i = 0; i < _stages.Length; i++)
            {
                if (stageDict.ContainsKey(_stages[i].stageId))
                {
                    Debug.LogError($"동일한 스테이지 아이디가 존재합니다. ID : {_stages[i].stageId}");
                    continue;
                }
                else
                {
                    stageDict.Add(_stages[i].stageId, _stages[i]);
                }
            }
        });



        // MonsterInfo 파싱
        ResourceStorage.GetObjectRes<TextAsset>(MONSTERDATA_PATH, _textAsset =>
        {
            MonsterInfo[] _monsterInfos = Parse<MonsterInfo>(_textAsset.text);
            monsterDict = new Dictionary<string, MonsterInfo>();

            for (int i = 0; i < _monsterInfos.Length; i++)
            {
                MonsterInfo _monsterInfo = _monsterInfos[i];

                if (monsterDict.ContainsKey(_monsterInfo.monsterId))
                {
                    Debug.LogError($"동일한 몬스터 아이디가 존재합니다. ID:{_monsterInfo.monsterId}");
                    continue;
                }
                else
                {
                    monsterDict.Add(_monsterInfo.monsterId, _monsterInfo);
                }
            }
        });


        // UnitInfo 파싱
        ResourceStorage.GetObjectRes<TextAsset>(UNITDATA_PATH, _textAsset =>
        {
            UnitInfo[] _unitInfos = Parse<UnitInfo>(_textAsset.text);
            unitRarityMap = new Dictionary<UnitRarity, List<UnitInfo>>();

            for (int i = 0; i < _unitInfos.Length; i++)
            {
                UnitInfo _currUnit = _unitInfos[i];
                if (!unitRarityMap.ContainsKey(_currUnit.rarity))
                    unitRarityMap.Add(_currUnit.rarity, new List<UnitInfo>());

                unitRarityMap[_unitInfos[i].rarity].Add(_currUnit);
            }
        });



        // UnitRarityInfo 파싱
        ResourceStorage.GetObjectRes<TextAsset>(UNITRARITY_PATH, _textAsset =>
        {
            unitRarities = Parse<UnitRarityInfo>(_textAsset.text);
        });


        // for (int i = 0; i < 100; i++)
        // {
        //     GetRandomUnitInfo();
        // }
    }

    private T[] Parse<T>(string _text)
    {
        T[] _parse = JsonWrapper.FromJson<T>(_text);
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
    public UnitInfo GetRandomUnitInfo()
    {
        UnitRarity _pickedRarity = GetRandomRarity();
        List<UnitInfo> _selectedUnitList = unitRarityMap[_pickedRarity];

        if (_selectedUnitList == null)
            Debug.LogError($"{_pickedRarity}등급에 해당되는 유닛 정보가 없습니다.");


        return _selectedUnitList[Random.Range(0, _selectedUnitList.Count)];
    }
    public UnitInfo GetRandomUnitInfoByRarity(UnitRarity _rarity)
    {
        if (!unitRarityMap.ContainsKey(_rarity))
        {
            Debug.LogError($"{_rarity}등급에 대한 유닛 정보가 존재하지 않습니다");
        }

        List<UnitInfo> _unitList = unitRarityMap[_rarity];

        return _unitList[Random.Range(0, _unitList.Count)];
    }
    private UnitRarity GetRandomRarity()
    {
        float _totalWeight = 0;

        for (int i = 0; i < unitRarities.Length; i++)
            _totalWeight += unitRarities[i].weight;


        float _randomWeight = Random.value * _totalWeight;

        for (int i = 0; i < unitRarities.Length; i++)
        {
            if (_randomWeight < unitRarities[i].weight)
                return unitRarities[i].rarity;
            else
                _randomWeight -= unitRarities[i].weight;
        }

        return UnitRarity.Legendary;
    }

}
