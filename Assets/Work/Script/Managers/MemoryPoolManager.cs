using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using framework;


public class MemoryPoolManager : Singleton<MemoryPoolManager>
{
    private MemoryPool<Debuff> debuffPool;
    private Dictionary<string, MemoryPool<object>> memoryDict;

    public void Init()
    {
        memoryDict = new Dictionary<string, MemoryPool<object>>();
        debuffPool = new MemoryPool<Debuff>();
        debuffPool.AddPool<Debuff>(Constants.DEBUFF_KEY, 10);


    }

    public T Get<T>(string _key) where T : class, new()
    {
        if (memoryDict.ContainsKey(_key))
        {
            return memoryDict[_key].Get<T>(_key);
        }
        else
        {

            MemoryPool<T> _newPool = new MemoryPool<T>();
            _newPool.AddPool<T>(_key, 5);
            memoryDict.Add(_key, _newPool as MemoryPool<object>);
            return _newPool.Get<T>(_key);
        }
    }
    public Debuff GetDebuff()
    {
        return debuffPool.Get<Debuff>(Constants.DEBUFF_KEY);
    }
    public void ReleaseDebuff(Debuff _debuff)
    {
        debuffPool.Release(Constants.DEBUFF_KEY, _debuff);
    }
}
