using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPool<T> where T : class, new()
{
    private Dictionary<string, Queue<T>> poolDict;

    public MemoryPool()
    {
        poolDict = new Dictionary<string, Queue<T>>();
    }
    public void AddPool<Child>(string _key, int _count) where Child : T, new()
    {
        if (poolDict.ContainsKey(_key))
        {
            Debug.LogError("같은 Key값의 Pool 추가하려고 하고 있습니다");
            return;
        }

        poolDict.Add(_key, new Queue<T>());

        for (int i = 0; i < _count; i++)
            poolDict[_key].Enqueue(new Child());
    }

    public Child Get<Child>(string _key) where Child : T, new()
    {

        if (!poolDict.ContainsKey(_key))
        {
            Debug.LogError("풀안에 존재하지 않는 키값 입니다");
            return default(Child);
        }
        if (poolDict[_key].Count > 0)
            return (Child)poolDict[_key].Dequeue();

        else
            return new Child();

    }
    public void Release(string _key, T _item)
    {
        if (poolDict[_key].Contains(_item))
            Debug.Log("이미 풀안에 들어가 있는 메모리를 다시 넣으려고 하고있습니다.");

        poolDict[_key].Enqueue(_item);
    }

}
