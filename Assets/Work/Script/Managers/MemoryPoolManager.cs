using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using framework;

public class MemoryPoolManager : Singleton<MemoryPoolManager>
{
    private MemoryPool<Debuff> debuffPool;

    public void Init()
    {
        debuffPool = new MemoryPool<Debuff>();
        debuffPool.AddPool<Debuff>(Constants.DEBUFF_KEY, 10);
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
