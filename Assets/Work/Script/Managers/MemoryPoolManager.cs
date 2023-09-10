using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using framework;


public class MemoryPoolManager : Singleton<MemoryPoolManager>
{
    private MemoryPool<IMemoryPool> memoryPool;

    public void Init()
    {
        memoryPool = new MemoryPool<IMemoryPool>();
    }

    public T Get<T>(string _key) where T : class, IMemoryPool, new()
    {
        return memoryPool.Get<T>(_key);
    }

    public void Release<T>(T _target) where T : class, IMemoryPool
    {
        memoryPool.Release(_target.Key, _target);
    }
    public Debuff GetDebuff(DebuffType _type)
    {
        if (_type == DebuffType.Slow)
            return Get<Debuff>(Constants.DEBUFF_KEY);
        else if (_type == DebuffType.Bleed)
            return Get<Bleed>(Constants.BLEED_KEY);

        return null;
    }
    public void ReleaseDebuff(IMemoryPool _debuff)
    {
        Release(_debuff);
    }
}
