using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;
using System;

public class ElementsPool
{
    private Dictionary<string, Queue<VisualElement>> poolDict;

    //public static event Action<string, VisualElement> OnReturnPool;
    public ElementsPool()
    {
        poolDict = new Dictionary<string, Queue<VisualElement>>();
    }
    public void AddPool(VisualElement _elemnet, string _key, int _poolingCount)
    {
        if (poolDict == null) poolDict = new Dictionary<string, Queue<VisualElement>>();
        if (poolDict.ContainsKey(_key)) Debug.LogError("이미 존재하는 풀링 ID 입니다.");


        Queue<VisualElement> _elementPool = new Queue<VisualElement>();
        for (int i = 0; i < _poolingCount; i++)
        {
            VisualElement _newElement = _elemnet.visualTreeAssetSource.CloneTree();
            _newElement.visible = false;

            _elementPool.Enqueue(_newElement);
        }
    }
    public void AddPool<T>(string _key, int _poolingCount, VisualElement _root) where T : VisualElement, new()
    {
        if (poolDict == null) poolDict = new Dictionary<string, Queue<VisualElement>>();
        if (poolDict.ContainsKey(_key)) Debug.LogError("이미 존재하는 풀링 ID 입니다.");


        Queue<VisualElement> _elementPool = new Queue<VisualElement>();
        for (int i = 0; i < _poolingCount; i++)
        {
            T _newElement = new T();
            _elementPool.Enqueue(_newElement);
            _newElement.visible = false;
            _root.Add(_newElement);
        }
        poolDict.Add(_key, _elementPool);
    }
    public T GetParts<T>(string _key) where T : VisualElement
    {
        if (poolDict == null) return null;

        if (!poolDict.ContainsKey(_key)) return null;
        if (poolDict[_key].Count <= 0) return null;
        T _result = poolDict[_key].Dequeue() as T;
        if (!_result.visible) _result.visible = true;

        return _result;
    }
    public void ReturnParts<T>(string _key, T _element) where T : VisualElement
    {
        if (!poolDict.ContainsKey(_key)) return;

        _element.RemoveFromHierarchy();
        _element.visible = false;
        poolDict[_key].Enqueue(_element);
    }

    // public VisualElement GetElement(string _key)
    // {
    //     if (!poolDict.ContainsKey(_key))
    //         return null;

    //     Queue<VisualElement> _pool = poolDict[_key];

    //     if (_pool.Count > 0)
    //     {
    //         VisualElement _element = _pool.Dequeue();
    //         _element.visible = true;
    //         return _element;
    //     }
    //     else
    //     {

    //     }

    // }
}